/************************************************************************
* Copyright (c) 2019 All Rights Reserved.
*命名空间：Lucky.Core.DapperHelper
*文件名： ConnectionFactory
*创建人： Lxsh
*创建时间：2019/1/15 11:19:54
*描述
*=======================================================================
*修改标记
*修改时间：2019/1/15 11:19:54
*修改人：Lxsh
*描述：
************************************************************************/
using Lucky.Proect.Core.Extensions;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;  
using Npgsql;



namespace Lucky.Proect.Core.DapperHelper
{
   public class ConnectionFactory
    {
        /// <summary>
        /// 获取数据库连接
        /// </summary>
        /// <param name="dbtype">数据库类型</param>
        /// <param name="strConn">数据库连接字符串</param>
        /// <returns>数据库连接</returns>
        public static IDbConnection CreateConnection(string dbtype, string strConn)
        {
            if (dbtype.IsNullOrWhiteSpace())
                throw new ArgumentNullException("获取数据库连接居然不传数据库类型，你想上天吗？");
            if (strConn.IsNullOrWhiteSpace())
                throw new ArgumentNullException("获取数据库连接居然不传数据库类型，你想上天吗？");
            var dbType = GetDataBaseType(dbtype);
            return CreateConnection(dbType, strConn);
        }
        /// <summary>
        ///   获取数据库连接
        /// </summary>
        /// <param name="dbType">数据库类型</param>
        /// <param name="strConn">数据库连接字符串</param>
        /// <returns></returns>
        public static IDbConnection CreateConnection(DatabaseType dbType, string strConn)
        {
            IDbConnection connection = null;
            if (strConn.IsNullOrWhiteSpace())
                throw new ArgumentNullException("获取数据库连接居然不传数据库类型，你想上天吗？");
            switch (dbType)
            {
                case DatabaseType.SqlServer:
                    connection = new SqlConnection(strConn);
                    break;
                case DatabaseType.MySQL:
                    connection = new MySqlConnection(strConn);
                    break;
                case DatabaseType.PostgreSQL:
                    connection = new NpgsqlConnection(strConn);
                    break;
                default:
                    throw new ArgumentNullException($"暂时还不支持的{dbType.ToString()}数据库类型");

            }
            if (connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }
            return connection;
        }
        /// <summary>
        /// 转换数据库类型
        /// </summary>
        /// <param name="dbtype">数据库类型字符串</param>
        /// <returns>数据库类型</returns>
        public static DatabaseType GetDataBaseType(string dbtype="1")
        {
            if (dbtype.IsNullOrWhiteSpace())
                throw new ArgumentNullException("获取数据库连接居然不传数据库类型，你想上天吗？");
            DatabaseType returnValue = DatabaseType.SqlServer;
            foreach (DatabaseType dbType in Enum.GetValues(typeof(DatabaseType)))
            {
                if (((int)dbType).ToString()== dbtype)
                {
                    returnValue = dbType;
                    break;
                }
            }
            return returnValue;
        }
    }
}