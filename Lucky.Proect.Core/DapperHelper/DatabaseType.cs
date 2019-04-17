/************************************************************************
* Copyright (c) 2019 All Rights Reserved.
*命名空间：Lucky.Core.DapperHelper
*文件名： DatabaseType
*创建人： Lxsh
*创建时间：2019/1/15 11:18:41
*描述
*=======================================================================
*修改标记
*修改时间：2019/1/15 11:18:41
*修改人：Lxsh
*描述：
************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lucky.Proect.Core.DapperHelper
{
    public  enum DatabaseType
    {
        MySQL=0,
        SqlServer,
        SQLite,
        Oracle,
        PostgreSQL,
        InMemory, 
        MariaDB,
        MyCat,
        Firebird,
        DB2,
        Access   
    }
}