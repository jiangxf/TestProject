using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Web;
using System.Data;

namespace COM.Utility
{
    public class NewtonsoftJson : IJsonSerializer
    {
        private Newtonsoft.Json.JsonSerializer _newtonsoftJosnSerializer = null;
        private Newtonsoft.Json.JsonSerializerSettings _newtonsoftJsonSettings = null;

        public NewtonsoftJson()
        {
            _newtonsoftJsonSettings = CreateSettings();
            _newtonsoftJosnSerializer = Newtonsoft.Json.JsonSerializer.Create(_newtonsoftJsonSettings);
        }

        public static Newtonsoft.Json.JsonSerializerSettings CreateSettings()
        {
            Newtonsoft.Json.JsonSerializerSettings newtonsoftJsonSettings = new Newtonsoft.Json.JsonSerializerSettings();

            Newtonsoft.Json.Converters.IsoDateTimeConverter idtc = new Newtonsoft.Json.Converters.IsoDateTimeConverter();
            idtc.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
            newtonsoftJsonSettings.Converters.Add(idtc);

            newtonsoftJsonSettings.Formatting = Newtonsoft.Json.Formatting.Indented;

            newtonsoftJsonSettings.ContractResolver = new LowCasePropertyNemesContractResolver();

            newtonsoftJsonSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            newtonsoftJsonSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
            newtonsoftJsonSettings.MissingMemberHandling = Newtonsoft.Json.MissingMemberHandling.Ignore;
            newtonsoftJsonSettings.DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.Ignore;

            return newtonsoftJsonSettings;
        }

        public string SerializeObject(object obj)
        {
            try
            {
                return Newtonsoft.Json.JsonConvert.SerializeObject(obj, _newtonsoftJsonSettings);
            }
            catch (System.Exception ex)
            {
                return "";
            }
        }

        public T DeserializeObject<T>(string json)
        {
            try
            {
                using (TextReader sr = new StringReader(json))
                {
                    return (T)_newtonsoftJosnSerializer.Deserialize(sr, typeof(T));
                }
            }
            catch (System.Exception ex)
            {
                return default(T);
            }
        }

        public object GetValue(string json)
        {
            Newtonsoft.Json.Linq.JObject jObject = Newtonsoft.Json.Linq.JObject.Parse(json);
            return GetValue(jObject);
        }

        public T GetValue<T>(string json, params string[] keys)
        {
            if (keys == null || keys.Length == 0)
            {
                return default(T);
            }

            string temp = json;
            foreach(string key in keys)
            {
                temp = GetValue(temp, key);
                if(string.IsNullOrEmpty(temp))
                {
                    return default(T);
                }
            }
            return DeserializeObject<T>(temp);
        }

        private string GetValue(string json, string key)
        {
            Newtonsoft.Json.Linq.JObject jObject = Newtonsoft.Json.Linq.JObject.Parse(json);
            foreach (KeyValuePair<string, Newtonsoft.Json.Linq.JToken> kv in jObject)
            {
                if (kv.Key == key)
                    return kv.Value.ToString();
            }
            return "";
        }

        private object GetValue(Newtonsoft.Json.Linq.JObject jObject)
        {
            Dictionary<string, object> resultMap = new Dictionary<string, object>();
            foreach (KeyValuePair<string, Newtonsoft.Json.Linq.JToken> kv in jObject)
            {
                if (kv.Value is Newtonsoft.Json.Linq.JValue)
                {
                    resultMap[kv.Key] = ((Newtonsoft.Json.Linq.JValue)kv.Value).Value;
                }
                else if (kv.Value is Newtonsoft.Json.Linq.JObject)
                {
                    resultMap[kv.Key] = GetValue((Newtonsoft.Json.Linq.JObject)kv.Value);
                }
                else if (kv.Value is Newtonsoft.Json.Linq.JArray)
                {
                    resultMap[kv.Key] = GetValue((Newtonsoft.Json.Linq.JArray)kv.Value);
                }
                else if (kv.Value is Newtonsoft.Json.Linq.JProperty)
                {
                    resultMap[kv.Key] = null;//GetValue((Newtonsoft.Json.Linq.JProperty)kv.Value);
                }
            }
            return resultMap;
        }

