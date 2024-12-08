using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;

namespace System
{
    public class BitList : List<Bit>
#if NET7_0_OR_GREATER
        , IAdditionOperators<BitList, BitList, BigInteger>
        , ISubtractionOperators<BitList, BitList, BigInteger>
        , IMultiplyOperators<BitList, BitList, BigInteger>
        , IDivisionOperators<BitList, BitList, BigInteger>
        , IParsable<BitList>
#endif
    {

        #region Constructors

        public BitList(params Bit[] Value)
        {
            foreach (var Bit in Value)
                this.Add(Bit);
        }

        public BitList(IEnumerable<Bit> Bits)
        {
            foreach (Bit bit in Bits)
            {
                this.Add(bit);
            }
        }
        #endregion

        #region Functions

        /// <summary>
        /// Returns the <see cref="BitList"/> with all leading <see cref="Bit"/>s removed which are equal to the value of <paramref name="value"/>.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public BitList TrimStart(bool value = false) => TrimStart(new Bit(value));
        /// <summary>
        /// Returns the <see cref="BitList"/> with all leading <see cref="Bit"/>s removed which are equal to the value of <paramref name="value"/>.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public BitList TrimStart(Bit value)
        {
            if (this.Count == 0)
            {
                return new BitList((List<Bit>)this.MemberwiseClone());
            }
            if (!this.Any(B => B != value))
            {
                return new BitList((List<Bit>)this.MemberwiseClone());
            }
            BitList Val = new BitList();
            Val.AddRange(this.GetRange((this.IndexOf(!value))));
            return Val;
        }

        /// <summary>
        /// Returns a <see cref="BitList"/> that right-aligns the <see cref="Bit"/>s in this instance by padding them with zero-values on the left,
        /// for the specified total length of <paramref name="ItemsTotal"/>.
        /// </summary>
        /// <param name="ItemsTotal">The minimum size of the <see cref="BitList"/> which will be returned.</param>
        /// <returns></returns>
        public BitList PadLeft(int ItemsTotal) => this.PadLeft(ItemsTotal, false);
        /// <summary>
        /// Returns a <see cref="BitList"/> that right-aligns the <see cref="Bit"/>s in this instance by padding them with the value of <paramref name="PaddingChar"/>
        /// on the left, for the specified total length of <paramref name="ItemsTotal"/>.
        /// </summary>
        /// <param name="ItemsTotal">The minimum size of the <see cref="BitList"/> which will be returned.</param>
        /// <param name="PaddingChar">The value that will be used to fill the <see cref="BitList"/> if necessary.</param>
        /// <returns></returns>
        public BitList PadLeft(int ItemsTotal, Bit PaddingChar)
        {
            BitList PaddedBitlist = this;
            while (ItemsTotal > PaddedBitlist.Count)
            {
                PaddedBitlist.Insert(0, PaddingChar);
            }
            return PaddedBitlist;
        }

        /// <summary>
        /// Returns a <see cref="BitList"/> that left-aligns the <see cref="Bit"/>s in this instance by padding them with the value of <paramref name="PaddingChar"/>
        /// on the right, for the specified total length of <paramref name="ItemsTotal"/>.
        /// </summary>
        /// <param name="ItemsTotal">The minimum size of the <see cref="BitList"/> which will be returned.</param>
        /// <param name="PaddingChar">The value that will be used to fill the <see cref="BitList"/> if necessary.</param>
        /// <returns></returns>
        public BitList PadRight(int ItemsTotal, Bit Value)
        {
            BitList PaddedBitlist = this;
            while (ItemsTotal > PaddedBitlist.Count)
            {
                PaddedBitlist.Add(Value);
            }
            return PaddedBitlist;
        }

        /// <summary>
        /// Returns a new <see cref="BitList"/> with a total length which is dividable by 8.
        /// The copy of the instance will be filled with leading zeros if necessary.
        /// </summary>
        /// <returns></returns>
        public BitList ToByteSize() => BitList.ToByteSize(this);

        public void ForEach(Action<byte> action)
        {
            this.ToByteList().ForEach(action);
        }

        /// <summary>
        /// Sets the list to a specific amount of bits.
        /// If the preferred <paramref name="Length"/> is longer than the current amount of bits the list will be filled with zeros.
        /// If the list is longer than <paramref name="Length"/> the leading bits will be cut off
        /// </summary>
        /// <param name="Length">The number of bits the bitlist will get</param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public BitList ToLength(int Length)
        {
            if (Length < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(Length));
            }
            if (Length < this.Count)
            {
                return new BitList(this.GetRange(this.Count - Length, Length));
            }
            if (Length > this.Count)
            {
                return this.PadLeft(Length);
            }
            return this;
        }

