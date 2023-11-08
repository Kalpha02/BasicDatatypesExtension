using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    public class BitList : List<Bit>
    {
        #region Constructors

        public BitList() { }

        public BitList(Bit Value) : this(new List<Bit>() { Value }) { }

        public BitList(params Bit[] Value)
        {
            foreach (var Bit in Value)
                this.Add(Bit);
        }

        public BitList(List<Bit> Bits)
        {
            Bits.ForEach(this.Add);
        }
        #endregion

        #region Functions

        public BitList TrimStart(bool value = false) => TrimStart(new Bit(value));
        public BitList TrimStart(Bit value)
        {
            BitList TrimmedList = this;
            for (int i = TrimmedList.Count - 1; i >= 0; i--)
            {
                if (TrimmedList[0] != value)
                {
                    break;
                }
                TrimmedList.RemoveAt(0);
            }
            return TrimmedList;
        }

        public BitList PadLeft(int ItemsTotal)
        {
            BitList PaddedBitlist = this;
            while (ItemsTotal > PaddedBitlist.Count)
            {
                PaddedBitlist.Insert(0, false);
            }
            return PaddedBitlist;
        }

        public BitList PadLeft(int ItemsTotal, Bit Value)
        {
            BitList PaddedBitlist = this;
            while (ItemsTotal > PaddedBitlist.Count)
            {
                PaddedBitlist.Insert(0, Value);
            }
            return PaddedBitlist;
        }

        public BitList PadRight(int ItemsTotal, Bit Value)
        {
            BitList PaddedBitlist = this;
            while (ItemsTotal > PaddedBitlist.Count)
            {
                PaddedBitlist.Add(Value);
            }
            return PaddedBitlist;
        }

        public Color ToColor() => BitList.ToColor(this);

        public byte[] ToByteArray() => BitList.ToByteArray(this);

        public string ToString(int PadLeft)
        {
            return this.ToString().PadLeft(PadLeft, '0');
        }

        public override string ToString()
        {
            StringBuilder retruningValue = new StringBuilder();
            this.ForEach(Bit => retruningValue.Append(Bit));
            if (this.Count == 0)
            {
                retruningValue.Append('0');
            }
            return retruningValue.ToString();
        }

        public override bool Equals(object obj)
        {
            if (obj is List<Bit> | obj is BitList)
            {
                return this.GetHashCode() == new BitList((List<Bit>)obj).GetHashCode();
            }
            if (obj is Bit[] BitArray)
            {
                return this.GetHashCode() == new BitList(BitArray).GetHashCode();
            }
            return false;
        }

        public override int GetHashCode()
        {
            int hash = 2147483647;
            foreach (Bit b in this.TrimStart().ToArray())
            {
                hash = hash * 1754323 + b.GetHashCode();
            }
            return hash;
        }

        #endregion

        #region Static Functions

#if NET5_0_OR_GREATER
        /// <summary>
        /// Converts a Number to a List of Bits
        /// </summary>
        /// <param name="Value"></param>
        /// <param name="PadLeft"></param>
        /// <returns></returns>
        public static BitList ToBitList(BigInteger Value, int PadLeft = 0)
        {
            BitList BTC = new BitList();
            while (Value != 0)
            {
                BTC.Add(Value % 2 != 0);
                Value /= 2;
            }
            while (PadLeft > BTC.Count)
            {
                BTC.Add(false);
            }
            BTC.Reverse();
            return BTC;
        }

        public static BigInteger ToNumber(List<Bit> BitCode, int MaxBits = -1)
        {
            if (MaxBits > 0)
            {
                while (BitCode.Count > MaxBits)
                {
                    if ((bool)BitCode.First()) throw new ArgumentOutOfRangeException(nameof(BitCode), $"List of bits has more bits than the maximum amount of {MaxBits}");
                    BitCode.RemoveAt(0);
                }
            }
            int BitCodeLength = BitCode.Count;
            BigInteger Value = 0;
            for (int i = 0; i < BitCodeLength; i++)
            {
                Value += BitCode[i][BitCodeLength - i - 1];
            }
            return Value;
        }

        public static BigInteger ToNumber(Bit[] BitCode)
        {
            BigInteger Value = 0;
            for (int i = 0; i < BitCode.Length; i++)
            {
                Value += BitCode[i][BitCode.Length - i - 1];
            }
            return Value;
        }
#else
        public static int ToNumber(List<Bit> BitCode, int MaxBits = -1)
        {
            if (MaxBits > 0)
            {
                while (BitCode.Count > MaxBits)
                {
                    if ((bool)BitCode.First()) throw new ArgumentOutOfRangeException(nameof(BitCode), $"List of bits has more bits than the maximum amount of {MaxBits}");
                    BitCode.RemoveAt(0);
                }
            }
            int BitCodeLength = BitCode.Count;
            int Value = 0;
            for (int i = 0; i < BitCodeLength; i++)
            {
                Value += (int)(BitCode[i][BitCodeLength - i - 1]);
            }
            return Value;
        }

        public static int ToNumber(Bit[] BitCode)
        {
            int Value = 0;
            for (int i = 0; i < BitCode.Length; i++)
            {
                Value += (int)(BitCode[i][BitCode.Length - i - 1]);
            }
            return Value;
        }

        /// <summary>
        /// Converts a Number to a List of Bits
        /// </summary>
        /// <param name="Value"></param>
        /// <param name="PadLeft"></param>
        /// <returns></returns>
        public static BitList ToBitList(int Value, int PadLeft = 0)
        {
            BitList BTC = new BitList();
            while (Value != 0)
            {
                BTC.Add(Value % 2 != 0);
                Value /= 2;
            }
            while (PadLeft > BTC.Count)
            {
                BTC.Add(false);
            }
            BTC.Reverse();
            return BTC;
        }
#endif

        /// <summary>
        /// Converts the list of bits into a color. It will use 8 bit segments for each alpha, red, green and blue value. The list of bits will align right.
        /// </summary>
        /// <param name="Bitlist"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static Color ToColor(BitList Bitlist)
        {
            while (Bitlist.Count > 32)
            {
                if ((bool)Bitlist.First()) throw new ArgumentOutOfRangeException(nameof(Bitlist), $"List of bits has more bits than the maximum amount of {32}");
                Bitlist.RemoveAt(0);
            }
            while (Bitlist.Count < 32)
            {
                Bitlist.Insert(0, false);
            }
            Console.WriteLine(Bitlist);
            return Color.FromArgb((int)BitList.ToNumber(Bitlist.GetRange(0, 8)), (int)BitList.ToNumber(Bitlist.GetRange(8, 8)), (int)BitList.ToNumber(Bitlist.GetRange(16, 8)), (int)BitList.ToNumber(Bitlist.GetRange(24, 8)));
        }

        public static byte[] ToByteArray(BitList Value)
        {
            List<byte> Bytes = new List<byte>();
            if (Value.Count % 8 != 0)
            {
                Value = Value.PadLeft(Value.Count + (8 - (Value.Count % 8)), false);
            }
            for (int i = 0; i < Value.Count; i += 8)
            {
                Bytes.Add((byte)BitList.ToNumber(Value.GetRange(i, 8)));
            }
            return Bytes.ToArray();
        }

        /// <summary>
        /// Moves bits to the right.
        /// </summary>
        /// <param name="BitCode">The bit-list that should be moved</param>
        /// <param name="Value">How many times the bit should be moved</param>
        /// <returns></returns>
        public static BitList BitOperator(List<Bit> BitCode, int Value)
        {
            int BitCodeLength = BitCode.Count;
            Value %= BitCodeLength;
            /*if(Value < 0)
            {
                Value = BitCodeLength + Value;
            }
            List<Bit> List = new List<Bit>();
            List.AddRange(BitCode.GetRange(BitCodeLength - Value, Value));
            BitCode.*/
            Bit[] New_BitCode = new Bit[BitCodeLength];
            for (int i = 0; i < BitCodeLength; i++)
            {
                New_BitCode[(BitCodeLength + Value + i) % BitCodeLength] = BitCode[i];
            }
            return new BitList(New_BitCode);
        }

        private static void ToSameLength(ref BitList Value1, ref List<Bit> Value2)
        {
            while (Value1.Count < Value2.Count)
            {
                Value1.Insert(0, false);
            }
            while (Value1.Count > Value2.Count)
            {
                Value2.Insert(0, false);
            }
        }

        private static bool GreaterThan(List<Bit> Value1, List<Bit> Value2)
        {
            Value1 = new BitList(Value1).TrimStart();
            Value2 = new BitList(Value2).TrimStart();
            if (Value1.Count > Value2.Count)
            {
                return true;
            }
            if (Value2.Count > Value1.Count)
            {
                return false;
            }
            for (int i = 0; i < Value2.Count; i++)
            {
                if (Value1[i] > Value2[i])
                {
                    return true;
                }
            }
            return false;
        }

        #endregion

        #region Implicit Operators

        public static implicit operator BitList(Bit[] Value)
        {
            return new BitList(Value);
        }

        public static implicit operator BitList(Bit Value)
        {
            return new BitList(Value);
        }

        public static implicit operator BitList(byte Value)
        {
            return BitList.ToBitList(Value).PadLeft(8, false);
        }

        public static implicit operator BitList(byte[] Value)
        {
            if (Value == null)
            {
                throw new ArgumentNullException(nameof(Value));
            }
            if (Value.Length == 0)
            {
                throw new Exception("Total length need to be bigger than 0");
            }
            BitList val = Value[0];
            for (int i = 1; i < Value.Length; i++)
            {
                val.Add(Value[i]);
            }
            return val;
        }
#if NET5_0_OR_GREATER
        public static implicit operator BitList(BigInteger Value)
        {
            return BitList.ToBitList(Value);
        }
#else
        public static implicit operator BitList(int Value)
        {
            return BitList.ToBitList(Value);
        }
#endif

        #endregion

        #region Explicit Operators

        public static explicit operator Bit[](BitList Value)
        {
            return Value.ToArray();
        }

#if NET5_0_OR_GREATER

        public static explicit operator BigInteger(BitList Value)
        {
            return BitList.ToNumber(Value);
        }

#else

        public static explicit operator int(BitList Value)
        {
            return BitList.ToNumber(Value);
        }
#endif

        #endregion

        #region Logical Operators
        public static BitList operator +(Bit Value1, BitList Value2) => Value2 + Value1;
        public static BitList operator +(BitList Value1, Bit Value2)
        {
            if ((bool)Value2)
            {
                int i = Value1.Count -1;
                while (i >= 0)
                {
                    Value1[i] ^= Value2;
                    if ((bool)Value1[i])
                    {
                        break;
                    }
                    i--;
                }
                if (i < 0)
                {
                    Value1.Insert(0, true);
                }
            }
            return Value1;
        }
        public static BitList operator +(BitList Value1, BitList Value2) => Value1 + (List<Bit>)Value2;
        public static BitList operator +(List<Bit> Value1, BitList Value2) => Value2 + Value1;
        public static BitList operator +(BitList Value1, List<Bit> Value2)
        {
            return BitList.ToNumber(Value1) + BitList.ToNumber(Value2);
        }

        public static BitList operator -(Bit Value1, BitList Value2) => Value2 - Value1;
        public static BitList operator -(BitList Value1, Bit Value2)
        {
            if ((bool)Value2)
            {
                if (!(BitList.ToNumber(Value1) > 0))
                {
                    return Value1; // Wäre kleiner als 0
                }
                int i = Value1.Count - 1;
                while (i >= 0)
                {
                    Value1[i] ^= Value2;
                    if (!Value1[i])
                    {
                        break;
                    }
                    i--;
                }
                if (i < 0)
                {
                    Value1.Insert(0, true);
                }
            }
            return Value1;
        }
        public static BitList operator -(BitList Value1, List<Bit> Value2) => Value1 - new BitList(Value2);
        public static BitList operator -(List<Bit> Value1, BitList Value2) => new BitList(Value1) - Value2;
        public static BitList operator -(BitList Value1, BitList Value2)
        {
            return BitList.ToNumber(Value1) - BitList.ToNumber(Value2);
        }

        public static BitList operator *(BitList Value1, BitList Value2) => Value1 * (List<Bit>)Value2;
        public static BitList operator *(List<Bit> Value1, BitList Value2) => Value2 * Value1;
        public static BitList operator *(BitList Value1, List<Bit> Value2)
        {
            return BitList.ToNumber(Value1) * BitList.ToNumber(Value2);
        }

        public static BitList operator /(BitList Value1, BitList Value2) => Value1 / (List<Bit>)Value2;
        public static BitList operator /(List<Bit> Value1, BitList Value2) => Value2 / Value1;
        public static BitList operator /(BitList Value1, List<Bit> Value2)
        {
            return BitList.ToNumber(Value1) / BitList.ToNumber(Value2);
        }

        public static BitList operator <<(BitList value, int MoveBits)
        {
            return BitList.BitOperator(value, -MoveBits);
        }

        public static BitList operator >>(BitList value, int MoveBits)
        {
            return BitList.BitOperator(value, MoveBits);
        }

        public static BitList operator ^(BitList Value1, BitList Value2) => Value1 ^ (List<Bit>)Value2;
        public static BitList operator ^(List<Bit> Value1, BitList Value2) => Value2 ^ Value1;
        public static BitList operator ^(BitList Value1, List<Bit> Value2)
        {
            BitList.ToSameLength(ref Value1, ref Value2);
            for (int i = 0; i < Value1.Count; i++)
            {
                Value1[i] ^= Value2[i];
            }
            return Value1;
        }

        public static BitList operator &(BitList Value1, BitList Value2) => Value1 & (List<Bit>)Value2;
        public static BitList operator &(List<Bit> Value1, BitList Value2) => Value2 & Value1;
        public static BitList operator &(BitList Value1, List<Bit> Value2)
        {
            ToSameLength(ref Value1, ref Value2);
            for (int i = 0; i < Value1.Count; i++)
            {
                Value1[i] &= Value2[i];
            }
            return Value1;
        }

        public static BitList operator |(BitList Value1, BitList Value2) => Value1 | (List<Bit>)Value2;
        public static BitList operator |(List<Bit> Value1, BitList Value2) => Value2 | Value1;
        public static BitList operator |(BitList Value1, List<Bit> Value2)
        {
            ToSameLength(ref Value1, ref Value2);
            for (int i = 0; i < Value1.Count; i++)
            {
                Value1[i] |= Value2[i];
            }
            return Value1;
        }

        public static BitList operator !(BitList Value)
        {
            for (int i = 0; i < Value.Count; i++)
            {
                Value[i] = !Value[i];
            }
            return Value;
        }

        public static BitList operator ++(BitList Value)
        {
            return Value + true;
        }

        public static BitList operator --(BitList Value)
        {
            return Value - true;
        }

        public static bool operator <(BitList Value1, BitList Value2) => GreaterThan(Value2, Value1);
        public static bool operator <(List<Bit> Value1, BitList Value2) => GreaterThan(Value2, Value1);
        public static bool operator <(BitList Value1, List<Bit> Value2) => GreaterThan(Value2, Value1);
        public static bool operator <(Bit[] Value1, BitList Value2) => GreaterThan(Value2, Value1.ToList());
        public static bool operator <(BitList Value1, Bit[] Value2) => GreaterThan(Value2.ToList(), Value1);

        public static bool operator >(BitList Value1, BitList Value2) => GreaterThan(Value1, Value2);
        public static bool operator >(List<Bit> Value1, BitList Value2) => GreaterThan(Value1, Value2);
        public static bool operator >(BitList Value1, List<Bit> Value2) => GreaterThan(Value1, Value2);
        public static bool operator >(Bit[] Value1, BitList Value2) => GreaterThan(Value1.ToList(), Value2);
        public static bool operator >(BitList Value1, Bit[] Value2) => GreaterThan(Value1, Value2.ToList());

        public static bool operator <=(BitList Value1, BitList Value2) => !GreaterThan(Value1, Value2);
        public static bool operator <=(List<Bit> Value1, BitList Value2) => !GreaterThan(Value1, Value2);
        public static bool operator <=(BitList Value1, List<Bit> Value2) => !GreaterThan(Value1, Value2);
        public static bool operator <=(Bit[] Value1, BitList Value2) => !GreaterThan(Value1.ToList(), Value2);
        public static bool operator <=(BitList Value1, Bit[] Value2) => !GreaterThan(Value1, Value2.ToList());

        public static bool operator >=(BitList Value1, BitList Value2) => !GreaterThan(Value2, Value1);
        public static bool operator >=(List<Bit> Value1, BitList Value2) => !GreaterThan(Value2, Value1);
        public static bool operator >=(BitList Value1, List<Bit> Value2) => !GreaterThan(Value2, Value1);
        public static bool operator >=(Bit[] Value1, BitList Value2) => !GreaterThan(Value1.ToList(), Value2);
        public static bool operator >=(BitList Value1, Bit[] Value2) => !GreaterThan(Value1, Value2.ToList());

        public static bool operator ==(BitList Value1, BitList Value2) => Value1.Equals(Value2);
        public static bool operator ==(List<Bit> Value1, BitList Value2) => Value2.Equals(Value1);
        public static bool operator ==(BitList Value1, List<Bit> Value2) => Value1.Equals(Value2);
        public static bool operator ==(Bit[] Value1, BitList Value2) => Value2.Equals(Value1);
        public static bool operator ==(BitList Value1, Bit[] Value2) => Value1.Equals(Value2);
        
        public static bool operator !=(BitList Value1, BitList Value2) => !Value1.Equals(Value2);
        public static bool operator !=(List<Bit> Value1, BitList Value2) => !Value2.Equals(Value1);
        public static bool operator !=(BitList Value1, List<Bit> Value2) => !Value1.Equals(Value2);
        public static bool operator !=(Bit[] Value1, BitList Value2) => !Value2.Equals(Value1);
        public static bool operator !=(BitList Value1, Bit[] Value2) => !Value1.Equals(Value2);

        #endregion
    }
}