using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Web;
using System.Data;

namespace COM.Utility
{
    public class ServiceStackJson : IJsonSerializer
    {
        public ServiceStackJson()
        {
            ServiceStack.Text.JsConfig.Reset();
            ServiceStack.Text.JsConfig.EmitCamelCaseNames = true;
            ServiceStack.Text.JsConfig.DateHandler = ServiceStack.Text.JsonDateHandler.ISO8601;
            //ServiceStack.Text.JsConfig.ConvertObjectTypesIntoStringDictionary = true;
        }

        /// <summary        /// ��C#����ʵ��ת��ΪJSON����
        /// </summary>
        /// <param name="obj">Ҫת��������ʵ��</param>
        /// <returns>JSON��ʽ�ַ���</returns>
        public string SerializeObject(object obj)
        {
            try
            {
                return ServiceStack.Text.JsonSerializer.SerializeToString(obj, obj.GetType());
            }
            catch (System.Exception ex)
            {
                return "";
            }
        }

        /// <summary>
        /// ��JSON����ת��ΪC#����ʵ��
        /// </summary>
        /// <param name="json">����JSON��ʽ���ַ���</param>
        /// <returns>T���͵Ķ���</returns>
        public T DeserializeObject<T>(string json)
        {
            try
            {
                return ServiceStack.Text.JsonSerializer.DeserializeFromString<T>(json);
            }
            catch (System.Exception ex)
            {
                return default(T);
            }
        }

        public object GetValue(string json)
        {
            ServiceStack.Text.JsonObject jObject = ServiceStack.Text.JsonObject.Parse(json);
            return (Dictionary<string, string>)jObject;
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
            ServiceStack.Text.JsonObject jObject = ServiceStack.Text.JsonObject.Parse(json);
            foreach (KeyValuePair<string, string> kv in jObject)
            {
                if (kv.Key == key)
                    return kv.Value;
            }
            return "";
        }
    }
}
