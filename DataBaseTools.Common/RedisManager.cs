using DataBaseTools.Common.Model;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseTools.Common
{
    public class RedisManager
    {
        private static string _connstr = ConfigurationManager.AppSettings["DefaultRedisConn"];

        private static IConnectionMultiplexer _redis;

        private static object locker = new object();

        public static IConnectionMultiplexer GetConnectionMultiplexer()
        {
            if (_redis == null)
            {
                lock (locker)
                {
                    if (_redis == null)
                    {
                        _redis = ConnectionMultiplexer.Connect(_connstr);
                    }
                }
            }
            return _redis;
        }

        public static IConnectionMultiplexer GetConnectionMultiplexer(string ip, int port = 6379)
        {
            return ConnectionMultiplexer.Connect($"{ip}:{port}");
        }

        /// <summary>
        /// get database by index, index should between 0 and 15
        /// default index is 0
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public static IDatabase GetDataBase(int index = 0)
        {
            if (index < 0 || index > 15)
            {
                index = 16;
            }
            var redis = GetConnectionMultiplexer();
            return redis.GetDatabase(index);
        }

        /// <summary>
        /// 获取默认的Server，如果默认的连接字符串中有多个数据库，则默认返回第一个
        /// </summary>
        /// <returns></returns>
        public static IServer GetServer()
        {
            var redis = GetConnectionMultiplexer();
            var hostList = _connstr.Split(',');
            var hostObj = hostList[0].Split(':');
            var ip = hostObj[0];
            var port = 6379;
            if (hostObj.Length > 1)
            {
                port = int.Parse(hostObj[1]);
            }
            return redis.GetServer(ip, port);
        }

        /// <summary>
        /// 获取Server
        /// </summary>
        /// <param name="ip">redis地址</param>
        /// <param name="port">端口</param>
        /// <returns></returns>
        public static IServer GetServer(string ip, int port = 6379)
        {
            var conn = GetConnectionMultiplexer(ip, port);
            return conn.GetServer(ip, port);
        }

        #region key operation
        /// <summary>
        /// 获取Key值的类型
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static async Task<RedisType> GetKeyType(RedisKey key)
        {
            var db = GetDataBase();
            return await db.KeyTypeAsync(key);
        }

        /// <summary>
        /// 获取Keys值
        /// </summary>
        /// <param name="database"></param>
        /// <param name="pattern"></param>
        /// <param name="pageSize"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public static IEnumerable<RedisKey> GetAllKeys(int database = 0, string pattern = "*", int pageSize = 20, int page = 0)
        {
            var server = GetServer();
            return server.Keys(0, pattern, 20, 0, page);
        }

        /// <summary>
        /// 获取数据库Info信息
        /// </summary>
        /// <returns></returns>
        public static RedisInfo GetInfo()
        {
            var server = GetServer();
            var infos = server.Info();
            var result = new RedisInfo();
            result.Keys = new Dictionary<int, long>();
            var keys = infos[(int)InfoSection.KeySpace];
            foreach (var item in keys.AsEnumerable())
            {
                var dbIndex = int.Parse(item.Key.Replace("db", ""));
                var keyInfo = item.Value.Split(',');
                var keyCount = keyInfo[0].Split('=')[1];
                result.Keys.Add(dbIndex, long.Parse(keyCount));
            }
            return result;
        }
        #endregion

        #region string operation
        public static async Task<RedisValue> GetValueAsync(RedisKey key)
        {
            var db = GetDataBase();
            return await db.StringGetAsync(key);
        }

        /// <summary>
        /// 设置值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expiredSecends"></param>
        /// <returns></returns>
        public static async Task<bool> SetValueAsync(RedisKey key, RedisValue value, long expiredSecends)
        {
            var db = GetDataBase();
            return await db.StringSetAsync(key, value, TimeSpan.FromSeconds(expiredSecends));
        }

        public static void MSetValue(List<RedisStringModel> list)
        {
            var db = GetDataBase();
            var batch = db.CreateBatch();
            for (int i = 0; i < list.Count; i++)
            {
                db.StringSet(list[i].key, list[i].Value, TimeSpan.FromSeconds(list[i].ExpiredTime));
            }
            batch.Execute();
        }
        #endregion
    }
}
