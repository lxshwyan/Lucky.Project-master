/************************************************************************
* Copyright (c) 2019 All Rights Reserved.
*命名空间：Lucky.Project.Services
*文件名： BaseOperateLogService
*创建人： Lxsh
*创建时间：2019/4/15 15:11:26
*描述
*=======================================================================
*修改标记
*修改时间：2019/4/15 15:11:26
*修改人：Lxsh
*描述：
************************************************************************/
using Lucky.Proect.Core;
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
    public class BaseOperateLogService : IBaseOperateLogService
    {
       private IRepository<Base_OperateLog> _BaseOperateLog;
        public BaseOperateLogService(IRepository<Base_OperateLog> BaseOperateLog)
        {
            _BaseOperateLog = BaseOperateLog;
        }
        public List<Base_OperateLog> getAll()
        {
            throw new NotImplementedException();
        }

        public TableDataModel SearchLog(Base_OperateLogSearchArg arg)
        {
            var query = _BaseOperateLog.Table;
            if (arg != null)
            {
                if (!string.IsNullOrEmpty(arg.UserId))
                    query = query.Where(o => o.UserId.Contains(arg.UserId));
                if (!string.IsNullOrEmpty(arg.IpAddress))
                    query = query.Where(o => o.IpAddress.Contains(arg.IpAddress));
                if (arg.CreateTime.HasValue)
                    query = query.Where(o => o.CreateTime > arg.CreateTime);
                if (!string.IsNullOrEmpty(arg.OperateCotent))
                    query = query.Where(o => o.OperateCotent.Contains(arg.OperateCotent));
                if (!string.IsNullOrEmpty(arg.OperateCotent))
                    query = query.Where(o => o.OperateCotent.Contains(arg.OperateCotent));  
            }
            query = query.OrderByDescending(o => o.CreateTime).ThenBy(o => o.OperateType);


          
            var list = new PagedList<Base_OperateLog>(query, arg.Page, arg.Limit);
            return new TableDataModel
            {
                count = list.TotalCount,
                data = list.Data,
            };
            //   return new PagedList<Base_OperateLog>(query, arg.Page, arg.Limit);


        }
    }
}