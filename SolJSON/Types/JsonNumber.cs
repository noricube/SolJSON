using System;
using System.Collections.Generic;
using System.Text;

namespace SolJSON.Types
{
    // RFC4627과는 다르게 Exponent부분을 구현하지 않았다. 사용시에 주의가 필요함
    public class JsonNumber : JsonObject
    {
        protected string _value;
        public JsonNumber(int value)
        {
            _value = value.ToString();
        }

        public JsonNumber(long value)
        {
            _value = value.ToString();
        }

        public JsonNumber(float value)
        {
            _value = value.ToString();
        }

        public JsonNumber(double value)
        {
            _value = value.ToString();
        }

        public JsonNumber(string value)
        {
            //TODO: 유효성 검증 필요
            _value = value;
        }

        public override TYPE Type
        {
            get
            {
                return TYPE.NUMBER;
            }
        }

        public int IntValue
        {
            get
            {
                return int.Parse(_value);
            }
        }

        public long LongValue
        {
            get
            {
                return long.Parse(_value);
            }
        }

        public ulong ULongValue
        {
            get
            {
                return ulong.Parse(_value);
            }
        }

        public float FloatValue
        {
            get
            {
                return float.Parse(_value);
            }
        }

        public double DoubleValue
        {
            get
            {
                return double.Parse(_value);
            }
        }

        public override string ToString()
        {
            return _value;
        }
    }
}
