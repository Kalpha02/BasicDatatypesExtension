using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

#if NET6_0_OR_GREATER
namespace System
{
    public struct Fraction
    {
        public BigInteger Numerator;
        public BigInteger Denominator;

        public Fraction(decimal value)
        {
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
        }

        public Fraction(BigInteger value)
        {
            Denominator = 1;
            Numerator = value;
        }

        public Fraction(BigInteger Numerator, BigInteger Denominator)
        {
            this.Numerator = Numerator;
            this.Denominator = Denominator;
        }

        private void SimplifyFraction()
        {
            this = Fraction.SimplifyFraction(this);
        }

        private static Fraction SimplifyFraction(Fraction fraction)
        {
            BigInteger gcd = BigInteger.GreatestCommonDivisor(fraction.Numerator, fraction.Denominator);
            return new Fraction(fraction.Numerator / gcd, fraction.Denominator / gcd);
        }

        public static Fraction Add(Fraction fraction1, Fraction fraction2)
        {
            BigInteger numerator = fraction1.Numerator * fraction2.Denominator + fraction2.Numerator * fraction1.Denominator;
            BigInteger denominator = fraction1.Denominator * fraction2.Denominator;
            return Fraction.SimplifyFraction(new Fraction(numerator, denominator));
        }

        public static Fraction Subtract(Fraction fraction1, Fraction fraction2)
        {
            BigInteger numerator = fraction1.Numerator * fraction2.Denominator - fraction2.Numerator * fraction1.Denominator;
            BigInteger denominator = fraction1.Denominator * fraction2.Denominator;
            return Fraction.SimplifyFraction(new Fraction(numerator, denominator));
        }

        public static Fraction Multiply(Fraction fraction1, Fraction fraction2)
        {
            BigInteger numerator = fraction1.Numerator * fraction2.Numerator;
            BigInteger denominator = fraction1.Denominator * fraction2.Denominator;
            return Fraction.SimplifyFraction(new Fraction(numerator, denominator));
        }

        public static Fraction Divide(Fraction fraction1, Fraction fraction2)
        {
            BigInteger numerator = fraction1.Numerator * fraction2.Denominator;
            BigInteger denominator = fraction1.Denominator * fraction2.Numerator;
            return Fraction.SimplifyFraction(new Fraction(numerator, denominator));
        }

        public static Fraction Modulo(Fraction fraction1, Fraction fraction2)
        {
            BigInteger numerator = fraction1.Numerator * fraction2.Denominator % (fraction2.Numerator * fraction1.Denominator);
            BigInteger denominator = fraction1.Denominator * fraction2.Denominator;
            return Fraction.SimplifyFraction(new Fraction(numerator, denominator));
        }

        public static Fraction Power(Fraction fraction, BigInteger exponent)
        {
            Fraction result = 1;
            if (exponent == 0)
            {
                return result;
            }
            if (exponent < 0)
            {
                for (Fraction i = -1; i >= exponent; i--)
                {
                    result /= fraction;
                }
                return result;
            }
            for (Fraction i = 1; i <= exponent; i++)
            {
                result *= fraction;
            }
            return result;
        }

        public static BigInteger Round(Fraction Value)
        {
            return (Value.Numerator - (Value.Numerator % Value.Denominator)) / Value.Denominator;
        }

        public decimal ToDecimal()
        {
            return (decimal)Numerator / (decimal)Denominator;
        }

        /// <summary>
        /// rounds the number to an integer. Returns true if the value was rounded
        /// </summary>
        public bool Round()
        {
            BigInteger RoundedValue = Fraction.Round(this);
            if (RoundedValue != this)
            {
                this = RoundedValue;
                return true;
            }
            return false;
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return Numerator.ToString() + "/" + Denominator.ToString();
        }

        public static implicit operator Fraction(decimal value)
        {
            return new Fraction(value);
        }

        public static implicit operator Fraction(BigInteger value)
        {
            return new Fraction(value);
        }

        public static explicit operator decimal(Fraction value)
        {
            return value.ToDecimal();
        }

        public static Fraction operator +(Fraction value1, Fraction value2)
        {
            return Add(value1, value2);
        }

        public static Fraction operator ++(Fraction value)
        {
            return Add(value, 1);
        }

        public static Fraction operator -(Fraction value1, Fraction value2)
        {
            return Subtract(value1, value2);
        }

        public static Fraction operator --(Fraction value)
        {
            return Subtract(value, 1);
        }

        public static Fraction operator *(Fraction value1, Fraction value2)
        {
            return Multiply(value1, value2);
        }

        public static Fraction operator /(Fraction value1, Fraction value2)
        {
            return Divide(value1, value2);
        }

        public static Fraction operator %(Fraction value1, Fraction value2)
        {
            return Modulo(value1, value2);
        }

        /// <summary>
        /// Calculates value1 to the power of value2.
        /// </summary>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        /// <returns></returns>
        public static Fraction operator ^(Fraction value1, BigInteger value2)
        {
            return Power(value1, value2);
        }

        /// <summary>
        /// Equal operator
        /// </summary>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        /// <returns></returns>
        public static bool operator ==(Fraction value1, Fraction value2)
        {
            return value1.Numerator == value2.Numerator && value1.Denominator == value2.Denominator;
        }

        public static bool operator !=(Fraction value1, Fraction value2)
        {
            return value1.Numerator != value2.Numerator || value1.Denominator != value2.Denominator;
        }

        public static bool operator <(Fraction value1, Fraction value2)
        {
            return (value1 - value2).Numerator < 0;
        }

        public static bool operator >(Fraction value1, Fraction value2)
        {
            return (value1 - value2).Numerator > 0;
        }

        public static bool operator <=(Fraction value1, Fraction value2)
        {
            return value1 < value2 || value1 == value2;
        }

        public static bool operator >=(Fraction value1, Fraction value2)
        {
            return value1 > value2 || value1 == value2;
        }
    }
}
#endif