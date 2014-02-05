using System;
using System.Collections.Generic;
using System.Text;

/* 
 * JSON 관련 타입 구현은 RFC4627 ( http://www.ietf.org/rfc/rfc4627.txt ) 를 참조하였다.
 * 다른 타입은 다 같고 Object의 경우 C# Object와의 혼동을 피하기 위해 Dictonary로 구현하였다.
 */
namespace SolJSON.Types
{
    public abstract class JsonObject
    {
        public enum TYPE
        {
            ARRAY = 0 ,
            DICTONARY = 1,
            NUMBER = 2,
            BOOL = 3,
            STRING = 4,
            NULL = 5,
        };

        public abstract TYPE Type
        {
            get;
        }

        public JsonArray AsArray
        {
            get
            {
                return this as JsonArray;
            }
        }

        public JsonBool AsBool
        {
            get
            {
                return this as JsonBool;
            }
        }

        public JsonDictonary AsDictonary
        {
            get
            {
                return this as JsonDictonary;
            }
        }

        public JsonNumber AsNumber
        {
            get
            {
                return this as JsonNumber;
            }
        }

        public JsonString AsString
        {
            get
            {
                return this as JsonString;
            }
        }

        public JsonNull AsNull
        {
            get
            {
                return this as JsonNull;
            }
        }


        public abstract override string ToString();

    }
}
