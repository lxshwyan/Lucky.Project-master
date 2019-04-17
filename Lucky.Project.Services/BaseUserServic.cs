using Lucky.Proect.Core.Cache;
using Lucky.Proect.Core.Helper;
using Lucky.Proect.Core.Models;
using Lucky.Proect.Core.Repository;
using Lucky.Project.IServices;
using Lucky.Project.Models;
using Lucky.Project.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace Lucky.Project.Services
{
    
    public class BaseUserServic : IBaseUserService
    {
         private IRepository<Base_User> _sysUserRepository;
         private IRepository<Base_UserToken> _sysUserTokenRepository;
         private IRepository<Base_OperateLog> _sysOperateLogRepository;
         private const string MODEL_KEY = "Lucky.Project.Services.user_{0}";
         private ICache _cache;
        private IHttpContextAccessor _httpContextAccessor;
        public BaseUserServic(IHttpContextAccessor httpContextAccessor, IRepository<Base_User> sysUserRepository,IRepository<Base_OperateLog> sysOperateLogRepository,IRepository<Base_UserToken> sysUserTokenRepository,ICache cache)
        {
              this._sysUserRepository = sysUserRepository;
              this._sysUserTokenRepository = sysUserTokenRepository;
              this._sysOperateLogRepository = sysOperateLogRepository;
              this._cache = cache;
            this._httpContextAccessor = httpContextAccessor;
        } 
        /// <summary>
        /// 验证登录状态
        /// </summary>
        /// <param name="account">登录账号</param>
        /// <param name="password">登录密码</param>
        /// <param name="r">登录随机数</param>
        /// <returns></returns>
        public ValidLoginModel<Base_User> ValidateUser(string account, string password, string r)
        {
            ValidLoginModel<Base_User> loginModel = new ValidLoginModel<Base_User>();
            loginModel.Success = false;
            loginModel.Token = "";
            loginModel.User = null;
            var user = GetUserInfoByAccount(account); 
            if (user == null)
            {
               loginModel.Message = "未找到对应帐号";   
               return loginModel;
            }   
            if (!user.Enabled)
            {
               loginModel.Message = "你的账号已被冻结";   
                return loginModel;
            }
            if (user.LoginLock)
            {
                if (user.AllowLoginTime > DateTime.Now)
                {
                    loginModel.Message = "账号已被锁定" + ((int) (user.AllowLoginTime - DateTime.Now).Value.TotalSeconds + 1) + "秒。";
                    return loginModel;
                }
            }
            var md5Password = EncryptorHelper.GetMD5(user.Password + r);
            //匹配密码
            if (password.Equals(md5Password, StringComparison.InvariantCultureIgnoreCase))
            {
                user.LoginLock = false;
                user.LoginFailedNum = 0;
                user.AllowLoginTime = null;
                user.LastLoginTime = DateTime.Now;
                user.LastIpAddress = "";
                loginModel.Success = true; 
                loginModel.Message = "登录成功";
                loginModel.Token  = Guid.NewGuid().ToString();
                loginModel.User = user;
                _sysUserTokenRepository.DbContext.Add(new Base_UserToken()
                {
                    Id = loginModel.Token,
                    ExpireTime = DateTime.Now.AddDays(15),
                    UserId = user.Id
                });     
                _sysOperateLogRepository.DbContext.Add(new Base_OperateLog()
                {
                       Id = Guid.NewGuid().ToString(),
                       UserId =user.Id,
                       CreateTime = DateTime.Now,
                       OperateCotent = "登录成功",
                       OperateType ="LoginIn",
                       IpAddress=this._httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString() 
                });
            }
            else
            {
                user.LoginFailedNum++;
                if (user.LoginFailedNum >4)
                {
                    user.LoginLock = true;
                    user.AllowLoginTime = DateTime.Now.AddMinutes(2);
                    user.LoginFailedNum = 0;
                    loginModel.Message = "账号已被锁定,请2分钟后再登录" ;
                }
                else
                {
                    loginModel.Message =$"登录密码错误，还有{5-user.LoginFailedNum}次机会";
                }
              
            }      
            _sysUserRepository.DbContext.SaveChanges();
            return loginModel;
        }
        /// <summary>
        ///  通过账号获取用户信息
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public Base_User GetUserInfoByAccount(string account)
        {
            return _sysUserRepository.Table.FirstOrDefault(o => (o.Account == account||o.MobilePhone== account) && !o.IsDeleted);
        }

        public Base_User GetUserInfoByID(Guid id)
        {
            throw new NotImplementedException();
        }

        public Base_User GetUserInfoBytoken(string token)
        {
            Base_UserToken userToken = null;
            Base_User sysUser = null;   
            userToken= _cache.GetCache<Base_UserToken>(token);

            if (userToken != null)
            {
               sysUser= _cache.GetCache<Base_User>(String.Format(MODEL_KEY, userToken.UserId));
            }
            if (sysUser != null)
                return sysUser;

            var tokenItem = _sysUserTokenRepository.Table.Where(u => u.Id == token);

            var SysUser = _sysUserTokenRepository.Table.Join(_sysUserRepository.Table, u => u.UserId, g => g.Id, (c, g) => g).FirstOrDefault();
            if (tokenItem != null)
            {
                _cache.SetCache(token, tokenItem, DateTimeOffset.Now.AddHours(4));
                //缓存
                _cache.SetCache(String.Format(MODEL_KEY, SysUser.Id), SysUser, DateTimeOffset.Now.AddHours(4));
                return SysUser;
            }
          

            return null;
        }
        public ValidLoginModel<Base_User> ChangePassword(ChangePasswordModel model)
        {
            ValidLoginModel<Base_User> loginModel = new ValidLoginModel<Base_User>();
            loginModel.Success = false;
            loginModel.Token = model.token;
            loginModel.User = null;
            var user = GetUserInfoByAccount(model.Accont);
            user.Password = EncryptorHelper.GetMD5(model.NewPassword+ user.Salt);   
            _sysUserRepository.update(user, true);
            //缓存
            _cache.SetCache(String.Format(MODEL_KEY, user.Id), user, DateTimeOffset.Now.AddHours(4));
            loginModel.User = user;
            return loginModel;

        }
    }
}
