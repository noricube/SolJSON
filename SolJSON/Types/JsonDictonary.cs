using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace SolJSON.Types
{
    public class JsonDictonary : JsonObject, IEnumerable<KeyValuePair<string, JsonObject>>
    {
        protected Dictionary<string, JsonObject> _dict;

        public JsonDictonary()
        {
            _dict = new Dictionary<string, JsonObject>();
        }

        public void Add(string key, JsonObject value)
        {
            _dict.Add(key, value);
        }

        public int Count
        {
            get
            {
                return _dict.Count;
            }
        }

        public bool Contains(string key)
        {
            return _dict.ContainsKey(key);
        }

        public JsonObject this[string key]
        {
            get
            {
                return _dict[key];
            }
        }


        public override TYPE Type
        {
            get
            {
                return TYPE.DICTONARY;
            }
        }


        IEnumerator IEnumerable.GetEnumerator()
        {
            return _dict.GetEnumerator();
        }

        public IEnumerator<KeyValuePair<string, JsonObject>> GetEnumerator()
        {
            return _dict.GetEnumerator();
        }


        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");

            bool first = true;

            foreach (var value in _dict)
            {
                if (first == true)
                {
                    first = false;
                }
                else
                {
                    sb.Append(",");
                }

                sb.Append('"');
                sb.Append(JsonString.Escape(value.Key));
                sb.Append('"');
                sb.Append(':');
                sb.Append(value.Value.ToString());
            }
            sb.Append("}");

            return sb.ToString();
        }
    }
}
