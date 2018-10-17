using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using SysResourceManager = System.Resources.ResourceManager;

namespace DataBaseTools.Common
{
    public class ResourceManager
    {
        private static string _defaultAssemblyName;
        private static string _defaultResourceName;
        private static object locker = new object();

        private static SysResourceManager _resourceManager;

        /// <summary>
        /// 设置默认资源文件，并初始化默认的资源文件对象实例
        /// 注意默认的资源文件对象实例只被初始化一次
        /// </summary>
        /// <param name="assemblyName"></param>
        /// <param name="resourceName"></param>
        public static void SetDefaultResource(string resourceName, string assemblyName)
        {
            _defaultAssemblyName = assemblyName;
            _defaultResourceName = resourceName;
            GetInstance();
        }

        /// <summary>
        /// 获取默认资源对象实例
        /// </summary>
        public static SysResourceManager GetInstance()
        {
            if (_resourceManager == null)
            {
                lock (locker)
                {
                    if (_resourceManager == null)
                    {
                        _resourceManager = new SysResourceManager(_defaultResourceName, Assembly.Load(_defaultAssemblyName));
                    }
                }
            }

            return _resourceManager;
        }

        /// <summary>
        /// 获取默认资源对象实例
        /// </summary>
        public static SysResourceManager GetInstance( string resourceName, string assemblyName)
        {
            return new SysResourceManager(resourceName,Assembly.Load(assemblyName));
        }

        /// <summary>
        /// 根据key值获取默认的资源中对应的value
        /// </summary>
        /// <param name="key">key值</param>
        /// <param name="defaultValue">当资源中不存在时返回的默认值</param>
        /// <returns></returns>
        public static string GetResourse(string key,string defaultValue = "")
        {
            try
            {
                return _resourceManager.GetString(key);
            }
            catch (Exception)
            {
                if (string.IsNullOrEmpty(defaultValue))
                {
                    return "";
                }

                return defaultValue;
            }
        }

        /// <summary>
        /// 根据key值获取默认的资源中对应的value
        /// </summary>
        /// <param name="key"></param>
        /// <param name="cultureInfoStr">语言信息字符串，如：zh-CN</param>
        /// <param name="defaultValue">当资源中不存在时返回的默认值</param>
        /// <returns></returns>
        public static string GetResourseWithCulture(string key,string cultureInfoStr, string defaultValue = "")
        {
            CultureInfo cultureInfo = null;
            try
            {
                cultureInfo = new CultureInfo(cultureInfoStr);
                var result = _resourceManager.GetString(key,cultureInfo);
                if (string.IsNullOrEmpty(result))
                {
                    return defaultValue;
                }

                return result;
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }
    }
}
