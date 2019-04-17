/************************************************************************
* Copyright (c) 2019 All Rights Reserved.
*命名空间：Lucky.Project.ViewModels
*文件名： Base_OperateLogSearchArg
*创建人： Lxsh
*创建时间：2019/4/15 15:08:22
*描述
*=======================================================================
*修改标记
*修改时间：2019/4/15 15:08:22
*修改人：Lxsh
*描述：
************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;

namespace Lucky.Project.ViewModels
{
  public  class Base_OperateLogSearchArg:PageModel
    {
      
        public String UserId { get; set; }
        public string OperateType { get; set; }
        public string OperateCotent { get; set; }
        public DateTime? CreateTime { get; set; }
        public string IpAddress { get; set; }
    }
}