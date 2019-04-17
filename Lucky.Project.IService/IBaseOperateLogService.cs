/************************************************************************
* Copyright (c) 2019 All Rights Reserved.
*命名空间：Lucky.Project.IServices
*文件名： IBaseOperateLogService
*创建人： Lxsh
*创建时间：2019/4/15 14:38:32
*描述
*=======================================================================
*修改标记
*修改时间：2019/4/15 14:38:32
*修改人：Lxsh
*描述：
************************************************************************/
using Lucky.Project.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Lucky.Project.ViewModels;
using Lucky.Proect.Core;

namespace Lucky.Project.IServices
{
 public  interface IBaseOperateLogService
    {
        List<Base_OperateLog> getAll();

        TableDataModel SearchLog(Base_OperateLogSearchArg arg);
    }
}