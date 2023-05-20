using System;
using System.Numerics;
using RSANamespace;

namespace tests
{
    public class EncryptionTests
    {
        [Test]
        public void TestEncryptMessage()
        {
            List<BigInteger> expectedResult = new List<BigInteger> { 182, 111, 182, 2, 25, 182, 2, 147, 27, 67, 182 };
            Assert.AreEqual(expectedResult, Encryption.EncryptMessage("ALA MA KOTA", RSA.CreatePublicKey(13, 17)));

            List<BigInteger> expectedResult2 = new List<BigInteger> { 3404, 6594, 4643, 4842, 1715, 8005, 6594, 6722, 3404, 5697, 5697, 6792 };
            Assert.AreEqual(expectedResult2, Encryption.EncryptMessage("I LOVE PIZZA", RSA.CreatePublicKey(97, 89)));

            List<BigInteger> expectedResult3 = new List<BigInteger> { 20081, 32768, 44040, 41480, 32768, 44040, 39300, 31869, 41480, 20081, 24061 };
            Assert.AreEqual(expectedResult3, Encryption.EncryptMessage("I AM ATOMIC", RSA.CreatePublicKey(431, 107)));
        }
        [Test]
        public void TestDecryptMessage()
        {
            string expectedResult = "ALA MA KOTA";
            Assert.AreEqual(expectedResult, Encryption.DecryptMessage(new List<BigInteger> { 182, 111, 182, 2, 25, 182, 2, 147, 27, 67, 182 }, RSA.CreatePrivateKey(13, 17)));

            string expectedResult2 = "I LOVE PIZZA";
            Assert.AreEqual(expectedResult2, Encryption.DecryptMessage(new List<BigInteger> { 3404, 6594, 4643, 4842, 1715, 8005, 6594, 6722, 3404, 5697, 5697, 6792 }, RSA.CreatePrivateKey(97, 89)));

            string expectedResult3 = "I AM ATOMIC";
            Assert.AreEqual(expectedResult3, Encryption.DecryptMessage(new List<BigInteger> { 20081, 32768, 44040, 41480, 32768, 44040, 39300, 31869, 41480, 20081, 24061 }, RSA.CreatePrivateKey(431, 107)));
        }

        [Test]
        public void TestEncryptAndDecryptMessage()
        {
            string expectedResult1 = "My name is Ozymandias, King of Kings; Look on my Works, ye Mighty, and despair!";
            Assert.AreEqual(expectedResult1, Encryption.DecryptMessage(Encryption.EncryptMessage(expectedResult1, RSA.CreatePublicKey(821, 1259)), RSA.CreatePrivateKey(821, 1259)));

            string expectedResult2 = "Run? Who's running? Running where? And WHY?!!!";
            Assert.AreEqual(expectedResult2, Encryption.DecryptMessage(Encryption.EncryptMessage(expectedResult2, RSA.CreatePublicKey(139, 607)), RSA.CreatePrivateKey(139, 607)));

            string expectedResult3 = "Justice will triumph, you say? Of course it will! Because the winners will become justice!";
            Assert.AreEqual(expectedResult3, Encryption.DecryptMessage(Encryption.EncryptMessage(expectedResult3, RSA.CreatePublicKey(503, 449)), RSA.CreatePrivateKey(503, 449)));
        }
    }

