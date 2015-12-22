using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace COM.Utility
{
    public interface IJsonSerializer
    {
        string SerializeObject(object obj);
        T DeserializeObject<T>(string json);
        object GetValue(string json);
        T GetValue<T>(string json, params string[] keys);
    }
}
