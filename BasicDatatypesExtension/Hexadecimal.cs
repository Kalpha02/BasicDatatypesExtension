using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    public struct HexadecimalChar
    {
#if NET6_0_OR_GREATER
        private Bit[] _BitCode = new Bit[4] { 0, 0, 0, 0 };

        public HexadecimalChar() { }
#else
        private Bit[] _BitCode;
#endif
        public HexadecimalChar(byte Value) : this(BitList.ToBitList(Value)) { }

        public HexadecimalChar(List<Bit> BitCode)
        {
#if NETFRAMEWORK
            _BitCode = BitCode.ToArray();
#endif
            this.BitCode = BitCode;
        }

        public override string ToString()
        {
            var n = (byte)BitList.ToNumber(this.BitCode);
#if NET6_0_OR_GREATER
            return n switch
            {
                > 10 and <= 15 => ((char)(n + 55)).ToString(),
                _ => n.ToString(),
            };
#else
            return n.ToString();
#endif
        }

#region Properties

        public List<Bit> BitCode
        {
            get
            {
                return _BitCode.ToList();
            }
            set
            {
                while (value.Count > 4)
                {
                    if ((bool)value.First()) throw new ArgumentOutOfRangeException("List of bits is greater than 15.");
                    value.RemoveAt(0);
                }
                while (value.Count < 4)
                {
                    value.Insert(0, false);
                }
            }
        }

        public byte Value
        {
            get
            {
                return (byte)BitList.ToNumber(BitCode);
            }
            set
            {
                try
                {
                    this = new HexadecimalChar(value);
                }
                catch { throw; }
            }
        }

#if NET6_0_OR_GREATER
        public Char AsChar
        {
            get => this.ToString()[0];
            set
            {
                this.BitCode = value switch
                {
                    > '0' and <= '9' => BitList.ToBitList((byte)value),
                    'A' or 'a' => BitList.ToBitList(10),
                    'B' or 'b' => BitList.ToBitList(11),
                    'C' or 'c' => BitList.ToBitList(12),
                    'D' or 'd' => BitList.ToBitList(13),
                    'E' or 'e' => BitList.ToBitList(14),
                    'F' or 'f' => BitList.ToBitList(15),
                    _ => throw new ArgumentException("Value is not a hexadecimal character", nameof(value)),
                };
            }
        }
#endif

#endregion


#region Static functions

        public static List<HexadecimalChar> ToHex(ulong value)
        {
            List<HexadecimalChar> HexValue = new List<HexadecimalChar>();
            if (value == 0)
            {
                HexValue.Add(new HexadecimalChar(0));
                return HexValue;
            }
            while (value > 0)
            {
                throw new NotImplementedException();
            }
            return new List<HexadecimalChar>();
        }

        /// <summary>
        /// Converts a number into hexadecimal number.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="maxDecimals"></param>
        /// <returns></returns>
        public static string ToHex(double value, int maxDecimals = -1)
        {
            string result = string.Empty;
            if (value < 0)
            {
                result += "-";
                value = -value;
            }
            if (value > ulong.MaxValue)
            {
                result += double.PositiveInfinity.ToString();
                return result;
            }
            ulong trunc = (ulong)value;
            result += trunc.ToString("X");
            value -= trunc;
            if (value == 0)
            {
                return result;
            }
            result += ".";
            byte hexdigit;
            while ((value != 0) && (maxDecimals != 0))
            {
                value *= 16;
                hexdigit = (byte)value;
                result += hexdigit.ToString("X");
                value -= hexdigit;
                maxDecimals--;
            }
            return result;
        }

        /// <summary>
        /// Converts a hexadecimal number into a double.
        /// </summary>
        /// <param name="Hex"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static double ToDouble(string Hex)
        {
            double ReturningDouble = 0;
            Hex = Hex.ToUpper();
            string[] Var = Hex.Split('.', ',');
            bool Negativ = false;
            if (Var[0][0] == '-')
            {
                Var[0] = Var[0].Substring(1);
                Negativ = true;
            }
            if (Var.Length > 2)
            {
                throw new ArgumentException("No more than 1 column expected.");
            }
            try
            {
                for (int i = 0; i < Var[0].Length; i++)
                {
                    int n = (byte)System.Uri.FromHex(Var[0][i]);
                    ReturningDouble += Math.Pow(16, Var[0].Length - 1 - i) * n;
                }
                if (Var.Length > 1)
                {
                    for (int i = 0; i < Var[1].Length; i++)
                    {
                        int n = (byte)System.Uri.FromHex(Var[1][i]);
                        ReturningDouble += Math.Pow(16, -i - 1) * n;
                    }
                }
            }
            catch { throw; }
            return Negativ ? -ReturningDouble : ReturningDouble;
        }

#endregion

#region Operators

        public static implicit operator HexadecimalChar(List<Bit> value)
        {
            HexadecimalChar n = new HexadecimalChar(0);
            n.BitCode = value;
            return n;
        }

        public static implicit operator HexadecimalChar(Bit[] value)
        {
            return value.ToArray();
        }

#endregion
    }
    /*
    /// <summary>
    /// Contains two arrays of four bits. It takes a lot of memory but it is exactly.
    /// </summary>
    public struct Hexadecimal_Value
    {
        private bool _Negativ = false;

        public Hexadecimal_Value(decimal Value)
        {
            if (Value < 0)
            {
                Value *= -1;
                _Negativ= true;
            }
        }
    }*/
}