using System;
using System.Collections.Generic;
using System.Text;

namespace SolJSON.Types
{
    public class JsonNull : JsonObject
    {

        public JsonNull()
        {
        }

        public override TYPE Type
        {
            get
            {
                return TYPE.NULL;
            }
        }

        public override string ToString()
        {
            return "null";
        }
    }

}