        /// <inheritdoc cref = "List{T}.IndexOf(T)"/>
        public int IndexOf(IEnumerable<Bit> item) => this.IndexOf(item, 0, this.Count);
        /// <inheritdoc cref="List{T}.IndexOf(T, int)"/>
        public int IndexOf(IEnumerable<Bit> item, int StartIndex) => this.IndexOf(item, StartIndex, this.Count);
        /// <inheritdoc cref="List{T}.IndexOf(T, int, int)"/>
        public int IndexOf(IEnumerable<Bit> item, int StartIndex, int Count)
        {
            if (item is null)
            {
                return -1;
            }
            int ItemsCounted = item.Count();
            if (ItemsCounted == 0)
            {
                return -1;
            }
            if (ItemsCounted > (this.Count - StartIndex))
            {
                return -1;
            }
            for (int j = 0; StartIndex < this.Count; StartIndex++, j++)
            {
                if (j == ItemsCounted)
                    throw new ArgumentOutOfRangeException(nameof(item));
                if (this[StartIndex] == item.ElementAt(j))
                {
                    Count--;
                    if (Count == 0)
                    {
                        return StartIndex - ItemsCounted - 1;
                    }
                    continue;
                }
                Count = ItemsCounted;
            }
            return -1;
        }

        /// <inheritdoc cref="List{T}.GetRange(int, int)"/>
        public BitList GetRange(int index)
        {
            if (index < 0 || index >= this.Count)
            {
                if (this.Count == 0)
                {
                    throw new Exception("You can't get a range from an empty list.");
                }
                throw new IndexOutOfRangeException();
            }
            return new BitList(base.GetRange(index, this.Count - index - 1));
        }

        /// <summary>
        /// Adds a range to the end of the current instance of this <see cref="BitList"/>.
        /// </summary>
        /// <param name="Value">The <see cref="List{T}"/> which will be added.</param>
        public void AddRange(IEnumerable<bool> Value)
        {
            foreach (bool b in Value)
            {
                this.Add(b);
            }
        }

        /// <summary>
        /// Adds the 8-bit range of an byte to the end of the current instance if this <see cref="BitList"/>.
        /// </summary>
        /// <param name="Value">The byte which will be added.</param>
        public void Add(byte Value)
        {
            this.AddRange(BitList.ToBitList(Value, 8));
        }

        /// <summary>
        /// Reads all the bytes of a path and executes the xor on the current instance with the bits in the file.
        /// </summary>
        /// <param name="Path">The path of the file from which the bits will be used for execution of xor.</param>
        /// <returns>The new <see cref="BitList"/> where xor was executed. If the file is too short the file will be added again to its own <see cref="BitList"/>.
        /// If its too long the <see cref="Bit"/>s will be cut of on the right side.</returns>
        /// <inheritdoc cref="System.IO.File.ReadAllBytes(string)"/>
        public BitList xOrWithFile(string Path)
        {
            BitList file;
            try
            {
                file = System.IO.File.ReadAllBytes(Path);
            }
            catch
            {
                throw;
            }
            while (file.Count < this.Count)
            {
                file.AddRange(file);
            }
            if (file.Count > this.Count)
            {
                file = new BitList(file.GetRange(0, this.Count));
            }
            return this ^ file;
        }

        /// <summary>
        /// The actual value of the <see cref="BitList"/>.
        /// </summary>
        public BigInteger Number
        {
            get
            {
                return BitList.ToNumber(this);
            }
        }

        /// <summary>
        /// Converts the list of bits into a <see cref="Color"/>. It will use 8 bit segments for each alpha, red, green and blue value. The list of bits will align right.
        /// Leading zeros won't throw an exception if only the last 32 items contains ones.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public Color ToColor() => BitList.ToColor(this);

        public byte[] ToByteArray() => BitList.ToByteArray(this);

        public List<byte> ToByteList() => BitList.ToByteList(this);

        public string ToString(int PadLeft) => this.ToString().PadLeft(PadLeft, '0');

