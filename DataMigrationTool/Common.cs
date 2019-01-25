using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace DataMigrationTool
{
    class Common
    {
        public static string GetConfigValue(string key)
        {
            var file = System.Windows.Forms.Application.ExecutablePath;
            var config = ConfigurationManager.OpenExeConfiguration(file);
            return config.AppSettings.Settings.AllKeys.Any(item => item == key) ? config.AppSettings.Settings[key].Value : null;
        }

        public static void SetConfigValueByKey(string key, string value)
        {
            var file = System.Windows.Forms.Application.ExecutablePath;
            var config = ConfigurationManager.OpenExeConfiguration(file);
            if (config.AppSettings.Settings.AllKeys.Any(t => t == key))
            {
                config.AppSettings.Settings[key].Value = value;
            }
            else
            {
                config.AppSettings.Settings.Add(key, value);
            }
            config.Save(ConfigurationSaveMode.Modified);
            System.Configuration.ConfigurationManager.RefreshSection("App");
        }

        public static ResponseResult Get(string url)
        {
            ResponseResult result;
            if (string.IsNullOrEmpty(url))
            {
                return result = new ResponseResult { Status = 0, Info = "url为空", List = null };
            }
            if (!url.StartsWith("http://", StringComparison.CurrentCultureIgnoreCase))
            {
                return result = new ResponseResult { Status = 0, Info = "url格式不正确", List = null };
            }
            var request = WebRequest.CreateHttp(url);
            request.Method = "GET";
            request.Timeout = 5000;
            request.Accept = "application/json";
            try
            {
                var response = request.GetResponse();
                using (var sr = new StreamReader(response.GetResponseStream() ?? throw new InvalidOperationException($"return null"), Encoding.UTF8))
                {
                    result = JsonConvert.DeserializeObject<ResponseResult>(sr.ReadToEnd());
                    if (result == null)
                    {
                        return result = new ResponseResult { Status = 0, Info = "反序列化错误", List = null };
                    }
                }
                response.Close();
                return result;
            }
            catch (Exception e)
            {
                return result = new ResponseResult { Status = 0, Info = e.Message, List = null };
            }
        }
        public static ResponseResult Post(string url, Dictionary<string, string> param)
        {
            ResponseResult result;
            if (string.IsNullOrEmpty(url))
            {
                return result = new ResponseResult { Status = 0, Info = "url为空", List = null };
            }
            if (!url.StartsWith("http://", StringComparison.CurrentCultureIgnoreCase))
            {
                return result = new ResponseResult { Status = 0, Info = "url格式不正确", List = null };
            }
            var request = WebRequest.CreateHttp(url);
            request.Method = "POST";
            request.Timeout = 5000;
            request.Accept = "application/json";
            try
            {
                if (param != null && param.Count > 0)
                {
                    var firstFlag = true;
                    var sb = new StringBuilder();
                    foreach (var item in param)
                    {
                        if (firstFlag)
                        {
                            sb.Append($"{item.Key}={item.Value}");
                            firstFlag = false;
                        }
                        else
                        {
                            sb.Append($"&{item.Key}={item.Value}");
                        }
                    }
                    var data = Encoding.UTF8.GetBytes(sb.ToString());
                    request.GetRequestStream().Write(data, 0, data.Length);
                    request.ContentLength = data.Length;
                }
                var response = request.GetResponse();
                using (var stream = response.GetResponseStream())
                using (var sr = new StreamReader(stream ?? throw new InvalidOperationException($"return null"), Encoding.UTF8))
                {
                    result = JsonConvert.DeserializeObject<ResponseResult>(sr.ReadToEnd());
                    if (result == null)
                    {
                        return result = new ResponseResult { Status = 0, Info = "反序列化错误", List = null };
                    }
                }
                response.Close();
                return result;
            }
            catch (Exception e)
            {
                return result = new ResponseResult { Status = 0, Info = e.Message, List = null };
            }
        }

    }
}
