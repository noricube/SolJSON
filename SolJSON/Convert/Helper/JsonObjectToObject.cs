using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace SolJSON.Convert.Helper
{
    public class JsonObjectToObject
    {
        public static T Convert<T>(Types.JsonObject obj)
        {
            var result = Convert(typeof(T), obj);

            if (result == null)
            {
                return default(T);
            }
            return (T)result;
        }

        public static object Convert(Type type, Types.JsonObject obj)
        {
            /* 
             * List인지 Dictonary인지 판단하여 따로 컨버팅 하게하고
             * 그 외에는 변수 1:1 매칭으로 컨버팅을 수행함
             */

            if (obj.Type == Types.JsonObject.TYPE.NULL)
            {
                return null;
            }

            bool isStruct = type.IsValueType && !type.IsEnum;

            if (type == typeof(int))
            {
                return (object)(obj.AsNumber.IntValue);
            }
            else if (type == typeof(long))
            {
                return (object)(obj.AsNumber.LongValue);
            }
            else if (type == typeof(float))
            {
                return (object)(obj.AsNumber.FloatValue);
            }
            else if (type == typeof(double))
            {
                return (object)(obj.AsNumber.DoubleValue);
            }
            else if (type == typeof(bool))
            {
                return (object)(obj.AsBool.Value);
            }
            else if (type == typeof(string))
            {
                return (object)(obj.AsString.Raw);
            }
            else if (typeof(SolJSON.Types.JsonObject) == type)
            {
                return (object)(obj);
            }
            else if (typeof(object) == type) // target이 object라면 모두 변환가능..
            {
                if (obj.Type == Types.JsonObject.TYPE.NUMBER)
                {
                    return (object)(obj.AsNumber.IntValue);
                }
                else if (obj.Type == Types.JsonObject.TYPE.STRING)
                {
                    return (object)(obj.AsString.Raw);
                }
                else if (obj.Type == Types.JsonObject.TYPE.BOOL)
                {
                    return (object)(obj.AsBool.Value);
                }
                else
                {
                    throw new Exception("unmathced object type");
                }
            }
            else if (typeof(IList).IsAssignableFrom(type) == true)
            {
                var sub_types = type.GetGenericArguments();
                //sub_types[0]

                var from_array = obj as Types.JsonArray;
                IList to_array = (IList)Activator.CreateInstance(type);

								if (from_array == null)
								{
	                foreach (var value in from_array)
	                {
	                    to_array.Add(Convert(sub_types[0], value));
	                }
								}
                return to_array;
            }
            else if (typeof(IDictionary).IsAssignableFrom(type) == true)
            {
                var sub_types = type.GetGenericArguments();
                //sub_types[0]

                var from_dict = obj as Types.JsonDictonary;
                IDictionary to_dict = (IDictionary)Activator.CreateInstance(type);

                foreach (var value in from_dict)
                {
                    object key = value.Key;
                    if (sub_types[0].Equals(typeof(int)))
                    {
                        key = System.Convert.ToInt32(key);
                    }

                    to_dict.Add(key, Convert(sub_types[1], value.Value));
                }
                return to_dict;
            }
            else if (type.IsClass == true || isStruct)
            {
                var from_dict = obj as Types.JsonDictonary;
                object target_obj = Activator.CreateInstance(type);

                if (from_dict == null)
                {
                    throw new Exception("unmatched type " + type.ToString() + " " + obj.Type.ToString());
                }

                foreach (var element in from_dict)
                {
                    var property = type.GetProperty(element.Key);
                    if (property == null)
                    {
                        // TODO: 값이 없는것을 경고하자
                        continue;
                    }
                    property.SetValue(target_obj, Convert(property.PropertyType, element.Value), null);
                }

                var method_info = type.GetMethod("OnDeserialized");
                if (method_info != null)
                {
                    method_info.Invoke(target_obj, null);
                }
                return target_obj;
            }
            else if (type.IsEnum == true)
            {
                return Enum.ToObject(type, obj.AsNumber.IntValue);
            }

            throw new Exception("unhandle object type " + type.Name);
        }
    }
}
