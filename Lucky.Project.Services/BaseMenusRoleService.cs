/************************************************************************
* Copyright (c) 2019 All Rights Reserved.
*命名空间：Lucky.Project.Services
*文件名： BaseUserRoleService
*创建人： Lxsh
*创建时间：2019/3/26 11:06:06
*描述
*=======================================================================
*修改标记
*修改时间：2019/3/26 11:06:06
*修改人：Lxsh
*描述：
************************************************************************/
using AutoMapper;
using Lucky.Proect.Core.Repository;
using Lucky.Project.IServices;
using Lucky.Project.Models;
using Lucky.Project.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lucky.Project.Services
{
    public class BaseMenusRoleService : IBaseMenusRoleService
    {
        private readonly IRepository<Base_RolePermission> _BaseRolePermissionRepository;
        private readonly IRepository<Base_Menu> _BaseMenuRepository;
        private readonly IMapper _mapper;
       
        public BaseMenusRoleService(IRepository<Base_RolePermission> BaseRolePermissionRepository,IRepository<Base_Menu> BaseMenuRepository,IMapper mapper)
        {
            _BaseMenuRepository = BaseMenuRepository;
            _BaseRolePermissionRepository = BaseRolePermissionRepository;
               _mapper = mapper;

        }
        public List<MenuNavView> GetMenusByRoleId(string roleId)
        {   
            var menuList = _BaseRolePermissionRepository.Table.Where(s => s.RoleId ==roleId).Join(_BaseMenuRepository.Table.Where(o=>o.IsMenu==true),r=>r.MenuId,m=>m.Id, (c, g) => g).ToList();
            var menuNavViewList = new List<MenuNavView>();
            menuList.ForEach(x =>
            {
                var navView = _mapper.Map<MenuNavView>(x);
                menuNavViewList.Add(navView);
            });
            return menuNavViewList;
         
        }

     
    }
}