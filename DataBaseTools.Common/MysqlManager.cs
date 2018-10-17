using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using MySql.Data.MySqlClient;
using System.Data;

namespace DataBaseTools.Common
{
    public class MysqlManager : ISqlManager
    {
        //private IConfiguration configuration;
        private string connStr = ConfigurationManager.ConnectionStrings["mysql"].ToString();

        //public MysqlManager(IConfiguration configuration)
        //{
        //    this.configuration = configuration;
        //    this.connStr = configuration.GetSection("ConnectionStrings").GetValue<string>("mysql");
        //}

        /// <summary>
        /// 创建连接对象
        /// </summary>
        /// <returns></returns>
        public MySqlConnection CreateConnection()
        {
            var conn = new MySqlConnection(connStr);
            conn.Open();
            return conn;
        }

        /// <summary>
        /// 创建连接对象
        /// </summary>
        /// <param name="connStr">连接字符串</param>
        /// <returns></returns>
        public static MySqlConnection CreateConnection(string connStr)
        {
            var conn = new MySqlConnection(connStr);
            conn.Open();
            return conn;
        }

        /// <summary>
        /// 执行数据库sql,返回影响行数
        /// </summary>
        /// <param name="cmdStr">执行的sql语句</param>
        /// <param name="cmdType"></param>
        /// <param name="paras">查询参数</param>
        /// <returns></returns>
        public int ExecuteNonQuery(string cmdStr, CommandType cmdType = CommandType.Text, int outTime = 60, params MySqlParameter[] paras)
        {
            using (var conn = CreateConnection())
            using (var cmd = conn.CreateCommand())
            {
                if (conn.State == ConnectionState.Broken || conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                cmd.CommandText = cmdStr;
                cmd.CommandType = cmdType;
                cmd.CommandTimeout = outTime;
                if (paras != null && paras.Length > 0)
                {
                    cmd.Parameters.Add(paras);
                }
                return cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// 执行数据库sql,返回首行首列单元格内容
        /// </summary>
        /// <param name="cmdStr"></param>
        /// <param name="cmdType"></param>
        /// <param name="paras"></param>
        /// <returns></returns>
        public object ExecuteScalar(string cmdStr, CommandType cmdType = CommandType.Text, int outTime = 60, params MySqlParameter[] paras)
        {
            using (var conn = CreateConnection())
            using (var cmd = conn.CreateCommand())
            {
                if (conn.State == ConnectionState.Broken || conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                cmd.CommandText = cmdStr;
                cmd.CommandType = cmdType;
                cmd.CommandTimeout = outTime;
                if (paras != null && paras.Length > 0)
                {
                    cmd.Parameters.AddRange(paras);
                }
                return cmd.ExecuteScalar();
            }
        }

        /// <summary>
        /// 执行sql查询,返回数据表
        /// </summary>
        /// <param name="cmdStr"></param>
        /// <param name="cmdType"></param>
        /// <param name="outTime"></param>
        /// <param name="paras"></param>
        /// <returns></returns>
        public List<DataTable> ExecuteQuery(string cmdStr, CommandType cmdType = CommandType.Text, int outTime = 60, params MySqlParameter[] paras)
        {
            var dataSet = new DataSet();
            var result = new List<DataTable>();
            using (var conn = CreateConnection())
            using (var cmd = new MySqlCommand(cmdStr, conn))
            {
                cmd.CommandType = cmdType;
                cmd.CommandTimeout = outTime;
                if (paras != null && paras.Length > 0)
                {
                    cmd.Parameters.AddRange(paras);
                }
                using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                {
                    adapter.Fill(dataSet);
                }
            }

            if (dataSet != null && dataSet.Tables != null && dataSet.Tables.Count > 0)
            {
                for (int i = 0; i < dataSet.Tables.Count; i++)
                {
                    result.Add(dataSet.Tables[i]);
                }
            }
            return result;
        }

        /// <summary>
        /// 执行数据库sql,返回影响行数
        /// </summary>
        /// <param name="cmdStr">执行的sql语句</param>
        /// <param name="cmdType"></param>
        /// <param name="paras">查询参数</param>
        /// <returns></returns>
        public static int ExecuteNonQuery(string connStr, string cmdStr, CommandType cmdType = CommandType.Text, int outTime = 60, params MySqlParameter[] paras)
        {
            using (var conn = CreateConnection(connStr))
            using (var cmd = conn.CreateCommand())
            {
                if (conn.State == System.Data.ConnectionState.Broken || conn.State == System.Data.ConnectionState.Closed)
                {
                    conn.Open();
                }
                cmd.CommandText = cmdStr;
                cmd.CommandType = cmdType;
                cmd.CommandTimeout = outTime;
                if (paras != null && paras.Length > 0)
                {
                    cmd.Parameters.Add(paras);
                }
                return cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// 执行数据库sql,返回首行首列单元格内容
        /// </summary>
        /// <param name="cmdStr"></param>
        /// <param name="cmdType"></param>
        /// <param name="paras"></param>
        /// <returns></returns>
        public static object ExecuteScalar(string connStr, string cmdStr, CommandType cmdType = CommandType.Text, int outTime = 60, params MySqlParameter[] paras)
        {
            using (var conn = CreateConnection(connStr))
            using (var cmd = conn.CreateCommand())
            {
                if (conn.State == System.Data.ConnectionState.Broken || conn.State == System.Data.ConnectionState.Closed)
                {
                    conn.Open();
                }
                cmd.CommandText = cmdStr;
                cmd.CommandType = cmdType;
                cmd.CommandTimeout = outTime;
                if (paras != null && paras.Length > 0)
                {
                    cmd.Parameters.AddRange(paras);
                }
                return cmd.ExecuteScalar();
            }
        }

        /// <summary>
        /// 执行sql查询,返回数据表
        /// </summary>
        /// <param name="cmdStr"></param>
        /// <param name="cmdType"></param>
        /// <param name="outTime"></param>
        /// <param name="paras"></param>
        /// <returns></returns>
        public static List<DataTable> ExecuteQuery(string connStr, string cmdStr, CommandType cmdType = CommandType.Text, int outTime = 60, params MySqlParameter[] paras)
        {
            var dataSet = new DataSet();
            var result = new List<DataTable>();
            using (var conn = CreateConnection(connStr))
            using (var cmd = new MySqlCommand(cmdStr, conn))
            {
                cmd.CommandType = cmdType;
                cmd.CommandTimeout = outTime;
                if (paras != null && paras.Length > 0)
                {
                    cmd.Parameters.AddRange(paras);
                }
                using (var adapter = new MySqlDataAdapter(cmd))
                {
                    adapter.Fill(dataSet);
                }
            }

            if (dataSet != null && dataSet.Tables != null && dataSet.Tables.Count > 0)
            {
                for (int i = 0; i < dataSet.Tables.Count; i++)
                {
                    result.Add(dataSet.Tables[i]);
                }
            }
            return result;
        }

        /// <summary>
        /// 执行数据库sql,返回影响行数,带有事务
        /// </summary>
        /// <param name="cmdStr"></param>
        /// <param name="cmdType"></param>
        /// <param name="paras"></param>
        /// <returns></returns>
        public int ExecuteNonQueryWithTranction(string cmdStr, CommandType cmdType = CommandType.Text, params MySqlParameter[] paras)
        {
            using (var conn = CreateConnection())
            using (var sqlTransaction = conn.BeginTransaction())
            using (var cmd = conn.CreateCommand())
            {
                var res = -1;
                try
                {
                    cmd.Transaction = sqlTransaction;
                    cmd.CommandText = cmdStr;
                    cmd.CommandType = cmdType;
                    cmd.CommandTimeout = 30000;
                    if (paras != null && paras.Length > 0)
                    {
                        cmd.Parameters.Add(paras);
                    }

                    if (conn.State == ConnectionState.Broken || conn.State == ConnectionState.Closed)
                    {
                        conn.Open();
                    }
                    res = cmd.ExecuteNonQuery();
                    sqlTransaction.Commit();
                    return res;
                }
                catch (Exception ex)
                {
                    sqlTransaction.Rollback();
                    return res;
                }
            }
        }

        /// <summary>
        /// 批量插入数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public int BulkCopy<T>(List<T> list, string tableName)
        {
            if (list == null || list.Count <= 0 || string.IsNullOrEmpty(tableName))
            {
                return -1;
            }

            var sb = new StringBuilder();
            var sbKey = new StringBuilder();
            var sbValue = new StringBuilder();
            sb.AppendLine(string.Format("INSERT INTO {0}", tableName));
            sbKey.Append("(");
            sbValue.Append("VALUES(");
            var type = typeof(T);
            var propInfos = type.GetProperties();
            for (int i = 0; i < list.Count; i++)
            {
                var obj = list[i];
                if (i == 0)
                {
                    for (int j = 0; j < propInfos.Length; j++)
                    {
                        var prop = propInfos[j];
                        if (prop.Name == "Id")
                        {
                            continue;
                        }

                        // 格式化日期格式
                        var value = prop.GetValue(obj);
                        if (prop.PropertyType == typeof(DateTime))
                        {
                            value = ((DateTime) value).ToString("yyyy-MM-dd HH:mm:ss");
                        }
                        sbKey.Append("`" + prop.Name + "`,");
                        sbValue.Append("'" + value + "',");
                    }
                    sbKey.Remove(sbKey.Length - 1, 1).Append(") ");
                    sbValue.Remove(sbValue.Length - 1, 1).Append("),(");
                }
                else
                {
                    for (int j = 0; j < propInfos.Length; j++)
                    {
                        var prop = propInfos[j];
                        if (prop.Name == "Id")
                        {
                            continue;
                        }

                        // 格式化日期格式
                        var value = prop.GetValue(obj);
                        if (prop.PropertyType == typeof(DateTime))
                        {
                            value = ((DateTime)value).ToString("yyyy-MM-dd HH:mm:ss");
                        }
                        sbValue.Append("'" + value + "',");
                    }
                    sbValue.Remove(sbValue.Length - 1, 1).Append("),(");
                }
            }

            sbValue.Remove(sbValue.Length - 2, 2);
            var str = sb.ToString() + sbKey.ToString() + sbValue.ToString();
            return ExecuteNonQuery(str, CommandType.Text, 300);
        }

        /// <summary>
        /// 是否可以连接
        /// </summary>
        /// <returns></returns>
        public bool CanConnection()
        {
            try
            {
                using (var conn = CreateConnection())
                {
                    conn.Open();
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// 是否可以连接
        /// </summary>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public static bool CanConnection(string connStr)
        {
            try
            {
                using (var conn = CreateConnection(connStr))
                {
                    //conn.Open();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 获取使用Windows用户登录模式时的连接字符串
        /// </summary>
        /// <param name="serverIP"></param>
        /// <returns></returns>
        public static string GetConnectionString(string serverIP)
        {
            return string.Format("Data Source={0};Integrated Security=True;", serverIP);
        }

        /// <summary>
        /// 获取数据库连接字符串
        /// </summary>
        /// <param name="serverIP"></param>
        /// <param name="dbName"></param>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static string GetConnectionString(string serverIP, string dbName, string userName, string password)
        {
            return string.Format("Data Source={0};Initial Catalog={1};User ID={2};Password={3};", serverIP, dbName, userName, password);
        }
    }
}
