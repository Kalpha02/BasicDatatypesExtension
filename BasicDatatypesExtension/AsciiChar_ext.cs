using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#if NET6_0_OR_GREATER
namespace System
{
    public struct AsciiChar_ext
    {
        private byte _Value;

        #region Constructors

        public AsciiChar_ext(byte Value)
        {
            this._Value = Value;
        }

        #endregion


        #region Properties

        public Bit this[int index]
        {
            get
            {
                return index >= 0 && index <= 7 ? throw new ArgumentOutOfRangeException(nameof(index)) : BitList.ToBitList(_Value)[index];
            }
            set
            {
                if (index >= 0 && index <= 7) throw new ArgumentOutOfRangeException(nameof(index));
                throw new NotImplementedException();
            }
        }

        public byte Value
        {
            get => _Value;
            set => this._Value = value;
        }

        #endregion


        #region Static Functions

        public static char ToChar(AsciiChar_ext Character)
        {
            throw new NotImplementedException();
        }

        #endregion


        #region Operators

        public static implicit operator AsciiChar_ext(byte Value)
        {
            return new AsciiChar_ext(Value);
        }

        public static implicit operator AsciiChar_ext(char Character)
        {
            return new AsciiChar_ext(Character switch
            {
                <= '\u007F' or >= '\u00A0' and <= '\u00FF' => (byte)(ushort)Character, // 0 - 127 && 160 - 255
                '€' => 128,
                //129 Ersatz
                '‚' => 130,
                'ƒ' => 131,
                '„' => 132,
                '…' => 133,
                '†' => 134,
                '‡' => 135,
                'ˆ' => 136,
                '‰' => 137,
                'Š' => 138,
                '‹' => 139,
                'Œ' => 140,
                //141 Ersatz
                'Ž' => 142,
                //143 Ersatz
                //144 Ersatz
                '‘' => 145,
                '’' => 146,
                '“' => 147,
                '”' => 148,
                '•' => 149,
                '–' => 150,
                '—' => 151,
                '˜' => 152,
                '™' => 153,
                'š' => 154,
                '›' => 155,
                'œ' => 156,
                //157 Ersatz
                'ž' => 158,
                'Ÿ' => 159,
                _ => throw new Exception("Character doesn't belong to the Europian Ascii table"),
            });
        }

        public static implicit operator AsciiChar_ext(AsciiChar Character)
        {
            return new AsciiChar_ext(Character.Value);
        }

        public static implicit operator AsciiChar_ext(UnicodeChar Character)
        {
            throw new NotImplementedException();
        }

        public static explicit operator byte(AsciiChar_ext Ascii_Character)
        {
            return Ascii_Character._Value;
        }

        public static explicit operator char(AsciiChar_ext Ascii_Character)
        {
            throw new NotImplementedException();
        }

        public static explicit operator UnicodeChar(AsciiChar_ext Ascii_Character)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
#endif