using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    public static class Conversion
    {
        public static string ToHex(IEnumerable<Bit> Val)
        {
            StringBuilder sb = new StringBuilder();

            int Elements = Val.Count();

            if (Elements % 4 != 0)
            {
                Val = Enumerable.Repeat(new Bit(false), 4 - Elements % 4).Concat(Val);
                Elements += 4 - (Elements % 4);
            }

            HexadecimalChar character = new HexadecimalChar();
            for (int i = 0; i < Elements; i++)
            {
                character[i % 4] = Val.ElementAt(i);
                if (i % 4 == 3)
                {
                    sb.Append(character.ToString());
                }
            }
            return sb.ToString();
        }
    }
}