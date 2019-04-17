/************************************************************************
* Copyright (c) 2019 All Rights Reserved.
*命名空间：Lucky.Project.Services
*文件名： BaseRolePermission
*创建人： Lxsh
*创建时间：2019/3/26 15:36:19
*描述
*=======================================================================
*修改标记
*修改时间：2019/3/26 15:36:19
*修改人：Lxsh
*描述：
************************************************************************/
using Lucky.Proect.Core.Cache;
using Lucky.Proect.Core.Repository;
using Lucky.Project.IServices;
using Lucky.Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lucky.Project.Services
{
  public  class BaseRolePermissionService:IBaseRolePermissionService
    {
         private ICache _memoryCache;
         private IRepository<Base_RolePermission> _BaseRolePermissionRepository;

        private const string MODEL_KEY = "Lucky.Project.Services.BaseRolePermissionService";
        public BaseRolePermissionService(ICache cache, IRepository<Base_RolePermission> BaseRolePermissionRepository )
        {
            this._BaseRolePermissionRepository = BaseRolePermissionRepository;
            this._memoryCache = cache;
        }

        public List<Base_RolePermission> getAll()
        {
            List<Base_RolePermission> list = _memoryCache.GetCache<List<Base_RolePermission>>(MODEL_KEY);
            if (list != null) return list;

            list = _BaseRolePermissionRepository.Table.ToList();
            _memoryCache.SetCache(MODEL_KEY, list, DateTimeOffset.Now.AddDays(1));
            return list;
        }

        public void removeCache()
        {
            this._memoryCache.RemoveCache(MODEL_KEY);
        }

        public List<Base_RolePermission> getByRoleId(string id)
        {
            var list = getAll();
            if (list == null) return null;
            return list.Where(o => o.RoleId == id).ToList();
        }
        /// <summary>
        /// 保存角色权限数据
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="categoryIds"></param>
        /// <param name="creator"></param>
        public void saveRolePermission(string roleId, List<string> menuIds, string creator)
        {
            var list = _BaseRolePermissionRepository.Table.Where(o => o.RoleId == roleId);
            if (menuIds == null || !menuIds.Any())
                foreach (var del in list)
                    _BaseRolePermissionRepository.Entities.Remove(del);
            else
            {
                foreach (string menuId in menuIds)
                {
                    var item = list.FirstOrDefault(o => o.MenuId == menuId);
                    if (item == null)
                    {
                        _BaseRolePermissionRepository.Entities.Add(new Base_RolePermission()
                        {
                            Id = Guid.NewGuid().ToString(),
                            RoleId = roleId,
                            MenuId = menuId, 
                        });
                    }
                }
                foreach (var del in list)
                    if (!menuIds.Any(o => o == del.MenuId))
                        _BaseRolePermissionRepository.Entities.Remove(del);
            }
            _BaseRolePermissionRepository.DbContext.SaveChanges();
            removeCache();
        }
       
    }
}