        /// <inheritdoc/>
        public override string ToString()
        {
            StringBuilder retruningValue = new StringBuilder();
            {
                Action<Bit> act = Bit => retruningValue.Append(Bit);
                this.ForEach(act);
            }
            if (this.Count == 0)
            {
                retruningValue.Append('0');
            }
            return retruningValue.ToString();
        }

        public virtual string ToString(string Format)
        {
            if (string.IsNullOrEmpty(Format))
            {
                return ToString();
            }
            if (Format.StartsWith('0'))
            {
                if (Format.TrimStart('0').StartsWith('.'))
                {
                    if (Format.Trim('0').Length == 1)
                    {
                        return BitList.ToNumber(this).ToString().PadLeft(Format.Length - Format.TrimStart('0').Length, '0') + '.' + Format.TrimStart('0').Substring(1);
                    }
                }
                return this.ToString();
            }
            if (Format.ToLower() == "g")
            {
                return BitList.ToNumber(this).ToString();
            }
            int val;
            if (!int.TryParse(Format.Substring(1), out val))
            {
                return this.ToString();
            }
            switch (Format.ToLower()[0])
            {
                case 'd':
                    return BitList.ToNumber(this).ToString().PadLeft(val, '0');
                case 'f':
                    if (val == 0)
                    {
                        return BitList.ToNumber(this).ToString();
                    }
                    return BitList.ToNumber(this).ToString() + ".".PadRight(val + 1, '0');
                case 'x':
                    StringBuilder sb = new StringBuilder();
                    foreach (var b in ToByteArray())
                    {
                        sb.Append(b.ToString("x2"));
                    }
                    return sb.ToString().PadLeft(val, '0');

            }
            return this.ToString();
        }

        public override bool Equals(object obj)
        {
            if (obj is List<Bit> | obj is BitList)
            {
                return this.Number == new BitList((List<Bit>)obj).Number;
            }
            if (obj is Bit[] BitArray)
            {
                return this.Number == new BitList(BitArray).Number;
            }
            return false;
        }

