/************************************************************************
* Copyright (c) 2019 All Rights Reserved.
*命名空间：Lucky.Project.Services
*文件名： BaseUserRoleService
*创建人： Lxsh
*创建时间：2019/3/26 12:36:44
*描述
*=======================================================================
*修改标记
*修改时间：2019/3/26 12:36:44
*修改人：Lxsh
*描述：
************************************************************************/
using Lucky.Proect.Core.Repository;
using Lucky.Project.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Lucky.Project.IServices;
using Lucky.Proect.Core.Cache;

namespace Lucky.Project.Services
{
   public class BaseUserRoleService:IBaseUserRoleService
    {

        private const string MODEL_KEY = " Lucky.Project.Services.BaseUserRoleService";

        private ICache _memoryCache;  
        private readonly IRepository<Base_UserRoleMap> _BaseUserRoleMapRepository;
        public BaseUserRoleService(IRepository<Base_UserRoleMap> BaseUserRoleMapRepository,  ICache memoryCache)
        {
            _BaseUserRoleMapRepository = BaseUserRoleMapRepository;
            _memoryCache = memoryCache;
        }
        public List<Base_UserRoleMap> GetRolesByUserId(string UserId)
        {
            return _BaseUserRoleMapRepository.Table.Where(o => o.UserId == UserId).ToList();
        }
        /// <summary>
        /// 获取所有的菜单数据 并缓存
        /// </summary>
        /// <returns></returns>
        public List<Base_UserRoleMap> getAll()
        {
            List<Base_UserRoleMap> list = _memoryCache.GetCache<List<Base_UserRoleMap>>(MODEL_KEY);
            if (list != null)
                return list;
            list = _BaseUserRoleMapRepository.Table.ToList();
            _memoryCache.SetCache(MODEL_KEY, list, DateTimeOffset.Now.AddDays(1));
            return list;
        }
    }
}