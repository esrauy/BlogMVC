using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog_Common.Helpers
{
    public class ConfigHelper
    {
        public string Get1(string key)
        {           //web.config içine giriyor. Tanımlanan değerlere key veriyouz parametre olarak, bize value döndürüyor
            return ConfigurationManager.AppSettings[key];
        }
        public static T Get<T>(string key)
        {
            return (T)Convert.ChangeType(ConfigurationManager.AppSettings[key], typeof(T));
        }
    }
}
