using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#if NET5_0_OR_GREATER
namespace System
{
    /// <summary>
    /// Contains a 16-Bit value stands for a unicode character
    /// </summary>
    public struct UnicodeChar
    {
        #region Variables

        private ushort _Value;

        #endregion


        #region Constructors

        public UnicodeChar(ushort Character)
        {
            this._Value = (char)Character;
        }

        public UnicodeChar(char Character)
        {
            this._Value = Character;
        }

        internal UnicodeChar(string Hexadecimal_Unicode)
        {
            this._Value = 0;
            this.Haxadicimal_Unicode = Hexadecimal_Unicode;
        }

        #endregion


        #region Functions

        #region Checking
        public bool IsEnglishLetter()
        {
            return this._Value switch
            {
                >= 'A' and <= 'Z' or >= 'a' and <= 'z' => true,
                _ => false,
            };
        }

        public bool IsGermanLetter()
        {
            return this._Value switch
            {
                >= 'A' and <= 'Z' or >= 'a' and <= 'z' or 'Ä' or 'a' or 'Ö' or 'ö' or 'Ü' or 'ü' or 'ß' => true,
                _ => false,
            };
        }

        public bool IsGreekLetter()
        {
            return this._Value switch
            {
                >= 'Α' /*΢ ist aus irgendwelchen Gründen dazwischen*/ and <= 'Ρ' or >= 'Σ' and 'Ω' or >= 'α' and <= 'ω' => true,
                _ => false
            };
        }

        public bool IsNumeral()
        {
            try
            {
                ToNumeral();
                return true;
            }
            catch
            {
                return false;
            }
        }

        #endregion

        #region Converting

        /// <summary>
        /// If the Character is a number it will return them
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public double ToNumeral()
        {
            double result = char.GetNumericValue((char)this._Value);
            return result != -1 ? result : throw new Exception("Character is not a numeral");
        }

        /// <summary>
        /// Converts the value to an actual character as a string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return ((char)_Value).ToString();
        }

        #endregion

        public override bool Equals(object obj)
        {
            return obj is UnicodeChar ? this.GetHashCode() == (UnicodeChar)obj.GetHashCode() : throw new Exception("object is null or a wrong datatype");
        }

        public bool Equals(ushort obj)
        {
            return this._Value.GetHashCode() == obj.GetHashCode();
        }

        public override int GetHashCode()
        {
            return this._Value;
        }

        #endregion


        #region Static Functions

        public static UnicodeChar FromUnicode(string Value)
        {
            while (Value.Length > 4)
            {
                if (Value[0] != '0')
                {
                    throw new ArgumentException("Value is hot a 4 digit hexadecimal number.");
                }
                Value = Value.Substring(1);
            }
            Value = Value.ToUpper().PadLeft(4, '0');
            UnicodeChar returning_Unicode = 0;
            for (int i = 0; i < 4; i++)
            {
                returning_Unicode += Value[i] switch
                {
                    >= '0' and <= '9' => (Value[i] - 48) * (ushort)Math.Pow(16, 3 - i),
                    >= 'A' and <= 'F' => (Value[i] - 55) * (ushort)Math.Pow(16, 3 - i),
                    _ => throw new Exception($"{Value} is not a valid Unicode."),
                };
            }
            return returning_Unicode;
        }

        /// <summary>
        /// Returns the character as a hexadecimal code
        /// </summary>
        /// <param name="Character"></param>
        /// <returns>A 4 digit string</returns>
        public static string ToUnicode(UnicodeChar Character)
        {
            string ReturningValue = "";
            bool GotValue = false;
            for (int i = ((ushort)Character).ToString().Length - 1; i >= 0; i--)
            {
                byte n = (byte)(((ushort)Character) / Math.Pow(16, i));
                if (n != 0 || GotValue)
                {
                    GotValue = true;
                    switch (n)
                    {
                        case 0:
                        case 1:
                        case 2:
                        case 3:
                        case 4:
                        case 5:
                        case 6:
                        case 7:
                        case 8:
                        case 9:
                            ReturningValue += n.ToString();
                            break;
                        case 10:
                        case 11:
                        case 12:
                        case 13:
                        case 14:
                        case 15:
                            ReturningValue += ((char)(n + 55)).ToString();
                            break;
                    }
                    Character -= (ushort)(Math.Pow(16, i) * n);
                }
            }
            return ReturningValue.PadLeft(4, '0');
        }

        /// <summary>
        /// Converts a list of bits to an actual Unicode Character
        /// </summary>
        /// <param name="BTC">Bitcode can contain more than 16 bits if the higher digits are false</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static UnicodeChar FromBitCode(List<Bit> BTC)
        {
            while (BTC.Count > 16)
            {
                if ((bool)BTC.First()) throw new ArgumentOutOfRangeException(nameof(BTC), "List of bits is greater than 16 bits.");
                BTC.RemoveAt(0);
            }
            return (ushort)BitList.ToNumber(BTC);
        }

