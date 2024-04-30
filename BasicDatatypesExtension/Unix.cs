using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    public partial struct Unix
    {
        BigInteger Value;

        public Unix(BigInteger Ticks)
        {
            Value = Ticks;
        }

        public Unix(DateTime dateTime)
        {
            Value = Unix.Parse(dateTime);
        }

        public BigInteger ToBigInteger()
        {
            return Value;
        }

        public DateTime ToDateTime()
        {
            return Unix.ToDateTime(this);
        }

        public static DateTime ToDateTime(Unix unixTimestamp)
        {
            return new DateTime(621355968000000000 + (TimeSpan.TicksPerSecond * (long)unixTimestamp.Value));
        }

        public static BigInteger Parse(DateTime date)
        {
            return (date.Ticks - 621355968000000000) / TimeSpan.TicksPerSecond;
        }

        public static implicit operator Unix(BigInteger Ticks)
        {
            return new Unix(Ticks);
        }

        public static explicit operator BigInteger(Unix unixTimestamp)
        {
            return unixTimestamp.Value;
        }
    }
}