        public virtual bool Equals(object obj, bool OnlyCompareNumber)
        {
            if (!OnlyCompareNumber)
            {
                return this.Equals(obj);
            }
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

        #region Properties

        public static readonly BitList Empty = new();

        #endregion

        #region Static Functions

        /// <summary>
        /// Converts a Number to an object of the <see cref="BitList"/> class.
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

        /// <summary>
        /// Returns a new <see cref="BitList"/> with a total length which is dividable by 8.
        /// The <paramref name="referenceList"/> will be filled with leading zeros if necessary.
        /// </summary>
        /// <param name="referenceList">The List of <see cref="Bit"/>s which will be adjusted. Instances of <see cref="BitList"/> can also be passed.</param>
        /// <returns></returns>
        public static BitList ToByteSize(IEnumerable<Bit> referenceList)
        {
            var result = new BitList(referenceList);
            byte mod8 = (byte)(8 - (result.Count % 8));
            while (mod8 != 0)
            {
                result.Insert(0, false);
                mod8--;
            }
            return result;
        }

        public static BigInteger ToNumber(IEnumerable<Bit> BitCode)
        {
            int BitCodeLength = BitCode.Count();
            BigInteger Value = 0;
            for (int i = 0; i < BitCodeLength; i++)
            {
                Value += BitCode.ElementAt(i)[BitCodeLength - i - 1];
            }
            return Value;
        }

        /// <summary>
        /// Converts the list of bits into a <see cref="Color"/>. It will use 8 bit segments for each alpha, red, green and blue value. The list of bits will align right.
        /// Leading zeros won't throw an exception if only the last 32 items contains ones.
        /// </summary>
        /// <param name="Bitlist"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static Color ToColor(IEnumerable<Bit> Bitlist)
        {
            int length = Bitlist.Count();
            if (length > 32)
            {
                int bitsToSkip = 0;
                for (int i = 0; i + 32 < length; i++)
                {
                    if ((bool)Bitlist.ElementAt(i)) throw new ArgumentOutOfRangeException(nameof(Bitlist), $"List of bits has more bits than the maximum amount of {32}");
                    bitsToSkip++;
                }
                Bitlist = Bitlist.Skip(bitsToSkip);
                length -= bitsToSkip;
            }
            if (length < 32)
            {
                int fillcount = 32 - length;
                Bitlist = Enumerable.Repeat(new Bit(false), fillcount);
                length += fillcount;
            }
            Debug.WriteLine(Bitlist);
            return Color.FromArgb((int)BitList.ToNumber(Bitlist.Take(8)),
                                  (int)BitList.ToNumber(Bitlist.Take(new Range(8, 16))),
                                  (int)BitList.ToNumber(Bitlist.Take(new Range(16, 24))),
                                  (int)BitList.ToNumber(Bitlist.Skip(24)));
        }

        /// <summary>
        /// Takes the <see cref="Bit"/>s from <paramref name="bitList"/> and converts it into an array of <see cref="byte"/>s.
        /// If the length does not match it will add leading zeros.
        /// </summary>
        /// <param name="bitList"></param>
        /// <returns></returns>
        public static byte[] ToByteArray(IEnumerable<Bit> bitList)
        {
            return BitList.ToByteList(bitList).ToArray();
        }

        public static List<byte> ToByteList(IEnumerable<Bit> Bits)
        {
            List<byte> Bytes = new List<byte>();
            List<Bit> bitList = BitList.ToByteSize(Bits);
            int ItemsCounted = Bits.Count();
            for (int i = 0; i < ItemsCounted; i += 8)
            {
                Bytes.Add((byte)BitList.ToNumber(bitList.GetRange(i, 8)));
            }
            return Bytes;
        }

        /// <summary>
        /// Moves all the <see cref="Bit"/>s in <see cref="List{T}"/> of <paramref name="BitCode"/> by the amount of <paramref name="Value"/>.
        /// </summary>
        /// <param name="BitCode"></param>
        /// <param name="Value">How many times the bit should be moved in the right direction</param>
        /// <returns></returns>
        public static BitList BitWiseOperator(IEnumerable<Bit> BitCode, BigInteger Value)
        {
            int BitCodeLength = BitCode.Count();
            if (BitCodeLength < 2)
            {
                return new BitList(BitCode);
            }
            int Moves = (int)(((Value % BitCodeLength) + BitCodeLength) % BitCodeLength);
            BitList New_BitCode = new();
            New_BitCode.AddRange(BitCode.Skip(BitCodeLength - Moves));
            New_BitCode.AddRange(BitCode.Take(new Range (0, BitCodeLength - Moves)));
            return New_BitCode;
        }

        private static void ToSameLength(ref BitList Value1, ref List<Bit> Value2)
        {
            //Value2.InsertRange(0, Value1);
            if (Value1.Count < Value2.Count)
            {
                Debug.WriteLine("schlupp");
                Value1.InsertRange(0, new Bit[Value2.Count - Value1.Count]);
            }
            if (Value1.Count > Value2.Count)
            {
                Debug.WriteLine("plop");
                Value1.InsertRange(0, new Bit[Value1.Count - Value2.Count]);
            }
        }

        private static void ToSameLength(ref BitList Value1, ref IEnumerable<Bit> Value2)
        {
            int Val1Length = Value1.Count();
            int Val2Length = Value2.Count();
            //Value2.InsertRange(0, Value1);
            if (Val1Length > Val2Length)
            {
                Value2 = Enumerable.Repeat(new Bit(false), Val2Length - Val1Length).Concat(Value2);
            }
            if (Val1Length < Val2Length)
            {
                Value1.InsertRange(0, new Bit[Val2Length - Val1Length]);
            }
        }

        private static bool GreaterThan(IEnumerable<Bit> Value1, IEnumerable<Bit> Value2)
        {
            Value1 = new BitList(Value1).TrimStart();
            Value2 = new BitList(Value2).TrimStart();
            int Val1Length = Value1.Count();
            int Val2Length = Value2.Count();
            if (Val1Length > Val2Length)
            {
                return true;
            }
            if (Val2Length > Val1Length)
            {
                return false;
            }
            for (int i = 0; i < Val2Length; i++)
            {
                if (Value1.ElementAt(i) > Value2.ElementAt(i))
                {
                    return true;
                }
            }
            return false;
        }

        public static BitList Parse(string s, IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        public static bool TryParse([NotNullWhen(true)] string s, IFormatProvider provider, [MaybeNullWhen(false)] out BitList result)
        {
            DateTime.Parse(s);
            try
            {
                result = BitList.Parse(s, provider);
                return true;
            }
            catch
            {
                result = BitList.Empty;
            }
            throw new NotImplementedException();
            //return false;
        }

        #endregion

        #region Implicit Operators

        public static implicit operator BitList(bool Value)
        {
            return new BitList(Value);
        }

        public static implicit operator BitList(bool[] Value)
        {
            BitList result = new BitList();
            result.AddRange(Value);
            return result;
        }

        public static implicit operator BitList(List<bool> Value)
        {
            BitList result = new BitList();
            result.AddRange(Value);
            return result;
        }

        public static implicit operator BitList(Stack<bool> Value)
        {
            BitList result = new BitList();
            result.AddRange(Value);
            return result;
        }

        public static implicit operator BitList(Bit Value)
        {
            return new BitList(Value);
        }

        public static implicit operator BitList(Bit[] Value)
        {
            return new BitList(Value);
        }

        public static implicit operator BitList(Stack<Bit> Value)
        {
            return new BitList(Value);
        }

        public static implicit operator BitList(byte Value)
        {
            return BitList.ToBitList(Value).PadLeft(8, false);
        }

        public static implicit operator BitList(byte[] Value)
        {
            ArgumentNullException.ThrowIfNull(Value, nameof(Value));
            if (Value.Length == 0)
            {
                throw new Exception("Total length need to be bigger than 0");
            }
            BitList val = BitList.Empty;
            for (int i = 0; i < Value.Length; i++)
            {
                val.Add(Value[i]);
            }
            return val;
        }

        public static implicit operator BitList(BigInteger Value)
        {
            return BitList.ToBitList(Value);
        }

        public static implicit operator BitList(long Value)
        {
            return BitList.ToBitList(Value);
        }

        #endregion

        #region Explicit Operators

        public static explicit operator Bit[](BitList Value)
        {
            return Value.ToArray();
        }

        public static explicit operator bool[](BitList Value)
        {
            bool[] result = new bool[Value.Count];
            for (int i = 0; i < Value.Count; i++)
            {
                result[i] = (bool)Value[i];
            }
            return result;
        }

        public static explicit operator byte[](BitList Value)
        {
            return BitList.ToByteArray(Value);
        }

        public static explicit operator BigInteger(BitList Value)
        {
            return BitList.ToNumber(Value);
        }

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
        public static BigInteger operator +(BitList Value1, BitList Value2) => Value1 + (IEnumerable<Bit>)Value2;
        public static BigInteger operator +(IEnumerable<Bit> Value1, BitList Value2) => Value2 + Value1;
        public static BigInteger operator +(BitList Value1, IEnumerable<Bit> Value2)
        {
            return BitList.ToNumber(Value1) + BitList.ToNumber(Value2);
        }

        public static BitList operator -(Bit Value1, BitList Value2) => new BitList(Value1) - Value2;
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
        public static BigInteger operator -(BitList Value1, IEnumerable<Bit> Value2) => Value1 - new BitList(Value2);
        public static BigInteger operator -(IEnumerable<Bit> Value1, BitList Value2) => new BitList(Value1) - Value2;
        public static BigInteger operator -(BitList Value1, BitList Value2)
        {
            return BitList.ToNumber(Value1) - BitList.ToNumber(Value2);
        }

        public static BigInteger operator *(BitList Value1, BitList Value2) => Value1 * (IEnumerable<Bit>)Value2;
        public static BigInteger operator *(IEnumerable<Bit> Value1, BitList Value2) => Value2 * Value1;
        public static BigInteger operator *(BitList Value1, IEnumerable<Bit> Value2)
        {
            return BitList.ToNumber(Value1) * BitList.ToNumber(Value2);
        }

        public static BigInteger operator /(BitList Value1, BitList Value2) => Value1 / (IEnumerable<Bit>)Value2;
        public static BigInteger operator /(IEnumerable<Bit> Value1, BitList Value2) => Value2 / Value1;
        public static BigInteger operator /(BitList Value1, IEnumerable<Bit> Value2)
        {
            return BitList.ToNumber(Value1) / BitList.ToNumber(Value2);
        }

        /// <summary>
        /// Moves the <see cref="Bit"/>s in <paramref name="bitList"/> by the amount of <paramref name="MoveTimes"/> to the left.
        /// The bits will be added to the right again.
        /// </summary>
        /// <param name="bitList"></param>
        /// <param name="MoveTimes"></param>
        /// <returns></returns>
        public static BitList operator <<(BitList bitList, int MoveTimes)
        {
            return BitList.BitWiseOperator(bitList, -MoveTimes);
        }

        /// <summary>
        /// Moves the <see cref="Bit"/>s in <paramref name="bitList"/> by the amount of <paramref name="MoveTimes"/> to the right.
        /// The bits will be added to the left again.
        /// </summary>
        /// <param name="bitList"></param>
        /// <param name="MoveTimes"></param>
        /// <returns></returns>
        public static BitList operator >>(BitList bitList, int MoveTimes)
        {
            return BitList.BitWiseOperator(bitList, MoveTimes);
        }

#if NET7_0_OR_GREATER
        public static BitList operator >>>(BitList value, int MoveBits)
        {
            throw new NotImplementedException();
            //return BitList.BitWiseOperator(value, MoveBits);
        }
#endif


        public static BitList operator ^(BitList Value1, BitList Value2) => Value1 ^ (IEnumerable<Bit>)Value2;
        public static BitList operator ^(IEnumerable<Bit> Value1, BitList Value2) => Value2 ^ Value1;
        public static BitList operator ^(BitList Value1, IEnumerable<Bit> Value2)
        {
            BitList.ToSameLength(ref Value1, ref Value2);
            int ListLength = Value1.Count;
            for (int i = 0; i < ListLength; i++)
            {
                //Debug.WriteLine("hi " + i);
                Value1[i] ^= Value2.ElementAt(i);
            }
            Debug.WriteLine("ready");
            return Value1;
        }

        public static BitList operator &(BitList Value1, BitList Value2) => Value1 & (IEnumerable<Bit>)Value2;
        public static BitList operator &(IEnumerable<Bit> Value1, BitList Value2) => Value2 & Value1;
        public static BitList operator &(BitList Value1, IEnumerable<Bit> Value2)
        {
            ToSameLength(ref Value1, ref Value2);
            int ListLength = Value1.Count;
            for (int i = 0; i < ListLength; i++)
            {
                Value1[i] &= Value2.ElementAt(i);
            }
            return Value1;
        }

        public static BitList operator |(BitList Value1, BitList Value2) => Value1 | (IEnumerable<Bit>)Value2;
        public static BitList operator |(IEnumerable<Bit> Value1, BitList Value2) => Value2 | Value1;
        public static BitList operator |(BitList Value1, IEnumerable<Bit> Value2)
        {
            ToSameLength(ref Value1, ref Value2);
            int ListLength = Value1.Count;
            for (int i = 0; i < ListLength; i++)
            {
                Value1[i] |= Value2.ElementAt(i);
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
        public static bool operator <(IEnumerable<Bit> Value1, BitList Value2) => GreaterThan(Value2, Value1);
        public static bool operator <(BitList Value1, IEnumerable<Bit> Value2) => GreaterThan(Value2, Value1);

        public static bool operator >(BitList Value1, BitList Value2) => GreaterThan(Value1, Value2);
        public static bool operator >(IEnumerable<Bit> Value1, BitList Value2) => GreaterThan(Value1, Value2);
        public static bool operator >(BitList Value1, IEnumerable<Bit> Value2) => GreaterThan(Value1, Value2);

        public static bool operator <=(BitList Value1, BitList Value2) => !GreaterThan(Value1, Value2);
        public static bool operator <=(IEnumerable<Bit> Value1, BitList Value2) => !GreaterThan(Value1, Value2);
        public static bool operator <=(BitList Value1, IEnumerable<Bit> Value2) => !GreaterThan(Value1, Value2);

        public static bool operator >=(BitList Value1, BitList Value2) => !GreaterThan(Value2, Value1);
        public static bool operator >=(IEnumerable<Bit> Value1, BitList Value2) => !GreaterThan(Value2, Value1);
        public static bool operator >=(BitList Value1, IEnumerable<Bit> Value2) => !GreaterThan(Value2, Value1);

        public static bool operator ==(BitList Value1, BitList Value2) => Value1.Equals(Value2);
        public static bool operator ==(IEnumerable<Bit> Value1, BitList Value2) => Value2.Equals(Value1);
        public static bool operator ==(BitList Value1, IEnumerable<Bit> Value2) => Value1.Equals(Value2);
        
        public static bool operator !=(BitList Value1, BitList Value2) => !Value1.Equals(Value2);
        public static bool operator !=(IEnumerable<Bit> Value1, BitList Value2) => !Value2.Equals(Value1);
        public static bool operator !=(BitList Value1, IEnumerable<Bit> Value2) => !Value1.Equals(Value2);

        #endregion
    }
}