/************************************************************************
* Copyright (c) 2019 All Rights Reserved.
*命名空间：Lucky.Proect.Core
*文件名： ConfigOption
*创建人： Lxsh
*创建时间：2019/3/21 10:09:31
*描述
*=======================================================================
*修改标记
*修改时间：2019/3/21 10:09:31
*修改人：Lxsh
*描述：
************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;

namespace Lucky.Proect.Core
{
    public class ConfigOption
    {
        public string IsUseRedis { get; set; }
        public string Redisconfig { get; set; }
        public string ConnectionString { get; set; }
        public string DbType { get; set; }
         public string RabbitMQconfig { get; set; }
    }
}

