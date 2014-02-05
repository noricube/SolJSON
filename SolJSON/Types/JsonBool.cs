using System;
using System.Collections.Generic;
using System.Text;

namespace SolJSON.Types
{
    public class JsonBool : JsonObject
    {
        protected bool _value;
 
        public JsonBool(bool value)
        {
            _value = value;
        }

        public override TYPE Type
        {
            get
            {
                return TYPE.BOOL;
            }
        }

        public bool Value
        {
            get
            {
                return _value;
            }
        }

        public override string ToString()
        {
            if (_value == true)
            {
                return "true";
            }
            else
            {
                return "false";
            }
        }
    }
}
