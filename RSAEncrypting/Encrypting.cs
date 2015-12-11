using System;
using System.Text.RegularExpressions;
using System.Numerics;

namespace RSAEncrypting
{
           
        public class Encrypting
        {
            /// <summary>
            /// Длина окрытого ключа
            /// </summary>
            public BigInteger openKeyLength; 
            /// <summary>
            /// Базовое значениеи для формирования закрытого ключа. Чем оно выше тем выше криптоустойичвость
            /// </summary>
            public BigInteger baseValue;

            /// <summary>
            /// Конструктор класса.
            /// </summary>
            /// <param name="min">Длина окрытого ключа</param>
            /// <param name="max">Базовое значениеи для формирования закрытого ключа. Чем оно выше, тем выше криптоустойичвость.</param>
            public Encrypting(BigInteger min, BigInteger max)
            {
                this.openKeyLength = min;
                this.baseValue = max;
            }

            /// <summary>
            /// Конструктор класса.
            /// </summary>
            public Encrypting()
            { }

            public string RSAEncode(string input, RSAKeys keys)
            {
                string output = string.Empty;
                Char[] input_list = input.ToCharArray();
                foreach (Char symbol in input_list)
                {
                    BigInteger value = (BigInteger)symbol;
                    output += BigInteger.ModPow(value, (BigInteger)keys.openKeyPart1, (BigInteger)keys.openKeyPart2).ToString() + ".";
                }
                return output;
            }

            public string RSADecode(string input, RSAKeys keys)
            {
                string output = string.Empty;
                MatchCollection matches = Regex.Matches(input, @"(-?[0-9]*\.)");
                foreach (Match symbol in matches)
                {
                    BigInteger value = (BigInteger)Convert.ToUInt64(symbol.Value.Replace(".", ""));
                    BigInteger char_code = BigInteger.ModPow(value, (BigInteger)keys.secretKeyPart1, (BigInteger)keys.secretKeyPart2);
                    output += ((char)char_code).ToString();
                }
                return output;
            }

            /// <summary>
            /// Генерирует открытый и закрытытй ключи
            /// </summary>
            /// <returns>Возвращает экземпляр класса RSAKeys, содержащий сгенированные открытый и закрытый ключи</returns>
            public RSAKeys generateKeys()
            {
                BigInteger p = getPrimeNumber(openKeyLength, baseValue);
                BigInteger q = p;
                while (q == p)
                {
                    q = getPrimeNumber(openKeyLength, baseValue);
                }
                BigInteger n = p * q;
                BigInteger e = getCoprimeNumber(openKeyLength, ((p - 1) * (q - 1)), (p - 1) * (q - 1));
                BigInteger d = getNumberE(e, p, q);
                RSAKeys keys = new RSAKeys(e, n, d, n);
                return keys;

            }

            /// <summary>
            /// Получить простое число в указанном диапозоне
            /// </summary>
            /// <param name="min">Включаемое минимальное значение</param>
            /// <param name="max">Включаемое максимальное значение</param>
            /// <returns>Возвращает число, которое делится только на само себя  и на 1</returns>
            private BigInteger getPrimeNumber(BigInteger min, BigInteger max)
            {
                bool ready = false;
                BigInteger key = 0;
                // Random randomize = new Random();                
                while (!ready)
                {
                    key = Helper.RandomIntegerBelow(max);
                    ready = isPrime(key);
                }
                return key;
            }

            /// <summary>
            /// Проверяет является ли число простым
            /// </summary>
            /// <param name="value">Проверяемое число</param>
            /// <returns>Возвращает true или false в звависимости от результата</returns>
            private bool isPrime(BigInteger value)
            {
                int sqrt_ = (int)Math.Sqrt((double)value);
                if (value % 2 == 0)
                {
                    return false;
                }

                for (int i = 3; i <= sqrt_; i += 2)
                {
                    if (value % i == 0)
                        return false;
                }
                return true;
            }


            /// <summary>
            /// Определяет случайное взаимнопростое число для указнного числа в указанном диапозоне
            /// </summary>
            /// <param name="min">Включаемое минимальное значение диапозона</param>
            /// <param name="max">Включаемое максимальное значение диапозона</param>
            /// <param name="value">Число, для которого определяется результат</param>
            /// <returns>Число, являющиеся взаимно простым с value</returns>
            private BigInteger getCoprimeNumber(BigInteger min, BigInteger max, BigInteger value)
            {
                bool ready = false;
                BigInteger key = 0;               
                while (!ready)
                {
                    key = Helper.RandomIntegerBelow(min);
                    if (!isPrime(key)) continue;
                    ready = isCoprime(key, value);
                }
                return key;
            }

            /// <summary>
            /// Определяет по алгоритму Евклида являются ли два числа взаимнопростыми
            /// </summary>
            /// <param name="a">Первое число</param>
            /// <param name="b">Второе число</param>
            /// <returns>Возвращает true или false в звависимости от результата</returns>
            private bool isCoprime(BigInteger a, BigInteger b)
            {
                bool result = false;
                a = reurciveCompire(a, b);
                result = (a == 1) ? true : false;
                return result;
            }

            /// <summary>
            /// Метод рекурсивного определения наибольшего общего делителя двух чисел
            /// </summary>
            /// <param name="a">Первое число</param>
            /// <param name="b">Второе число</param>
            /// <returns>Наибольший общий делитель</returns>
            private BigInteger reurciveCompire(BigInteger a, BigInteger b)
            {
                if (b == 0) return a;
                return reurciveCompire(b, a % b);
            }

            /// <summary>
            /// Возвращает случайное число e, удовлетворяющее следующему условию (e*d) mod ((p-1)*(q-1))=1
            /// </summary>            
            /// <param name="d">число d</param>
            /// <param name="p">число p</param>
            /// <param name="q">число q</param>                       
            /// <returns>Возвращает число e</returns>
            private BigInteger getNumberE(BigInteger d, BigInteger p, BigInteger q)
            {
                BigInteger n = ((p - 1) * (q - 1));
                BigInteger a = d;
                BigInteger i = n, v = 0, d1 = 1;
                while (a > 0)
                {
                    BigInteger t = i / a, x = a;
                    a = i % x;
                    i = x;
                    x = d1;
                    d1 = v - t * x;
                    v = x;
                }
                v %= n;
                if (v < 0) v = (v + n) % n;
                return v;
            }
        }         
}
