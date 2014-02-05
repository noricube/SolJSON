using System;
using System.Collections.Generic;
using System.Text;

namespace SolJSON.Types
{
    public class JsonString : JsonObject
    {
        protected string _value;

        public JsonString(string value, bool escape_string = false)
        {
            if (escape_string == true)
            {
                _value = UnEscape(value);
            }
            else
            {
                _value = value;
            }
        }

        public override TYPE Type
        {
            get
            {
                return TYPE.STRING;
            }
        }

        public string Value
        {
            get
            {
                return _value;
            }
        }

        /*
         * Escape / UnEscape는 RFC4627 #2.5를 참고하였다.
         * \uXXXX는 구현하지 않았음
         */
        public static string Escape(string plain_text)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char character in plain_text)
            {
                if (character == '"' || character == '\\' || character == '/')
                {
                    sb.Append('\\');
                }
                else if (character == '\x08') // backspace 
                {
                    sb.Append('\\');
                    sb.Append('b');
                    continue;
                }
                else if (character == '\x0C') // form feed
                {
                    sb.Append('\\');
                    sb.Append('f');
                    continue;
                }
                else if (character == '\x0A') // line feed
                {
                    sb.Append('\\');
                    sb.Append('n');
                    continue;
                }
                else if (character == '\x0D') // carriage return
                {
                    sb.Append('\\');
                    sb.Append('r');
                    continue;
                }
                else if (character == '\x09') //tab
                {
                    sb.Append('\\');
                    sb.Append('t');
                    continue;
                }
                sb.Append(character);
            }

            return sb.ToString();
        }

        public static string UnEscape(string escape_string)
        {
            StringBuilder sb = new StringBuilder();

            bool escaped = false;
            foreach (char character in escape_string)
            {
                if (escaped == true)
                {
                    if (character == '"' || character == '\\' || character == '/')
                    {
                        sb.Append(character);
                    }
                    else if (character == 'b')
                    {
                        sb.Append('\x08');
                    }
                    else if (character == 'f')
                    {
                        sb.Append('\x0C');
                    }
                    else if (character == 'n')
                    {
                        sb.Append('\x0A');
                    }
                    else if (character == 'r')
                    {
                        sb.Append('\x0D');
                    }
                    else if (character == 't')
                    {
                        sb.Append('\x09');
                    }
                    escaped = false;
                    continue;
                }

                if (escaped == false && character == '\\')
                {
                    escaped = true;
                    continue;
                }

                sb.Append(character);
            }

            return sb.ToString();
        }

        public string Raw
        {
            get
            {
                return Value;
            }
        }
        public override string ToString()
        {
            return '"' + Escape(Value) + '"';
        }
    }
}
