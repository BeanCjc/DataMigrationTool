using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Net;

namespace DataMigrationTool
{
    class Common
    {
        public static string GetConfigValue(string key)
        {
            var file = System.Windows.Forms.Application.ExecutablePath;
            var config = ConfigurationManager.OpenExeConfiguration(file);
            foreach (var item in config.AppSettings.Settings.AllKeys)
            {
                if (item == key)
                {
                    return config.AppSettings.Settings[key].Value;
                }
            }
            return null;
        }

        public static ResponseResult Get(string url, Dictionary<string, string> param)
        {
            ResponseResult result = null;
            if (string.IsNullOrEmpty(url))
            {
                return result = new ResponseResult { Status = 0, Info = "url为空", List = null };
            }
            if (!url.StartsWith("http://",StringComparison.CurrentCultureIgnoreCase))
            {
                return result = new ResponseResult() { Status = 0, Info = "url格式不正确", List = null };
            }
            return new ResponseResult { Status = 0, Info = "url为空", List = null };
        }


    }
}
