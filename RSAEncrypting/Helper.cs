using System;
using System.Numerics;

namespace RSAEncrypting
{
    public static class Helper
    {
        public static BigInteger RandomIntegerBelow(BigInteger N)
        {
            BigInteger R;
            Random random = new Random();
            byte[] bytes = N.ToByteArray();
            do
            {
                random.NextBytes(bytes);
                bytes[bytes.Length - 1] &= (byte)0x7F; //force sign bit to positive
                R = new BigInteger(bytes);
            } while (R >= N);

            return R;
        }
    }
}
