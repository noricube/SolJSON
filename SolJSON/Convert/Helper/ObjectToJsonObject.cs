using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace SolJSON.Convert.Helper
{
    public class ObjectToJsonObject
    {
        public static Types.JsonObject Convert(object obj)
        {
            /* 
             * List인지 Dictonary인지 판단하여 따로 컨버팅 하게하고
             * 그 외에는 변수 1:1 매칭으로 컨버팅을 수행함
             */

            if (obj == null)
            {
                return new Types.JsonNull();
            }

            Type type = obj.GetType();
            bool isStruct = type.IsValueType && !type.IsEnum;

            if (type == typeof(int))
            {
                return new Types.JsonNumber((int)obj);
            }
            else if (type == typeof(long))
            {
                return new Types.JsonNumber((long)obj);
            }
            else if (type == typeof(float))
            {
                return new Types.JsonNumber((float)obj);
            }
            else if (type == typeof(double))
            {
                return new Types.JsonNumber((double)obj);
            }
            else if (type == typeof(bool))
            {
                return new Types.JsonBool((bool)obj);
            }
            else if (type == typeof(string))
            {
                return new Types.JsonString((string)obj);
            }
            else if (typeof(Types.JsonObject).IsAssignableFrom(type) == true)
            {
                return (Types.JsonObject)obj;
            }
            else if (typeof(IList).IsAssignableFrom(type) == true)
            {
                var from_array = obj as IList;
                var to_array = new Types.JsonArray();

                foreach (var value in from_array)
                {
                    to_array.Add(Convert(value));
                }
                return to_array;
            }
            else if (typeof(IDictionary).IsAssignableFrom(type) == true)
            {
                var from_dict = obj as IDictionary;
                var to_dict = new Types.JsonDictonary();
                var args = type.GetGenericArguments();

                foreach (DictionaryEntry entry in from_dict)
                {
                    var key = entry.Key.ToString();
                    var value = Convert(entry.Value);

                    to_dict.Add(key, value);
                }


                return to_dict;
            }
            else if (type.IsClass == true || isStruct)
            {
                var from_dict = obj;
                var target_dict = new Types.JsonDictonary();

                foreach (var property in type.GetProperties())
                {
                    target_dict.Add(property.Name, (Convert(property.GetGetMethod().FastInvoke(from_dict))));
                }
                return target_dict;
            }
            else if (type.IsEnum == true)
            {
                return new Types.JsonNumber((int)obj);
            }

            throw new Exception("unhandle object type " + type.Name);
        }
    }
}
