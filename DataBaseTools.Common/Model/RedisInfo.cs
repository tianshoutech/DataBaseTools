using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseTools.Common.Model
{
    public class RedisInfo
    {
        /// <summary>
        /// Key值统计
        /// </summary>
        public Dictionary<int,long> Keys { get; set; }
    }

    public class RedisStringModel
    {
        public string key { get; set; }
        public string Value { get; set; }
        public long ExpiredTime { get; set; }
    }

    public enum InfoSection
    {
        Server =0,
        Clients,
        Memory,
        Persistence,
        Stats,
        Replication,
        CPU,
        Cluster,
        KeySpace
    }
}
