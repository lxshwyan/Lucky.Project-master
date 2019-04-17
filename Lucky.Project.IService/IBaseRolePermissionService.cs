/************************************************************************
* Copyright (c) 2019 All Rights Reserved.
*命名空间：Lucky.Project.IServices
*文件名： IBaseRolePermission
*创建人： Lxsh
*创建时间：2019/3/26 15:34:54
*描述
*=======================================================================
*修改标记
*修改时间：2019/3/26 15:34:54
*修改人：Lxsh
*描述：
************************************************************************/
using Lucky.Project.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lucky.Project.IServices
{
  public  interface IBaseRolePermissionService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        List<Base_RolePermission> getAll();

        /// <summary>
        /// 移除缓存
        /// </summary>
        void removeCache();

        /// <summary>
        /// 获取角色权限
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        List<Base_RolePermission> getByRoleId(string id);

        /// <summary>
        /// 保存角色权限数据
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="menuIds"></param>
        /// <param name="creator"></param>
        void saveRolePermission(string roleId, List<string> menuIds, string creator);
    }
}