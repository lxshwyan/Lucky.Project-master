using Lucky.Proect.Core.Models;
using System;
using Lucky.Project.Models;
using Lucky.Project.ViewModels;

namespace Lucky.Project.IServices
{
    public interface IBaseUserService
    {
        /// <summary>
        /// 验证登录状态
        /// </summary>
        /// <param name="account">登录账号</param>
        /// <param name="password">登录密码</param>
        /// <param name="r">登录随机数</param>
        /// <returns></returns>
        ValidLoginModel<Base_User> ValidateUser(string account, string password, string r);
        /// <summary>
        /// 通过账号获取用户信息
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        Base_User GetUserInfoByAccount(string account);

        /// <summary>
        /// 通过当前登录用户的token 获取用户信息，并缓存
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        Base_User GetUserInfoBytoken(string token);

        /// <summary>
        /// 获取用户详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Base_User GetUserInfoByID(Guid id);
        /// <summary>
        /// 修改用户名密码
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        ValidLoginModel<Base_User> ChangePassword(ChangePasswordModel model);
    }
}
