using System;
using System.Collections.Generic;
using System.Text;

namespace SolJSON.Convert.Helper
{
    public class JsonToJsonObject
    {
        protected enum STATE
        {
            JSON_START = 0,
            JSON_END = 1,
            ARRAY_VALUE = 10,
            ARRAY_VALUE_RETURN = 11,
            ARRAY_VALUE_END = 12,

            DICTONARY_KEY = 20,
            DICTONARY_KEY_END = 21,
            DICTONARY_VALUE = 25,
            DICTONARY_VALUE_RETURN = 26,
            DICTONARY_NEXT_ELEMENT = 27,

            STRING = 30,

            BOOl_TRUE = 40,
            BOOL_FALSE = 41,
            NULL = 42,

            NUMBER = 50
        }


        public static Types.JsonObject Convert(string json_string)
        {
            int pos = 0;

            var obj_stack = new Stack<Types.JsonObject>();
            var state_stack = new Stack<STATE>();
            state_stack.Push(STATE.JSON_START);

            // value 파싱을 위한  변수
            int startat = 0;
            bool escape = false;
            StringBuilder sb = new StringBuilder();

            while (true)
            {
                var peek = state_stack.Peek();
                if (peek == STATE.JSON_START)
                {
                    char character = json_string[pos];

                    if (character == ' ' || character == '\t' || character == '\f' || character == '\r' || character == '\n')
                    {
                        pos++;
                        continue;
                    }
                    else if (character == '[')
                    {
                        state_stack.Push(STATE.JSON_END);

                        obj_stack.Push(new Types.JsonArray());
                        state_stack.Push(STATE.ARRAY_VALUE);
                        pos++;
                        continue;
                    }
                    else if (character == '{')
                    {
                        state_stack.Push(STATE.JSON_END);

                        obj_stack.Push(new Types.JsonDictonary());
                        state_stack.Push(STATE.DICTONARY_KEY);
                        pos++;
                        continue;
                    }

                    throw new Exception("unhandled json start character");
                }
                else if (peek == STATE.JSON_END)
                {
                    return obj_stack.Peek();
                }
                else if (peek == STATE.ARRAY_VALUE)
                {
                    char character = json_string[pos];

                    if (character == ' ' || character == '\t' || character == '\f' || character == '\r' || character == '\n')
                    {
                        pos++;
                        continue;
                    }
                    else if (character == '[')
                    {
                        state_stack.Push(STATE.ARRAY_VALUE_RETURN);

                        obj_stack.Push(new Types.JsonArray());
                        state_stack.Push(STATE.ARRAY_VALUE);
                        pos++;
                        continue;
                    }
                    else if (character == '{')
                    {
                        state_stack.Push(STATE.ARRAY_VALUE_RETURN);

                        obj_stack.Push(new Types.JsonDictonary());
                        state_stack.Push(STATE.DICTONARY_KEY);
                        pos++;
                        continue;
                    }
                    else if (character == '"')
                    {
                        state_stack.Push(STATE.ARRAY_VALUE_RETURN);

                        //start string
                        state_stack.Push(STATE.STRING);

                        // 기존에 string 처리 관련 변수 초기화
                        startat = pos + 1;
                        escape = false;
                        if (sb.Length > 0)
                        {
                            sb.Remove(0, sb.Length);
                        }
                        pos++;
                        continue;
                    }
                    else if (character == 't') // true
                    {
                        if (json_string.Substring(pos, 4).Equals("true") == true)
                        {
                            obj_stack.Push(new Types.JsonBool(true));
                            state_stack.Push(STATE.ARRAY_VALUE_RETURN);
                            pos += 4;
                            continue;
                        }
                        else
                        {
                            throw new Exception("unhandled boolean string");
                        }
                    }
                    else if (character == 'f') // false
                    {
                        if (json_string.Substring(pos, 5).Equals("false") == true)
                        {
                            obj_stack.Push(new Types.JsonBool(false));
                            state_stack.Push(STATE.ARRAY_VALUE_RETURN);
                            pos += 5;
                            continue;
                        }
                        else
                        {
                            throw new Exception("unhandled boolean string");
                        }
                    }
                    else if (character == 'n') // null
                    {
                        if (json_string.Substring(pos, 4).Equals("null") == true)
                        {
                            obj_stack.Push(new Types.JsonNull());
                            state_stack.Push(STATE.ARRAY_VALUE_RETURN);
                            pos += 4;
                            continue;
                        }
                        else
                        {
                            throw new Exception("unhandled null string");
                        }
                    }
                    else if ((character >= '0' && character <= '9') || character == '-') // number
                    {
                        state_stack.Push(STATE.ARRAY_VALUE_RETURN);

                        state_stack.Push(STATE.NUMBER);
                        startat = pos;

                        continue;
                    }
                    else if (character == ']')
                    {
                        pos++;
                        state_stack.Pop();
                        continue;
                    }
                }
                else if (peek == STATE.ARRAY_VALUE_RETURN)
                {
                    var obj = obj_stack.Pop();
                    var array = obj_stack.Peek() as Types.JsonArray;

                    array.Add(obj);
                    state_stack.Pop();
                    state_stack.Push(STATE.ARRAY_VALUE_END);
                }
                else if (peek == STATE.ARRAY_VALUE_END)
                {
                    char character = json_string[pos];

                    if (character == ' ' || character == '\t' || character == '\f' || character == '\r' || character == '\n')
                    {
                        pos++;
                        continue;
                    }

                    if (character == ',')
                    {
                        state_stack.Pop();
                        pos++;
                        continue;
                    }

                    if (character == ']')
                    {
                        state_stack.Pop();
                        continue;
                    }
                }
                else if (peek == STATE.DICTONARY_KEY)
                {
                    char character = json_string[pos];

                    if (character == ' ' || character == '\t' || character == '\f' || character == '\r' || character == '\n')
                    {
                        pos++;
                        continue;
                    }
                    else if (character == '"')
                    {
                        state_stack.Push(STATE.DICTONARY_KEY_END);

                        //start string
                        state_stack.Push(STATE.STRING);

                        // 기존에 string 처리 관련 변수 초기화
                        startat = pos + 1;
                        escape = false;
                        if (sb.Length > 0)
                        {
                            sb.Remove(0, sb.Length);
                        }
                        pos++;
                        continue;
                    }
                    else if (character == '}')
                    {
                        pos++;
                        state_stack.Pop();
                        continue;
                    }
                }
                else if (peek == STATE.DICTONARY_KEY_END)
                {
                    char character = json_string[pos];

                    if (character == ' ' || character == '\t' || character == '\f' || character == '\r' || character == '\n')
                    {
                        pos++;
                        continue;
                    }
                    else if (character == ':')
                    {
                        pos++;
                        state_stack.Pop();
                        state_stack.Push(STATE.DICTONARY_VALUE);
                        continue;
                    }

                    throw new Exception("unhandled character in dictonary");
                }
                else if (peek == STATE.DICTONARY_VALUE)
                {
                    char character = json_string[pos];

                    if (character == ' ' || character == '\t' || character == '\f' || character == '\r' || character == '\n')
                    {
                        pos++;
                        continue;
                    }
                    else if (character == '[')
                    {
                        state_stack.Push(STATE.DICTONARY_VALUE_RETURN);

                        obj_stack.Push(new Types.JsonArray());
                        state_stack.Push(STATE.ARRAY_VALUE);
                        pos++;
                        continue;
                    }
                    else if (character == '{')
                    {
                        state_stack.Push(STATE.DICTONARY_VALUE_RETURN);

                        obj_stack.Push(new Types.JsonDictonary());
                        state_stack.Push(STATE.DICTONARY_KEY);
                        pos++;
                        continue;
                    }
                    else if (character == '"')
                    {
                        state_stack.Push(STATE.DICTONARY_VALUE_RETURN);

                        //start string
                        state_stack.Push(STATE.STRING);

                        // 기존에 string 처리 관련 변수 초기화
                        startat = pos + 1;
                        escape = false;
                        if (sb.Length > 0)
                        {
                            sb.Remove(0, sb.Length);
                        }
                        pos++;
                        continue;
                    }
                    else if (character == 't') // true
                    {
                        if (json_string.Substring(pos, 4).Equals("true") == true)
                        {
                            obj_stack.Push(new Types.JsonBool(true));
                            state_stack.Push(STATE.DICTONARY_VALUE_RETURN);
                            pos += 4;
                            continue;
                        }
                        else
                        {
                            throw new Exception("unhandled boolean string");
                        }
                    }
                    else if (character == 'f') // false
                    {
                        if (json_string.Substring(pos, 5).Equals("false") == true)
                        {
                            obj_stack.Push(new Types.JsonBool(false));
                            state_stack.Push(STATE.DICTONARY_VALUE_RETURN);
                            pos += 5;
                            continue;
                        }
                        else
                        {
                            throw new Exception("unhandled boolean string");
                        }
                    }
                    else if (character == 'n') // null
                    {
                        if (json_string.Substring(pos, 4).Equals("null") == true)
                        {
                            obj_stack.Push(new Types.JsonNull());
                            state_stack.Push(STATE.DICTONARY_VALUE_RETURN);
                            pos += 4;
                            continue;
                        }
                        else
                        {
                            throw new Exception("unhandled null string");
                        }
                    }
                    else if ((character >= '0' && character <= '9') || character == '-') // number
                    {
                        state_stack.Push(STATE.DICTONARY_VALUE_RETURN);

                        state_stack.Push(STATE.NUMBER);
                        startat = pos;

                        continue;
                    }
                }
                else if (peek == STATE.DICTONARY_VALUE_RETURN)
                {
                    var value = obj_stack.Pop(); // Value
                    var key = (obj_stack.Pop() as Types.JsonString).Raw; // key

                    var dict = obj_stack.Peek() as Types.JsonDictonary;
                    dict.Add(key,value);

                    state_stack.Pop(); // STATE.DICTONARY_VALUE_RETURN
                    state_stack.Pop(); // STATE.DICTONARY_VALUE
                    state_stack.Push(STATE.DICTONARY_NEXT_ELEMENT);
                }
                else if ( peek == STATE.DICTONARY_NEXT_ELEMENT ) 
                {
                    char character = json_string[pos];

                    if (character == ' ' || character == '\t' || character == '\f' || character == '\r' || character == '\n')
                    {
                        pos++;
                        continue;
                    }

                    if (character == ',')
                    {
                        state_stack.Pop();
                        pos++;
                        continue;
                    }

                    if (character == '}')
                    {
                        state_stack.Pop();
                        continue;
                    }
                }
                else if (peek == STATE.STRING)
                {
                    char character = json_string[pos];

                    if (character == '\\' && escape == false)
                    {
                        escape = true;
                        pos++;
                        continue;
                    }

                    if (escape == false && character == '"') // 문자열 끝
                    {
                        obj_stack.Push(new Types.JsonString(json_string.Substring(startat, pos - startat), true));
                        state_stack.Pop();
                        pos++;
                        continue;
                    }

                    if (escape == true)
                    {
                        escape = false;
                    }
                    pos++;
                    continue;
                }
                else if (peek == STATE.NUMBER)
                {
                    char character = json_string[pos];

                    if ((character >= '0' && character <= '9') 
                        || character == '-' 
                        || character == '.' 
                        || character == 'E'
                        || character == 'e') // number
                    {
                        pos++;
                        continue;
                    }
                    else
                    {
                        obj_stack.Push(new Types.JsonNumber(json_string.Substring(startat, pos - startat)));
                        state_stack.Pop();
                        continue;
                    }
                }
            }
        }
    }
}
