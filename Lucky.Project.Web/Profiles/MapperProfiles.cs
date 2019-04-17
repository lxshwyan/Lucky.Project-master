using AutoMapper;
using Lucky.Project.Models;
using Lucky.Project.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lucky.Project.Web.Profiles
{
    public class MapperProfiles : Profile
    {
        public MapperProfiles()
        {

            #region Menu    
            CreateMap<Base_Menu, MenuNavView>();
            #endregion
       
        }
    }
}
