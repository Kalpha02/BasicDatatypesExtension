using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    /// <summary>
    /// Datatype only contains a boolean
    /// </summary>
    public readonly struct Bit
    {
        private readonly bool _Value;

        #region Constructor

        public Bit(bool value)
        {
            this._Value = value;
        }

#endregion

#region Functions

        /// <summary>
        /// Returns the value of the bit
        /// </summary>
        /// <returns>0 or 1</returns>
        public override string ToString()
        {
            return _Value ? "1" : "0";
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        #endregion


#region Properties

        public bool ToBool()
        {
            return this._Value;
        }

        /// <summary>
        /// Multiplies the bit by the power of the param
        /// </summary>
        /// <param name="Power"></param>
        /// <returns></returns>
        public ulong this[int Power]
        {
            get
            {
                return _Value ? (ulong)Math.Pow(2, Power) : 0;
            }
        }

#endregion


#region Operators

        public static implicit operator Bit(bool Value)
        {
            return new Bit(Value);
        }

        public static implicit operator Bit(ulong Value)
        {
            return Value > 0;
        }

        public static implicit operator Bit(double Value)
        {
            return Value > 0;
        }

        /*public static implicit operator Bit(decimal Value)
        {
            return Value > 0;
        }*/

        public static explicit operator bool(Bit Value)
        {
            return Value.ToBool();
        }

        public static explicit operator byte(Bit Value)
        {
            return (byte)(Value._Value ? 1 : 0);
        }

        public static explicit operator sbyte(Bit Value)
        {
            return (sbyte)(Value._Value ? 1 : 0);
        }

        /// <summary>
        /// OR
        /// </summary>
        /// <param name="Left"></param>
        /// <param name="Right"></param>
        /// <returns></returns>
        public static bool operator |(Bit Left, Bit Right)
        {
            return Left._Value | Right._Value;
        }

        /// <summary>
        /// AND
        /// </summary>
        /// <param name="Left"></param>
        /// <param name="Right"></param>
        /// <returns></returns>
        public static bool operator &(Bit Left, Bit Right)
        {
            return Left._Value & Right._Value;
        }

        /// <summary>
        /// Exclusive OR
        /// </summary>
        /// <param name="Left"></param>
        /// <param name="Right"></param>
        /// <returns></returns>
        public static bool operator ^(Bit Left, Bit Right)
        {
            return Left._Value ^ Right._Value;
        }

        public static Bit operator *(Bit Left, Bit Right)
        {
            return Left & Right;
        }

        /// <summary>
        /// Multiply with an integer. If the bit is 1 the value itself will be returned.
        /// </summary>
        /// <param name="Left"></param>
        /// <param name="Right"></param>
        /// <returns></returns>
        public static int operator *(Bit Left, int Right)
        {
            return Right * (Left._Value ? 1 : 0);
        }

        /// <summary>
        /// Multiply with an integer. If the bit is 1 the value itself will be returned.
        /// </summary>
        /// <param name="Left"></param>
        /// <param name="Right"></param>
        /// <returns></returns>
        public static int operator *(int Left, Bit Right)
        {
            return Left * (Right._Value ? 1 : 0);
        }

        public static bool operator !(Bit value)
        {
            return !(value._Value);
        }

        public static bool operator <(Bit Left, Bit Right) => Right > Left;
        public static bool operator >(Bit Left, Bit Right) => !Right && (bool)Left;

        public static bool operator <=(Bit Left, Bit Right) => Right >= Left;
        public static bool operator >=(Bit Left, Bit Right) => Left | (!Right);

        public static bool operator ==(Bit Left, Bit Right)
        {
            return Left._Value == Right._Value;
        }

        public static bool operator !=(Bit Left, Bit Right)
        {
            return Left._Value != Right._Value;
        }

#endregion
    }
}