        private object GetValue(Newtonsoft.Json.Linq.JArray jArray)
        {
            List<object> resultList = new List<object>();
            Dictionary<string, object> resultMap = new Dictionary<string, object>();
            foreach (Newtonsoft.Json.Linq.JToken jToken in jArray)
            {
                if (jToken is Newtonsoft.Json.Linq.JValue)
                {
                    resultList.Add(((Newtonsoft.Json.Linq.JValue)jToken).Value);
                }
                else if (jToken is Newtonsoft.Json.Linq.JObject)
                {
                    resultList.Add(GetValue((Newtonsoft.Json.Linq.JObject)jToken));
                }
                else if (jToken is Newtonsoft.Json.Linq.JArray)
                {
                    resultList.Add(GetValue((Newtonsoft.Json.Linq.JArray)jToken));
                }
                else if (jToken is Newtonsoft.Json.Linq.JProperty)
                {
                    resultList.Add(GetValue((Newtonsoft.Json.Linq.JArray)jToken));
                }
            }
            return resultList;
        }

        //public static string dt2json(DataTable dt)
        //{
        //    JsonWriter writer = new IndentedJsonWriter();
        //    JsonObject objJson = new JsonObject();
        //    JsonArray arrs = new JsonArray();
        //    JsonObject arrItem;

        //    for (int i = 0; i < dt.Rows.Count; i++)
        //    {
        //        arrItem = new JsonObject();
        //        for (int j = 0; j < dt.Columns.Count; j++)
        //        {
        //            arrItem.Add(dt.Columns[j].ColumnName, dt.Rows[i][j].ToString());
        //        }
        //        arrs.Add(arrItem);
        //    }
        //    objJson.Add(dt.TableName, arrs);
        //    objJson.Write(writer);

        //    return writer.ToString();
        //}




        ///// <summary>
        ///// 添加时间转换器
        ///// </summary>
        ///// <param name="serializer"></param>
        //private static void AddIsoDateTimeConverter(JsonSerializer serializer)
        //{
        //    IsoDateTimeConverter idtc = new IsoDateTimeConverter();
        //    //定义时间转化格式
        //    idtc.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
        //    //idtc.DateTimeFormat = "yyyy-MM-dd";
        //    serializer.Converters.Add(idtc);
        //}

        ///// <summary>
        ///// Json转换配置
        ///// </summary>
        ///// <param name="serializer"></param>
        //private static void SerializerSetting(JsonSerializer serializer)
        //{
        //    serializer.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
        //    //serializer.NullValueHandling = NullValueHandling.Ignore;
        //    //serializer.MissingMemberHandling = MissingMemberHandling.Ignore;
        //    //serializer.DefaultValueHandling = DefaultValueHandling.Ignore;
        //}

        ///// <summary>
        ///// 把对象列表编码为Json数据
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="objList"></param>
        ///// <returns></returns>
        //public static string ListToJson<T>(List<T> objList)
        //{
        //    JsonSerializer serializer = new JsonSerializer();
        //    SerializerSetting(serializer);
        //    AddIsoDateTimeConverter(serializer);
        //    using (TextWriter sw = new StringWriter())
        //    using (JsonWriter writer = new JsonTextWriter(sw))
        //    {
        //        serializer.Serialize(writer, objList);
        //        return sw.ToString();
        //    }
        //}

        ///// <summary>
        /////  把一个对象编码为Json数据
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="obj"></param>
        ///// <returns></returns>
        //public static string ObjectToJson<T>(T obj)
        //{
        //    JsonSerializer serializer = new JsonSerializer();
        //    SerializerSetting(serializer);
        //    AddIsoDateTimeConverter(serializer);
        //    using (TextWriter sw = new StringWriter())
        //    using (JsonWriter writer = new JsonTextWriter(sw))            
        //    {
        //        serializer.Serialize(writer, obj);
        //        return sw.ToString();
        //    }
        //}

        ///// <summary>
        ///// 根据传入的Json数据，解码为对象(一个)
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="data"></param>
        ///// <returns></returns>
        //public static T JsonToObject<T>(string data)
        //{
        //    JsonSerializer serializer = new JsonSerializer();
        //    serializer.MissingMemberHandling = MissingMemberHandling.Ignore;
        //    AddIsoDateTimeConverter(serializer);
        //    StringReader sr = new StringReader(data);
        //    return (T)serializer.Deserialize(sr, typeof(T));
        //}

