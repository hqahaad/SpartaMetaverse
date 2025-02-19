using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

namespace Dev.SaveLoad
{
    public class JsonSerializer : ISerializer
    {
        public string Serialize<T>(T t)
        {
            return JsonConvert.SerializeObject(t);
        }

        public T Deserialize<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}