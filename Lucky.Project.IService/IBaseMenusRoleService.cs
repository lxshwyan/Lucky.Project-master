/************************************************************************
* Copyright (c) 2019 All Rights Reserved.
*命名空间：Lucky.Project.IServices
*文件名： IBaseMenusRoleService
*创建人： Lxsh
*创建时间：2019/3/26 12:35:10
*描述
*=======================================================================
*修改标记
*修改时间：2019/3/26 12:35:10
*修改人：Lxsh
*描述：
************************************************************************/
using Lucky.Project.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lucky.Project.IServices
{
 
    public interface IBaseMenusRoleService
    {

        List<MenuNavView> GetMenusByRoleId(string roleId);
    }
}