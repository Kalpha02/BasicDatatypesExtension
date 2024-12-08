using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    public class ByteList : List<byte>
#if NET7_0_OR_GREATER
        /*, IAdditionOperators<ByteList, ByteList, BigInteger>
        , ISubtractionOperators<ByteList, ByteList, BigInteger>
        , IMultiplyOperators<ByteList, ByteList, BigInteger>
        , IDivisionOperators<ByteList, ByteList, BigInteger>
        , IParsable<ByteList>*/
#endif
    {
        private List<Bit> _Overflow;

        #region Constructors

        public ByteList() { }

        public ByteList(params byte[] bytes)
        {
            foreach (var BYTE in bytes)
                this.Add(BYTE);
        }

        public ByteList(List<byte> Bits)
        {
            Bits.ForEach(this.Add);
        }

        public ByteList(params Bit[] Value)
        {
            foreach (var Bit in Value) ;
                //this.Add(Bit);
        }

        public ByteList(List<Bit> Bits)
        {
            //Bits.ForEach(this.Add);
        }
        #endregion

        #region Properties

        #endregion
    }
}