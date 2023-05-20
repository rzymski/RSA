using System.Numerics;
using System.Reflection;
using RSANamespace;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace tests
{
    public class Tests
    {
        [Test]
        public void TestEncrypt()
        {
            BigInteger n = RSA.CountN(new BigInteger(11), new BigInteger(13));
            BigInteger e = RSA.FindE(RSA.EulerFunction(new BigInteger(11), new BigInteger(13)));
            KeyValuePair<BigInteger, BigInteger> pk = new KeyValuePair<BigInteger, BigInteger>(n, e);
            Assert.AreEqual(new BigInteger(64), RSA.Encrypt(new BigInteger(25), pk));
            Assert.AreEqual(new BigInteger(42), RSA.Encrypt(new BigInteger(3), pk));
            Assert.AreEqual(new BigInteger(142), RSA.Encrypt(new BigInteger(142), pk));

            BigInteger n2 = RSA.CountN(new BigInteger(5749), new BigInteger(23509));
            BigInteger e2 = RSA.FindE(RSA.EulerFunction(new BigInteger(5749), new BigInteger(23509)));
            KeyValuePair<BigInteger, BigInteger> pk2 = new KeyValuePair<BigInteger, BigInteger>(n2, e2);
            Assert.AreEqual(new BigInteger(9765625), RSA.Encrypt(new BigInteger(25), pk2));
            Assert.AreEqual(new BigInteger(243), RSA.Encrypt(new BigInteger(3), pk2));
            Assert.AreEqual(new BigInteger(23285331), RSA.Encrypt(new BigInteger(1950), pk2));
            Assert.AreEqual(new BigInteger(38507972), RSA.Encrypt(new BigInteger(9000), pk2));
        }
        [Test]
        public void TestDecrypt()
        {
            BigInteger n = RSA.CountN(new BigInteger(11), new BigInteger(13));
            BigInteger d = RSA.InverseModulo(RSA.FindE(RSA.EulerFunction(new BigInteger(11), new BigInteger(13))), RSA.EulerFunction(new BigInteger(11), new BigInteger(13)));
            KeyValuePair<BigInteger, BigInteger> pk = new KeyValuePair<BigInteger, BigInteger>(n, d);
            Assert.AreEqual(new BigInteger(25), RSA.Decrypt(new BigInteger(64), pk));
            Assert.AreEqual(new BigInteger(3), RSA.Decrypt(new BigInteger(42), pk));
            Assert.AreEqual(new BigInteger(142), RSA.Decrypt(new BigInteger(142), pk));

            BigInteger n2 = RSA.CountN(new BigInteger(5749), new BigInteger(23509));
            BigInteger d2 = RSA.InverseModulo(RSA.FindE(RSA.EulerFunction(new BigInteger(5749), new BigInteger(23509))), RSA.EulerFunction(new BigInteger(5749), new BigInteger(23509)));
            KeyValuePair<BigInteger, BigInteger> pk2 = new KeyValuePair<BigInteger, BigInteger>(n2, d2);
            Assert.AreEqual(new BigInteger(3), RSA.Decrypt(new BigInteger(243), pk2));
            Assert.AreEqual(new BigInteger(9000), RSA.Decrypt(new BigInteger(38507972), pk2));
            Assert.AreEqual(new BigInteger(1950), RSA.Decrypt(new BigInteger(23285331), pk2));
        }
        [Test]
        public void TestExceptionMBiggerThanN()
        {
            KeyValuePair<BigInteger, BigInteger> publicKey = new KeyValuePair<BigInteger, BigInteger>(35, 5);
            KeyValuePair<BigInteger, BigInteger> privateKey = new KeyValuePair<BigInteger, BigInteger>(35, 5);
            Assert.Throws<Exception>(() => RSA.Encrypt(1000, publicKey), "Oczekiwano rzucenia wyjatku");
            Assert.Throws<Exception>(() => RSA.Decrypt(35, privateKey), "Oczekiwano rzucenia wyjatku");
        }
        [Test]
        public void TestCountN()
        {
            for (int i = 0; i < 100; i++)
            {
                for (int j = 0; j < 100; j++)
                {
                    if (MyRandom.checkIfPrime(i) && MyRandom.checkIfPrime(j))
                    {
                        BigInteger expected = i * j;
                        BigInteger result = RSA.CountN(i, j);
                        Assert.AreEqual(expected, result, $"Oczekiwano {expected}, a otrzymano {result}");
                    }
                }
            }
        }
        [Test]
        public void TestEulerFunction()
        {
            Assert.AreEqual(new BigInteger(131760000), RSA.EulerFunction(new BigInteger(5857), new BigInteger(22501)));
            Assert.AreEqual(new BigInteger(119385196), RSA.EulerFunction(new BigInteger(20147), new BigInteger(5927)));
            for (int i = 0; i < 100; i++)
            {
                for (int j = 0; j < 100; j++)
                {
                    if (MyRandom.checkIfPrime(i) && MyRandom.checkIfPrime(j))
                    {
                        BigInteger expected = (i-1) * (j-1);
                        BigInteger result = RSA.EulerFunction(i, j);
                        Assert.AreEqual(expected, result, $"Oczekiwano {expected}, a otrzymano {result}");
                    }
                }
            }
        }
        [Test]
        public void TestNWD()
        {
            Assert.AreEqual(new BigInteger(1), RSA.NWD(new BigInteger(2), new BigInteger(17)));
            Assert.AreEqual(new BigInteger(1), RSA.NWD(new BigInteger(5987), new BigInteger(23087)));
            Assert.AreEqual(new BigInteger(1000), RSA.NWD(new BigInteger(10000), new BigInteger(9000)));
            Assert.AreEqual(new BigInteger(13), RSA.NWD(new BigInteger(169), new BigInteger(13)));
        }
        [Test]
        public void TestFindE()
        {
            Assert.AreEqual(new BigInteger(7), RSA.FindE(new BigInteger(60)));
            Assert.AreEqual(new BigInteger(5), RSA.FindE(new BigInteger(192)));
            Assert.AreEqual(new BigInteger(3), RSA.FindE(new BigInteger(138192796)));
            Assert.AreEqual(new BigInteger(5), RSA.FindE(new BigInteger(152240256)));
            Assert.AreEqual(new BigInteger(7), RSA.FindE(new BigInteger(131760000)));
        }
        [Test]
        public void TestInverseModulo()
        {
            Assert.AreEqual(new BigInteger(3), RSA.InverseModulo(new BigInteger(5), new BigInteger(7)));
            Assert.AreEqual(new BigInteger(103), RSA.InverseModulo(new BigInteger(7), new BigInteger(120)));
            Assert.AreEqual(new BigInteger(77), RSA.InverseModulo(new BigInteger(5), new BigInteger(192)));
            Exception exception = Assert.Throws<Exception>(() => RSA.InverseModulo(new BigInteger(5), new BigInteger(120)));
            Assert.AreEqual($"Nie istnieje odwrotnosc modularna dla liczby 5 i modulu 120", exception.Message);
        }
        [Test]
        public void TestChangeValues()
        {
            BigInteger a = 1500100900;
            BigInteger b = 213742069;
            RSA.ChangeValues(ref a, ref b);
            Assert.AreEqual(new BigInteger(213742069), a);
            Assert.AreEqual(new BigInteger(1500100900), b);
        }

        [Test]
        public void TestChoseRandomNumberFromListe()
        {
            List<int> listWithAllPossibleValues = new List<int> { 2, 3, 5, 7, 11, 13, 17, 19 };
            Random random = new Random();
            for(int i = 0; i< 1000; i++)
            {
                int value = MyRandom.choseRandomNumberFromList(0, 20, random);
                Assert.Contains(value, listWithAllPossibleValues);
            }
        }

        [Test]
        public void TestFindPrimeFromRange() 
        {
            List<int> expected = new List<int> { 2, 3, 5, 7, 11, 13, 17, 19};
            List<int> result = MyRandom.findPrimeFromRange(0, 20);
            Assert.AreEqual(expected, result, $"Listy nie sa takie same.\nExpected = {expected}\nResult = {result}");

            List<int> expected2 = new List<int> { 10007, 10009, 10037, 10039, 10061, 10067, 10069, 10079, 10091, 10093, 10099 };
            List<int> result2 = MyRandom.findPrimeFromRange(10000, 10100);
            Assert.AreEqual(expected2, result2, $"Listy nie sa takie same.\nExpected = {expected2}\nResult = {result2}");
        }

        [Test]
        public void checkIfPrime()
        {
            Assert.IsTrue(MyRandom.checkIfPrime(2));
            Assert.IsTrue(MyRandom.checkIfPrime(3));
            Assert.IsTrue(MyRandom.checkIfPrime(5));
            Assert.IsTrue(MyRandom.checkIfPrime(7));
            Assert.IsTrue(MyRandom.checkIfPrime(97));
            Assert.IsTrue(MyRandom.checkIfPrime(23087));
            Assert.IsFalse(MyRandom.checkIfPrime(-2));
            Assert.IsFalse(MyRandom.checkIfPrime(0));
            Assert.IsFalse(MyRandom.checkIfPrime(1));
            Assert.IsFalse(MyRandom.checkIfPrime(4));
            Assert.IsFalse(MyRandom.checkIfPrime(169));
        }

    }
}