using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseTools.Common
{
    public class ResourceManager
    {
        private static System.Resources.ResourceManager _resourceManager = new System.Resources.ResourceManager("DataBaseTools.UI.Languages.lang", Assembly.Load("DataBaseTools.UI"));

        public static string GetResourseString(string key)
        {
            CultureInfo cultureInfo = null;
            try
            {
                //string languageCode = this.LanguageCode;
                //cultureInfo = new CultureInfo(languageCode);
                return _resourceManager.GetString(key, cultureInfo);
            }
            catch (Exception)
            {
                //默认读取英文的多语言
                //cultureInfo = new CultureInfo(MKey.kDefaultLanguageCode);
                return _resourceManager.GetString(key, cultureInfo);
            }
        }
    }
}
