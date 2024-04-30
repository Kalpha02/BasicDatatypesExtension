using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    public class Number
    {
        //private bool isImaginary;
        private BigInteger _Value1 = 0;
        private Number? _Value2;
        private System.Number.MathOperator _Operator = MathOperator.Add;

        private Number() { }

        public Number(BigInteger Value)
        {
            _Value1 = Value;
        }

        public Number(decimal Value)
        {
            /*
            Denominator = 1;
            Numerator = new BigInteger(value);
            value -= Math.Round(value, 0, MidpointRounding.ToZero);
            while (value != 0)
            {
                Numerator *= 10;
                Denominator *= 10;
                value *= 10;
                Numerator += new BigInteger(value);
                value -= Math.Round(value, 0, MidpointRounding.ToZero);
            }
            SimplifyFraction();
            */
        }

        public bool IsIrrationalNumber()
        {
            if (_Value2 is not null)
            {
                if (_Operator == MathOperator.Divide || _Operator == MathOperator.Root || _Operator == MathOperator.Logarithm)
                {
                    return true;
                }
                return _Value2.IsIrrationalNumber();
            }
            return false;
        }

        private System.Number.MathOperator GetHardestOperator()
        {
            if (_Value2 is not null)
            {
                if (_Value2.GetHardestOperator() > this._Operator)
                {
                    return _Value2.GetHardestOperator();
                }
            }
            return _Operator;
        }

        public override string ToString()
        {
            if (_Value2 is null)
            {
                return _Value1.ToString();
            }

            StringBuilder sb = new StringBuilder();

            if (_Value2._Value2 is not null)
            {
                sb.Append(')');
            }
            sb.Append(_Value1);

            switch (_Operator)
            {
                case MathOperator.Add:
                    sb.Append('+');
                    break;
                case MathOperator.Subtract:
                    sb.Append('-');
                    break;
                case MathOperator.Multiply:
                    sb.Append('*');
                    break;
                case MathOperator.Divide:
                    sb.Append('/');
                    break;
                case MathOperator.Power:
                    sb.Append('^');
                    break;
                case MathOperator.Root:
                case MathOperator.Logarithm:
                default:
                    throw new NotImplementedException();
            }
            sb.Append(_Value2);

            if (_Value2._Value2 is not null)
            {
                sb.Append(')');
            }

            return sb.ToString();
        }

        private static Number SimplifyFraction(BigInteger Value1, BigInteger Value2)
        {
            if (Value1 % Value2 == 0)
            {
                return new Number(Value1 / Value2);
            }
            BigInteger gcd = BigInteger.GreatestCommonDivisor(Value1, Value2);
            return new Number() {_Value1 = Value1 / gcd, _Operator = MathOperator.Divide, _Value2 = Value2 / gcd};
        }

        private static Number Add(Number Value1, Number Value2)
        {
            throw new NotImplementedException();
        }

        private static Number Subtract(Number Value1, Number Value2)
        {
            throw new NotImplementedException();
        }

        private static Number Multiply(Number Value1, Number Value2)
        {
            throw new NotImplementedException();
        }

        private static Number Divide(Number Nominator, Number Denominator)
        {
            throw new NotImplementedException();
        }

        private static Number Power(Number Base, Number Power)
        {
            throw new NotImplementedException();
        }

        private static Number Root(Number Value1, Number Value2)
        {
            throw new NotImplementedException();
        }

        private static Number Log(Number Value1, Number Value2)
        {
            throw new NotImplementedException();
        }

        public static implicit operator Number(BigInteger Value)
        {
            return new Number(Value);
        }

        public static Number operator +(Number Value1, Number Value2)
        {
            return Number.Add(Value1, Value2);
        }

        public static Number operator -(Number Value1, Number Value2)
        {
            return Number.Subtract(Value1, Value2);
        }

        public static Number operator *(Number Value1, Number Value2)
        {
            return Number.Multiply(Value1, Value2);
        }

        public static Number operator /(Number Value1, Number Value2)
        {
            return Number.Divide(Value1, Value2);
        }

        public static Number operator ^(Number Value1, Number Value2)
        {
            return Number.Power(Value1, Value2);
        }

        internal enum MathOperator : byte
        {
            Add = 0,
            Subtract = 1,
            Multiply = 2,
            Divide = 4,
            Power = 3,
            Root = 5,
            Logarithm = 6,
        }
    }
}