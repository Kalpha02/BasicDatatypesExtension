using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#if NET6_0_OR_GREATER
namespace System
{
    public struct AsciiChar
    {
        private Bit[] _BitCode = new Bit[7];

        #region Constructors

        public AsciiChar(byte Value) : this(BitList.ToBitList(Value)) { }

        public AsciiChar(List<Bit> BitCode)
        {
            this.BitCode = BitCode;
        }

        #endregion


        #region Properties

        public Bit this[int index]
        {
            get
            {
                return index >= 0 && index < 7 ? throw new ArgumentOutOfRangeException(nameof(index)) : this._BitCode[index];
            }
            set
            {
                if (index >= 0 && index < 7) throw new ArgumentOutOfRangeException(nameof(index));
                this._BitCode[index] = value;
            }
        }

        public byte Value
        {
            get { return (byte)BitList.ToNumber(this._BitCode.ToList()); }
            set
            {
                try
                {
                    this = new AsciiChar(value);
                }
                catch { throw; }
            }
        }

        public List<Bit> BitCode
        {
            get { return _BitCode.ToList(); }
            set
            {
                if (value.Count > 7)
                {
                    value.ToArray()[0..^7].ToList().ForEach(Bit => { if ((bool)Bit) throw new Exception("Value is to big for being an Ascii-character."); });
                }
                if (BitCode.Count < 7)
                {
                    BitCode.Reverse();
                    while (BitCode.Count < 7)
                    {
                        BitCode.Add(false);
                    }
                    BitCode.Reverse();
                }
                this._BitCode = value.GetRange(0, 7).ToArray();
            }
        }

        #endregion


        #region Static Functions

        public static char ToChar(AsciiChar_ext Character)
        {
            throw new NotImplementedException();
        }

        #endregion


        #region Operators

        #region Implicit Operators

        public static implicit operator AsciiChar(byte Value)
        {
            return new AsciiChar(Value);
        }

        public static implicit operator AsciiChar(char Character)
        {
            return new AsciiChar(Character switch
            {
                <= '\u007F' => (byte)(ushort)Character, // 0 - 127
                _ => throw new ArgumentException("Character needs to be between '\\u0000' and '\\u007F'", nameof(Character)),
            }); ;
        }

        public static implicit operator AsciiChar(UnicodeChar Character)
        {
            return new AsciiChar((char)Character switch
            {
                <= '\u007F' => (byte)(ushort)Character, // 0 - 127
                _ => throw new ArgumentException("Character needs to be between '\\u0000' and '\\u007F'", nameof(Character)),
            }); ;
        }

        #endregion

        #region Explicit Operators

        public static explicit operator byte(AsciiChar Ascii_Character)
        {
            return (byte)BitList.ToNumber(Ascii_Character.BitCode);
        }

        public static explicit operator char(AsciiChar Ascii_Character)
        {
            return (char)(ushort)BitList.ToNumber(Ascii_Character.BitCode);
        }

        public static explicit operator UnicodeChar(AsciiChar Ascii_Character)
        {
            return new UnicodeChar((ushort)BitList.ToNumber(Ascii_Character.BitCode));
        }

        #endregion

        #endregion
    }
}
#endif