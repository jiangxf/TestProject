using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Web;
using System.Data;

namespace COM.Utility
{
    public class FastJson : IJsonSerializer
    {
        private fastJSON.JSONParameters _paremeters = null;
        public FastJson()
        {
            _paremeters = new fastJSON.JSONParameters();
            _paremeters.DateTimeMilliseconds = false;
            _paremeters.IgnoreCaseOnDeserialize = true;
            _paremeters.UseUTCDateTime = true;
            _paremeters.UsingGlobalTypes = false;
            _paremeters.EnableAnonymousTypes = false;
            _paremeters.KVStyleStringDictionary = true;
            _paremeters.UseFastGuid = false;
            _paremeters.UseValuesOfEnums = true;
            _paremeters.InlineCircularReferences = false;
        }

        /// <summary        /// 将C#数据实体转化为JSON数据
        /// </summary>
        /// <param name="obj">要转化的数据实体</param>
        /// <returns>JSON格式字符串</returns>
        public string SerializeObject(object obj)
        {
            try
            {
                return fastJSON.JSON.ToNiceJSON(obj, _paremeters);
            }
            catch (System.Exception ex)
            {
                return "";
            }
        }

        /// <summary>
        /// 将JSON数据转化为C#数据实体
        /// </summary>
        /// <param name="json">符合JSON格式的字符串</param>
        /// <returns>T类型的对象</returns>
        public T DeserializeObject<T>(string json)
        {
            try
            {
                return fastJSON.JSON.ToObject<T>(json, _paremeters);
            }
            catch (System.Exception ex)
            {
                return default(T);
            }
        }

        public object GetValue(string json)
        {
            object result = fastJSON.JSON.Parse(json);
            return result;
        }


        public T GetValue<T>(string json, params string[] keys)
        {
            if (keys == null || keys.Length == 0)
            {
                return default(T);
            }

            string temp = json;
            foreach (string key in keys)
            {
                temp = GetValue(temp, key);
                if (string.IsNullOrEmpty(temp))
                {
                    return default(T);
                }
            }
            return DeserializeObject<T>(temp);
        }

        private string GetValue(string json, string key)
        {
            object result = fastJSON.JSON.Parse(json);
            foreach (KeyValuePair<string, object> kv in (Dictionary<string, object>)result)
            {
                if (kv.Key == key)
                    return fastJSON.JSON.ToJSON(kv.Value);
            }
            return "";
        }
    }
}
