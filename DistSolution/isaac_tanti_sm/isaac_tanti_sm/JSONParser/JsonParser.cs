using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web;

namespace isaac_tanti_sm.JSONParser
{
    public enum Version
    {
        NETCore2,
        NETCore3
    }
    class JsonParser<T>
    {
        public T parseJSON(string json, Version version)
        {
            var deserializedModel = Activator.CreateInstance(typeof(T));

            switch (version)
            {

                // .NET Core 3.0+ - no need for T model parameter
                case Version.NETCore3:
                    // deserializedModel= JsonSerializer.Deserialize<T>(response);
                    break;
                // .NET Core 2.0
                case Version.NETCore2:
                    var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(json));
                    var serializer = new DataContractJsonSerializer(typeof(T));
                    deserializedModel = serializer.ReadObject(memoryStream);
                    break;
            }

            return (T)deserializedModel;
        }
    }
}