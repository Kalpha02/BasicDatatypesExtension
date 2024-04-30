using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    public static class TimeBasedRandom
    {
        private const string URL = "https://qrng.anu.edu.au/API/jsonI.php?type=hex16&length=[length]&size=[size]";

        public static BigInteger Next(BigInteger MinimumValue, BigInteger MaximumPlus1)
        {
            Task<BigInteger> rnd = GetData(4);
            rnd.Wait();
            if (rnd.IsFaulted)
            {
                throw rnd.Exception;
            }
            return rnd.Result;
        }

        private async static Task<BigInteger> GetData(short Length, short Size = 1)
        {
            if (Length <= 0 | Length > 1024)
            {
                throw new ArgumentOutOfRangeException(nameof(Length));
            }
            if (Size <= 0 | Size > 1024)
            {
                throw new ArgumentOutOfRangeException(nameof(Size));
            }
            try
            {
                using var client = new HttpClient();
                var response = await client.GetByteArrayAsync(URL.Replace("[length]", Length.ToString()).Replace("[size]", Size.ToString()));

                using var stream = new MemoryStream(response);
                using var reader = new BinaryReader(stream);

                byte[] Bites = reader.ReadBytes(0);
                for (int i = 0; i < 24; i++)
                {
                    byte binaryByte = reader.ReadByte();
                    string binaryString = Convert.ToString(binaryByte, 2).PadLeft(8, '0');
                    Console.WriteLine($"Binär {i + 1}: {binaryString}");
                }
            }
            catch
            {
                throw;
            }
            return 0;
        }
    }
}