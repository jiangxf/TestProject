using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace COM.Utility
{
    public static class JsonHelper
    {
        private static IJsonSerializer _jsonSerializer = null;
        private static JsonType _jsonType = JsonType.Newtonsoft;

        public static JsonType JsonType
        {
            get { return JsonHelper._jsonType; }
            set
            {
                JsonHelper._jsonType = value;
                InitJson();
            }
        }

        static JsonHelper()
        {
            InitJson();
        }

        private static void InitJson()
        {
            if(_jsonType == JsonType.Newtonsoft)
            {
                _jsonSerializer = new NewtonsoftJson();
            }
            else if(_jsonType == JsonType.ServiceStack)
            {
                _jsonSerializer = new ServiceStackJson();
            }
            else if (_jsonType == JsonType.FastJson)
            {
                _jsonSerializer = new FastJson();
            }
        }

        public static string SerializeObject(object obj)
        {
            return _jsonSerializer.SerializeObject(obj);
        }

        public static T DeserializeObject<T>(string json)
        {
            try
            {
                return _jsonSerializer.DeserializeObject<T>(json);
            }
            catch (System.Exception ex)
            {
                return default(T);
            }
        }

        public static object GetValue(string json)
        {
            try
            {
                return _jsonSerializer.GetValue(json);
            }
            catch (System.Exception ex)
            {
                return null;
            }
        }

        public static T GetValue<T>(string json, params string[] keys)
        {
            try
            {
                return _jsonSerializer.GetValue<T>(json, keys);
            }
            catch (System.Exception ex)
            {
                return default(T);
            }
        }

            //public static HttpResponseMessage ToJson(object obj)
            //{
            //    String json;
            //    if (obj is String || obj is Char)
            //    {
            //        json = obj.ToString();
            //    }
            //    else
            //    {
            //        json = SerializeObject(obj);
            //    }
            //    HttpResponseMessage result = new HttpResponseMessage { Content = new StringContent(json, Encoding.GetEncoding("UTF-8"), "application/json") };
            //    return result; 
            //}
        }
}
