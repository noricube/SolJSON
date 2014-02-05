using System;
using System.Collections.Generic;
using System.Text;

namespace SolJSON.Convert
{
    public class JsonConverter
    {
        public static Types.JsonObject ToJsonObject(string json)
        {
            return Helper.JsonToJsonObject.Convert(json);
        }

        public static Types.JsonObject ToJsonObject(object obj)
        {
            return Helper.ObjectToJsonObject.Convert(obj);
        }

        public static T ToObject<T>(string json)
        {
            return Helper.JsonObjectToObject.Convert<T>(Helper.JsonToJsonObject.Convert(json));
        }

		public static object ToObject(Type type, string json)
        {
            return Helper.JsonObjectToObject.Convert(type, Helper.JsonToJsonObject.Convert(json));
        }

		
        public static T ToObject<T>(Types.JsonObject json_object)
        {
            return Helper.JsonObjectToObject.Convert<T>(json_object);
        }

        public static string ToJSON(object obj)
        {
            return ToJsonObject(obj).ToString();
        }
    }
}
