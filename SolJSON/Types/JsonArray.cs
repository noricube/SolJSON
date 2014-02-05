using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;


namespace SolJSON.Types
{
    public class JsonArray : JsonObject, IEnumerable<JsonObject>
    {
        protected List<JsonObject> _values;

        public JsonArray()
        {
            _values = new List<JsonObject>();
        }

        public void Add(JsonObject obj)
        {
            _values.Add(obj);
        }

        public int Count
        {
            get
            {
                return _values.Count;
            }
        }

        public JsonObject this[int index]
        { 
            get
            {
                return _values[index];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _values.GetEnumerator();
        }

        public IEnumerator<JsonObject> GetEnumerator()
        {
            return _values.GetEnumerator();
        }


        public override TYPE Type
        {
            get
            {
                return TYPE.ARRAY;
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("[");

            List<string> convert_result = new List<string>();
            foreach (var value in _values)
            {
                convert_result.Add(value.ToString());
            }

            sb.Append(String.Join(",", convert_result.ToArray()));
            sb.Append("]");

            return sb.ToString();
        }
    }

}
