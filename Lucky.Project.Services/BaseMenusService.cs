/************************************************************************
* Copyright (c) 2019 All Rights Reserved.
*命名空间：Lucky.Project.Services
*文件名： BaseMenusService
*创建人： Lxsh
*创建时间：2019/3/26 15:19:59
*描述
*=======================================================================
*修改标记
*修改时间：2019/3/26 15:19:59
*修改人：Lxsh
*描述：
************************************************************************/
using AutoMapper;
using Lucky.Proect.Core.Cache;
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
   public  class BaseMenusService:IBaseMenusService
    {

        private const string MODEL_KEY = " Lucky.Project.Services.BaseMenusService";

        private ICache _memoryCache;
        private IRepository<Base_Menu> _BaseMenuRepository;
        private readonly IMapper _mapper;
     

        public BaseMenusService(IRepository<Base_Menu> BaseMenuRepository,
            ICache memoryCache,IMapper mapper)
        {        
            this._memoryCache = memoryCache;
            this._BaseMenuRepository = BaseMenuRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// 初始化保存方法
        /// </summary>
        /// <param name="list"></param>
        public void initCategory(List<Base_Menu> list)
        { 
        }

        /// <summary>
        /// 获取所有的菜单数据 并缓存
        /// </summary>
        /// <returns></returns>
        public List<Base_Menu> getAll()
        {
            List<Base_Menu> list =_memoryCache.GetCache<List<Base_Menu>>(MODEL_KEY);
            if (list != null)
                return list;
            list = _BaseMenuRepository.Table.ToList();
            _memoryCache.SetCache(MODEL_KEY, list, DateTimeOffset.Now.AddDays(1));
            return list;
        }
        /// <summary>
        /// 获取所有的菜单数据 并缓存
        /// </summary>
        /// <returns></returns>
        public List<MenuNavView> getAllMenuNavView()
        {
              
            var menuNavViewList = new List<MenuNavView>();
            var menus = getAll().Where(s => s.IsMenu == true).ToList();
             menus.ForEach(x =>
            {
                var navView = _mapper.Map<MenuNavView>(x);
                menuNavViewList.Add(navView);
            });
            return menuNavViewList;
           
        }
    }
}