    public class RSATests
    {
        [Test]
        public void TestCreatePublicKey()
        {
            KeyValuePair<BigInteger, BigInteger> expectedPK1 = new KeyValuePair<BigInteger, BigInteger>(135153241, 5);
            Assert.AreEqual(expectedPK1, RSA.CreatePublicKey(new BigInteger(5749), new BigInteger(23509)));
            KeyValuePair<BigInteger, BigInteger> expectedPK2 = new KeyValuePair<BigInteger, BigInteger>(143, 7);
            Assert.AreEqual(expectedPK2, RSA.CreatePublicKey(new BigInteger(11), new BigInteger(13)));
            KeyValuePair<BigInteger, BigInteger> expectedPK3 = new KeyValuePair<BigInteger, BigInteger>(391, 3);
            Assert.AreEqual(expectedPK3, RSA.CreatePublicKey(new BigInteger(23), new BigInteger(17)));
        }
        [Test]
        public void TestCreatePrivateKey()
        {
            KeyValuePair<BigInteger, BigInteger> expectedPK1 = new KeyValuePair<BigInteger, BigInteger>(135153241, 27024797);
            Assert.AreEqual(expectedPK1, RSA.CreatePrivateKey(new BigInteger(5749), new BigInteger(23509)));
            KeyValuePair<BigInteger, BigInteger> expectedPK2 = new KeyValuePair<BigInteger, BigInteger>(143, 103);
            Assert.AreEqual(expectedPK2, RSA.CreatePrivateKey(new BigInteger(11), new BigInteger(13)));
            KeyValuePair<BigInteger, BigInteger> expectedPK3 = new KeyValuePair<BigInteger, BigInteger>(391, 235);
            Assert.AreEqual(expectedPK3, RSA.CreatePrivateKey(new BigInteger(23), new BigInteger(17)));
        }
        [Test]
        public void TestExceptionPorQIsNotPrimeInCreatingKey()
        {
            Exception exceptionPublicKey = Assert.Throws<Exception>(() => RSA.CreatePublicKey(new BigInteger(47804), new BigInteger(36979)));
            Assert.AreEqual("P lub Q nie jest liczba pierwsza. Blad tworzenia klucza publicznego", exceptionPublicKey.Message);
            Assert.Throws<Exception>(() => RSA.CreatePublicKey(new BigInteger(47942), new BigInteger(34408)));
            Assert.Throws<Exception>(() => RSA.CreatePublicKey(new BigInteger(18181), new BigInteger(17552)));

            Exception exceptionPrivateKey = Assert.Throws<Exception>(() => RSA.CreatePrivateKey(new BigInteger(47804), new BigInteger(36979)));
            Assert.AreEqual("P lub Q nie jest liczba pierwsza. Blad tworzenia klucza prywatnego", exceptionPrivateKey.Message);
            Assert.Throws<Exception>(() => RSA.CreatePrivateKey(new BigInteger(47942), new BigInteger(34408)));
            Assert.Throws<Exception>(() => RSA.CreatePrivateKey(new BigInteger(18181), new BigInteger(17552)));
        }
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
        public void TestNWDExceptionNotPositiveValues()
        {
            Exception exception = Assert.Throws<Exception>(() => RSA.NWD(-2, 10));
            Assert.AreEqual("Nie mozna obliczyc NWD jesli jakas wartosc nie jest dodatnia", exception.Message);
            Exception exception2 = Assert.Throws<Exception>(() => RSA.NWD(10, -1939));
            Assert.AreEqual("Nie mozna obliczyc NWD jesli jakas wartosc nie jest dodatnia", exception2.Message);
            Exception exception3 = Assert.Throws<Exception>(() => RSA.NWD(5, 0));
            Assert.AreEqual("Nie mozna obliczyc NWD jesli jakas wartosc nie jest dodatnia", exception3.Message);
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
            Assert.AreEqual("Nie istnieje odwrotnosc modularna dla liczby 5 i modulu 120", exception.Message);
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
    }

    public class RandomTests
    {
        [Test]
        public void TestChoseRandomNumberFromListe()
        {
            List<int> listWithAllPossibleValues = new List<int> { 2, 3, 5, 7, 11, 13, 17, 19 };
            Random random = new Random();
            for (int i = 0; i < 1000; i++)
            {
                int value = MyRandom.choseRandomNumberFromList(0, 20, random);
                Assert.Contains(value, listWithAllPossibleValues);
            }
        }
        [Test]
        public void TestFindPrimeFromRange()
        {
            List<int> expected = new List<int> { 2, 3, 5, 7, 11, 13, 17, 19 };
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