        ///// <summary>
        ///// 功能同DecodeObject
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="data"></param>
        ///// <returns></returns>
        //public static List<T> JsonToList<T>(string data)
        //{
        //    JsonSerializer serializer = new JsonSerializer();
        //    serializer.MissingMemberHandling = MissingMemberHandling.Ignore;
        //    AddIsoDateTimeConverter(serializer);
        //    StringReader sr = new StringReader(data);
        //    return (List<T>)serializer.Deserialize(sr, typeof(List<T>));
        //}

        ///// <summary        /// 将C#数据实体转化为JSON数据
        ///// </summary>
        ///// <param name="obj">要转化的数据实体</param>
        ///// <returns>JSON格式字符串</returns>
        //public static string SerializeObject<T>(T obj)
        //{
        //    //var objTemp = new { Formatting = "test" };

        //    JavaScriptSerializer js = new JavaScriptSerializer();
        //    string test1 = js.Serialize(obj);

        //    var settings = new JsonSerializerSettings { Formatting = Formatting.Indented };
        //    string indented = JsonConvert.SerializeObject(obj, settings);

        //    string none = JsonConvert.SerializeObject(obj, Formatting.Indented, settings);

        //    string test = Newtonsoft.Json.JsonConvert.SerializeObject(obj);

        //    return Newtonsoft.Json.JsonConvert.SerializeObject(obj, Formatting.Indented, new JsonSerializerSettings
        //    {
        //        //ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        //        ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver()
        //    });
        //}

        ///// <summary>
        ///// 将JSON数据转化为C#数据实体
        ///// </summary>
        ///// <param name="json">符合JSON格式的字符串</param>
        ///// <returns>T类型的对象</returns>
        //public static T DeserializeObject<T>(string json)
        //{
        //    return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json);
        //}

        ///// <summary        /// 将C#数据实体转化为JSON数据
        ///// </summary>
        ///// <param name="obj">要转化的数据实体</param>
        ///// <returns>JSON格式字符串</returns>
        //public static string SerializeObject<T>(T obj)
        //{
        //    JavaScriptSerializer js = new JavaScriptSerializer();
        //    return js.Serialize(obj);
        //}

        ///// <summary>
        ///// 将JSON数据转化为C#数据实体
        ///// </summary>
        ///// <param name="json">符合JSON格式的字符串</param>
        ///// <returns>T类型的对象</returns>
        //public static T DeserializeObject<T>(string json)
        //{
        //    JavaScriptSerializer js = new JavaScriptSerializer();
        //    return js.Deserialize<T>(json);
        //}

        ///// <summary        /// 将C#数据实体转化为JSON数据
        ///// </summary>
        ///// <param name="obj">要转化的数据实体</param>
        ///// <returns>JSON格式字符串</returns>
        //public static string SerializeObject<T>(T obj)
        //{
        //    DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));
        //    MemoryStream stream = new MemoryStream();
        //    serializer.WriteObject(stream, obj);
        //    stream.Position = 0;

        //    StreamReader sr = new StreamReader(stream);
        //    string resultStr = sr.ReadToEnd();
        //    sr.Close();
        //    stream.Close();

        //    return resultStr;
        //}

        ///// <summary>
        ///// 将JSON数据转化为C#数据实体
        ///// </summary>
        ///// <param name="json">符合JSON格式的字符串</param>
        ///// <returns>T类型的对象</returns>
        //public static T DeserializeObject<T>(string json)
        //{
        //    DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));
        //    MemoryStream ms = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(json.ToCharArray()));
        //    T obj = (T)serializer.ReadObject(ms);
        //    ms.Close();

        //    return obj;
        //}
    }

    public class LowCasePropertyNemesContractResolver : Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver
    {
        protected override string ResolvePropertyName(string propertyName)
        {
            string temp = base.ResolvePropertyName(propertyName);
            if (temp.StartsWith("_"))
            {
                temp = temp.Substring(1, temp.Length - 1);
            }
            return temp.Substring(0, 1).ToLower() + temp.Substring(1, temp.Length - 1);
            //return temp.ToLower();
        }
    }
}
