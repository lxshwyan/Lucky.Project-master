/************************************************************************
* Copyright (c) 2019 All Rights Reserved.
*命名空间：Lucky.Project.IServices
*文件名： IBaseMenus
*创建人： Lxsh
*创建时间：2019/3/26 15:18:01
*描述
*=======================================================================
*修改标记
*修改时间：2019/3/26 15:18:01
*修改人：Lxsh
*描述：
************************************************************************/
using Lucky.Project.Models;
using Lucky.Project.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lucky.Project.IServices
{
  public  interface IBaseMenusService
    {
     
        List<Base_Menu> getAll();
        void initCategory(List<Base_Menu> list);
        List<MenuNavView> getAllMenuNavView();
    }
}