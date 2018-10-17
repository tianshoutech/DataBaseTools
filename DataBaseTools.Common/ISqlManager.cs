using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace DataBaseTools.Common
{
    public interface ISqlManager
    {
        MySqlConnection CreateConnection();
        int ExecuteNonQuery(string cmdStr, CommandType cmdType = CommandType.Text, int outTime = 60, params MySqlParameter[] paras);
        object ExecuteScalar(string cmdStr, CommandType cmdType = CommandType.Text, int outTime = 60, params MySqlParameter[] paras);
        List<DataTable> ExecuteQuery(string cmdStr, CommandType cmdType = CommandType.Text, int outTime = 60, params MySqlParameter[] paras);
        int ExecuteNonQueryWithTranction(string cmdStr, CommandType cmdType = CommandType.Text, params MySqlParameter[] paras);
        int BulkCopy<T>(List<T> list, string tableName);

    }
}
