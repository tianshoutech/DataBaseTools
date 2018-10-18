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
        private static string _connstr = "localhost";//ConfigurationManager.AppSettings["DefaultRedisConn"];

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

        public static IServer GetServer()
        {
            var redis = GetConnectionMultiplexer();
            return redis.GetServer(_connstr,6379);
        }

        #region key operation
        public static async Task<RedisType> GetKeyType(RedisKey key)
        {
            var db = GetDataBase();
            return await db.KeyTypeAsync(key); 
        }

        public static IEnumerable<RedisKey> GetAllKeys()
        {
            var server = GetServer();
            return server.Keys(0, default(RedisValue), 20, CommandFlags.None);
        }
        #endregion

        #region string operation
        public static async Task<RedisValue> GetValueAsync(RedisKey key)
        {
            var db = GetDataBase();
            return await db.StringGetAsync(key);
        }
        #endregion
    }
}
