using System.Numerics;

namespace RSAEncrypting
{
    public class RSAKeys
    {
        public BigInteger openKeyPart1;
        public BigInteger openKeyPart2;
        public BigInteger secretKeyPart1;
        public BigInteger secretKeyPart2;

        /// <summary>
        /// Конструктор класа
        /// </summary>
        /// <param name="a">Первая часть открытого ключа</param>
        /// <param name="b">Вторая часть открытого ключа</param>
        /// <param name="c">Первая часть закрытого ключа</param>
        /// <param name="d">Вторая часть закрытого ключа</param>
        public RSAKeys(BigInteger a, BigInteger b, BigInteger c, BigInteger d)
        {
            this.openKeyPart1 = a;
            this.openKeyPart2 = b;
            this.secretKeyPart1 = c;
            this.secretKeyPart2 = d;
        }
    }
}
