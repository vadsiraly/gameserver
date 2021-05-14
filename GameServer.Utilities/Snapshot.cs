using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Utilities
{
    public static class Snapshot
    {
        public static T Take<T>(T obj)
        {
            var settings = new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore, TypeNameHandling = TypeNameHandling.All };
            string jsonData = JsonConvert.SerializeObject(obj, Formatting.None, settings);
            return JsonConvert.DeserializeObject<T>(jsonData, settings);
        }
    }
}