        /// <summary>
        /// converts a unicode character to a bitcode
        /// </summary>
        /// <param name="Character">The Character that is converted</param>
        /// <param name="PadLeft">The number of zeros ahead if it is not long enough</param>
        /// <returns></returns>
        public static BitList GetBitCode(UnicodeChar Character, int PadLeft = 0)
        {
            return BitList.ToBitList(Character._Value, PadLeft);
        }

        #endregion


        #region Properties

        /// <summary>
        /// Gets or sets a specific bit of the unicode Character
        /// </summary>
        /// <param name="index">Sets the index where the bit is get or set. Can't be bigger than 15.</param>
        /// <returns></returns>
        public Bit this[int index]
        {
            get => index >= 0 && index < 16 ? throw new ArgumentOutOfRangeException(nameof(index)) : (Bit)(bool)BitCode[index];
            set
            {
                if (index >= 0 && index < 16) throw new ArgumentOutOfRangeException(nameof(index));
                var BitCode = this.BitCode;
                BitCode[index] = value;
                this.BitCode = BitCode;
            }
        }

        public string Haxadicimal_Unicode
        {
            get
            {
                return ToUnicode(this);
            }
            set
            {
                try
                {
                    this.Value = (ushort)FromUnicode(value);
                }
                catch
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Gets or Sets the value of the Unicode Character
        /// </summary>
        public ushort Value
        {
            get
            {
                return this._Value;
            }
            set
            {
                this._Value = value;
            }
        }

        /// <summary>
        /// Gets or Sets the value of the Bit-Code of the Unicode Character
        /// </summary>
        public List<Bit> BitCode
        {
            get
            {
                return GetBitCode(this, 16);
            }
            set
            {
                if (value is null)
                {
                    return;
                }
                try
                {
                    this.Value = (ushort)FromBitCode(value);
                }
                catch
                {
                    throw;
                }
            }
        }

        #endregion


        #region Operators

        #region Implicit Operators

        public static implicit operator UnicodeChar(ushort Character)
        {
            return new UnicodeChar(Character);
        }

        public static implicit operator UnicodeChar(long Character)
        {
            Character %= 65536;
            if (Character < 0)
            {
                Character += 65536;
            }
            return new UnicodeChar((ushort)Character);
        }

        public static implicit operator UnicodeChar(char Character)
        {
            return new UnicodeChar(Character);
        }

        public static implicit operator UnicodeChar(AsciiChar Character)
        {
            return new UnicodeChar(Character.Value);
        }

        public static implicit operator UnicodeChar(AsciiChar_ext Character)
        {
            throw new NotImplementedException("Range 128 to 168 is not the same like UTF. Function for converting is not implemented yet.");
        }

        #endregion

        #region Explicit Operators

        public static explicit operator byte(UnicodeChar Character)
        {
            if (Character._Value >= 128)
            {
                throw new Exception("Character is to big for being converted to byte");
            }
            return (byte)Character._Value;
        }

        public static explicit operator ushort(UnicodeChar Character)
        {
            return Character._Value;
        }

        public static explicit operator char(UnicodeChar Character)
        {
            return (char)Character._Value;
        }

        public static explicit operator string(UnicodeChar Character)
        {
            return Character.ToString();
        }

        #endregion

        public static UnicodeChar operator +(UnicodeChar Char1, UnicodeChar Char2)
        {
            return (ushort)((Char1._Value + Char2._Value) % 65536);
        }

        public static UnicodeChar operator ++(UnicodeChar Unicode_Character)
        {
            return Unicode_Character + 1;
        }

        public static UnicodeChar operator -(UnicodeChar Char1, UnicodeChar Char2)
        {
            int value = Char1._Value - Char2._Value;
            if (value < 0)
            {
                value += 65536;
            }
            return (ushort)(value % 65536);
        }

        public static UnicodeChar operator --(UnicodeChar Unicode_Character)
        {
            return Unicode_Character - 1;
        }

        public static bool operator ==(UnicodeChar Char1, UnicodeChar Char2)
        {
            return Char1._Value == Char2._Value;
        }

        public static bool operator !=(UnicodeChar Char1, UnicodeChar Char2)
        {
            return Char1._Value != Char2._Value;
        }

        public static bool operator <(UnicodeChar Char1, UnicodeChar Char2)
        {
            return Char1._Value < Char2._Value;
        }

        public static bool operator >(UnicodeChar Char1, UnicodeChar Char2)
        {
            return Char1._Value > Char2._Value;
        }

        public static UnicodeChar operator <<(UnicodeChar Character, int Value)
        {
            Character.BitCode = BitList.BitWiseOperator(Character.BitCode, -Value);
            return Character;
        }

        public static UnicodeChar operator >>(UnicodeChar Character, int Value)
        {
            Character.BitCode = BitList.BitWiseOperator(Character.BitCode, Value);
            return Character;
        }

        #endregion
    }
}
#endif