using RSANamespace;
using System.Numerics;
#pragma warning disable IDE0090

namespace tests
{
    public class EncryptionTests
    {
        [Test]
        public void TestEncryptMessage()
        {
            List<BigInteger> expectedResult = new List<BigInteger> { 182, 111, 182, 2, 25, 182, 2, 147, 27, 67, 182 };
            Assert.That(Encryption.EncryptMessage("ALA MA KOTA", RSA.CreatePublicKey(13, 17)), Is.EqualTo(expectedResult));

            List<BigInteger> expectedResult2 = new List<BigInteger> { 3404, 6594, 4643, 4842, 1715, 8005, 6594, 6722, 3404, 5697, 5697, 6792 };
            Assert.That(Encryption.EncryptMessage("I LOVE PIZZA", RSA.CreatePublicKey(97, 89)), Is.EqualTo(expectedResult2));

            List<BigInteger> expectedResult3 = new List<BigInteger> { 20081, 32768, 44040, 41480, 32768, 44040, 39300, 31869, 41480, 20081, 24061 };
            Assert.That(Encryption.EncryptMessage("I AM ATOMIC", RSA.CreatePublicKey(431, 107)), Is.EqualTo(expectedResult3));
        }
        [Test]
        public void TestDecryptMessage()
        {
            string expectedResult = "ALA MA KOTA";
            Assert.That(Encryption.DecryptMessage(new List<BigInteger> { 182, 111, 182, 2, 25, 182, 2, 147, 27, 67, 182 }, RSA.CreatePrivateKey(13, 17)), Is.EqualTo(expectedResult));

            string expectedResult2 = "I LOVE PIZZA";
            Assert.That(Encryption.DecryptMessage(new List<BigInteger> { 3404, 6594, 4643, 4842, 1715, 8005, 6594, 6722, 3404, 5697, 5697, 6792 }, RSA.CreatePrivateKey(97, 89)), Is.EqualTo(expectedResult2));

            string expectedResult3 = "I AM ATOMIC";
            Assert.That(Encryption.DecryptMessage(new List<BigInteger> { 20081, 32768, 44040, 41480, 32768, 44040, 39300, 31869, 41480, 20081, 24061 }, RSA.CreatePrivateKey(431, 107)), Is.EqualTo(expectedResult3));
        }
        [Test]
        public void TestEncryptAndDecryptMessage()
        {
            string expectedResult1 = "My name is Ozymandias, King of Kings; Look on my Works, ye Mighty, and despair!";
            Assert.That(Encryption.DecryptMessage(Encryption.EncryptMessage(expectedResult1, RSA.CreatePublicKey(821, 1259)), RSA.CreatePrivateKey(821, 1259)), Is.EqualTo(expectedResult1));

            string expectedResult2 = "Run? Who's running? Running where? And WHY?!!!";
            Assert.That(Encryption.DecryptMessage(Encryption.EncryptMessage(expectedResult2, RSA.CreatePublicKey(139, 607)), RSA.CreatePrivateKey(139, 607)), Is.EqualTo(expectedResult2));

            string expectedResult3 = "Justice will triumph, you say? Of course it will! Because the winners will become justice!";
            Assert.That(Encryption.DecryptMessage(Encryption.EncryptMessage(expectedResult3, RSA.CreatePublicKey(503, 449)), RSA.CreatePrivateKey(503, 449)), Is.EqualTo(expectedResult3));
        }
    }

    public class RSATests
    {
        private static IEnumerable<TestCaseData> GetTestCases()
        {
            yield return new TestCaseData(new BigInteger(11), new BigInteger(7));
            yield return new TestCaseData(new BigInteger(13), new BigInteger(17));  // Przyk³adowe p i q
            yield return new TestCaseData(new BigInteger(97), new BigInteger(89));
            yield return new TestCaseData(new BigInteger(431), new BigInteger(107));
            yield return new TestCaseData(new BigInteger(199), new BigInteger(251));
        }
        [Test]
        [TestCaseSource(nameof(GetTestCases))]
        public void TestChangeValues(BigInteger p, BigInteger q)
        {
            BigInteger a = p;
            BigInteger b = q;
            RSA.ChangeValues(ref a, ref b);
            Assert.Multiple(() =>
            {
                Assert.That(a, Is.EqualTo(q));
                Assert.That(b, Is.EqualTo(p));
            });
        }

        [Test]
        public void TestCreatePublicKey()
        {
            KeyValuePair<BigInteger, BigInteger> expectedPK1 = new KeyValuePair<BigInteger, BigInteger>(135153241, 5);
            Assert.That(RSA.CreatePublicKey(new BigInteger(5749), new BigInteger(23509)), Is.EqualTo(expectedPK1));
            KeyValuePair<BigInteger, BigInteger> expectedPK2 = new KeyValuePair<BigInteger, BigInteger>(143, 7);
            Assert.That(RSA.CreatePublicKey(new BigInteger(11), new BigInteger(13)), Is.EqualTo(expectedPK2));
            KeyValuePair<BigInteger, BigInteger> expectedPK3 = new KeyValuePair<BigInteger, BigInteger>(391, 3);
            Assert.That(RSA.CreatePublicKey(new BigInteger(23), new BigInteger(17)), Is.EqualTo(expectedPK3));
        }
        [Test]
        public void TestCreatePrivateKey()
        {
            KeyValuePair<BigInteger, BigInteger> expectedPK1 = new KeyValuePair<BigInteger, BigInteger>(135153241, 27024797);
            Assert.That(RSA.CreatePrivateKey(new BigInteger(5749), new BigInteger(23509)), Is.EqualTo(expectedPK1));
            KeyValuePair<BigInteger, BigInteger> expectedPK2 = new KeyValuePair<BigInteger, BigInteger>(143, 103);
            Assert.That(RSA.CreatePrivateKey(new BigInteger(11), new BigInteger(13)), Is.EqualTo(expectedPK2));
            KeyValuePair<BigInteger, BigInteger> expectedPK3 = new KeyValuePair<BigInteger, BigInteger>(391, 235);
            Assert.That(RSA.CreatePrivateKey(new BigInteger(23), new BigInteger(17)), Is.EqualTo(expectedPK3));
        }
        [Test]
        public void TestExceptionPorQIsNotPrimeInCreatingKey()
        {
            Exception exceptionPublicKey = Assert.Throws<Exception>(() => RSA.CreatePublicKey(new BigInteger(47804), new BigInteger(36979)));
            Assert.That(exceptionPublicKey.Message, Is.EqualTo("P lub Q nie jest liczba pierwsza. Blad tworzenia klucza publicznego"));
            Assert.Throws<Exception>(() => RSA.CreatePublicKey(new BigInteger(47942), new BigInteger(34408)));
            Assert.Throws<Exception>(() => RSA.CreatePublicKey(new BigInteger(18181), new BigInteger(17552)));

            Exception exceptionPrivateKey = Assert.Throws<Exception>(() => RSA.CreatePrivateKey(new BigInteger(47804), new BigInteger(36979)));
            Assert.That(exceptionPrivateKey.Message, Is.EqualTo("P lub Q nie jest liczba pierwsza. Blad tworzenia klucza prywatnego"));
            Assert.Throws<Exception>(() => RSA.CreatePrivateKey(new BigInteger(47942), new BigInteger(34408)));
            Assert.Throws<Exception>(() => RSA.CreatePrivateKey(new BigInteger(18181), new BigInteger(17552)));
        }
        [Test]
        public void TestEncrypt()
        {
            BigInteger n = RSA.CountN(new BigInteger(11), new BigInteger(13));
            BigInteger e = RSA.FindE(RSA.EulerFunction(new BigInteger(11), new BigInteger(13)));
            KeyValuePair<BigInteger, BigInteger> pk = new KeyValuePair<BigInteger, BigInteger>(n, e);
            Assert.Multiple(() =>
            {
                Assert.That(RSA.Encrypt(new BigInteger(25), pk), Is.EqualTo(new BigInteger(64)));
                Assert.That(RSA.Encrypt(new BigInteger(3), pk), Is.EqualTo(new BigInteger(42)));
                Assert.That(RSA.Encrypt(new BigInteger(142), pk), Is.EqualTo(new BigInteger(142)));
            });
            BigInteger n2 = RSA.CountN(new BigInteger(5749), new BigInteger(23509));
            BigInteger e2 = RSA.FindE(RSA.EulerFunction(new BigInteger(5749), new BigInteger(23509)));
            KeyValuePair<BigInteger, BigInteger> pk2 = new KeyValuePair<BigInteger, BigInteger>(n2, e2);
            Assert.Multiple(() =>
            {
                Assert.That(RSA.Encrypt(new BigInteger(25), pk2), Is.EqualTo(new BigInteger(9765625)));
                Assert.That(RSA.Encrypt(new BigInteger(3), pk2), Is.EqualTo(new BigInteger(243)));
                Assert.That(RSA.Encrypt(new BigInteger(1950), pk2), Is.EqualTo(new BigInteger(23285331)));
                Assert.That(RSA.Encrypt(new BigInteger(9000), pk2), Is.EqualTo(new BigInteger(38507972)));
            });
        }

        [Test]
        public void TestDecrypt()
        {
            BigInteger n = RSA.CountN(new BigInteger(11), new BigInteger(13));
            BigInteger d = RSA.InverseModulo(RSA.FindE(RSA.EulerFunction(new BigInteger(11), new BigInteger(13))), RSA.EulerFunction(new BigInteger(11), new BigInteger(13)));
            KeyValuePair<BigInteger, BigInteger> pk = new KeyValuePair<BigInteger, BigInteger>(n, d);
            Assert.Multiple(() =>
            {
                Assert.That(RSA.Decrypt(new BigInteger(64), pk), Is.EqualTo(new BigInteger(25)));
                Assert.That(RSA.Decrypt(new BigInteger(42), pk), Is.EqualTo(new BigInteger(3)));
                Assert.That(RSA.Decrypt(new BigInteger(142), pk), Is.EqualTo(new BigInteger(142)));
            });
            BigInteger n2 = RSA.CountN(new BigInteger(5749), new BigInteger(23509));
            BigInteger d2 = RSA.InverseModulo(RSA.FindE(RSA.EulerFunction(new BigInteger(5749), new BigInteger(23509))), RSA.EulerFunction(new BigInteger(5749), new BigInteger(23509)));
            KeyValuePair<BigInteger, BigInteger> pk2 = new KeyValuePair<BigInteger, BigInteger>(n2, d2);
            Assert.Multiple(() =>
            {
                Assert.That(RSA.Decrypt(new BigInteger(243), pk2), Is.EqualTo(new BigInteger(3)));
                Assert.That(RSA.Decrypt(new BigInteger(38507972), pk2), Is.EqualTo(new BigInteger(9000)));
                Assert.That(RSA.Decrypt(new BigInteger(23285331), pk2), Is.EqualTo(new BigInteger(1950)));
            });
        }

        [Test]
        public void TestExceptionMBiggerThanN()
        {
            KeyValuePair<BigInteger, BigInteger> publicKey = new KeyValuePair<BigInteger, BigInteger>(35, 5);
            KeyValuePair<BigInteger, BigInteger> privateKey = new KeyValuePair<BigInteger, BigInteger>(35, 5);
            Assert.Throws<Exception>(() => RSA.Encrypt(1000, publicKey), "Oczekiwano rzucenia wyjatku");
            Assert.Throws<Exception>(() => RSA.Decrypt(35, privateKey), "Oczekiwano rzucenia wyjatku");
        }
        [Test, Order(1)]
        public void TestCountN()
        {
            for (int i = 0; i < 100; i++)
            {
                for (int j = 0; j < 100; j++)
                {
                    if (MyRandom.CheckIfPrime(i) && MyRandom.CheckIfPrime(j))
                    {
                        BigInteger expected = i * j;
                        BigInteger result = RSA.CountN(i, j);
                        Assert.That(result, Is.EqualTo(expected), $"Oczekiwano {expected}, a otrzymano {result}");
                    }
                }
            }
        }
        [Test, Order(2)]
        public void TestEulerFunction()
        {
            Assert.That(RSA.EulerFunction(new BigInteger(5857), new BigInteger(22501)), Is.EqualTo(new BigInteger(131760000)));
            Assert.That(RSA.EulerFunction(new BigInteger(20147), new BigInteger(5927)), Is.EqualTo(new BigInteger(119385196)));
            for (int i = 0; i < 100; i++)
            {
                for (int j = 0; j < 100; j++)
                {
                    if (MyRandom.CheckIfPrime(i) && MyRandom.CheckIfPrime(j))
                    {
                        BigInteger expected = (i - 1) * (j - 1);
                        BigInteger result = RSA.EulerFunction(i, j);
                        Assert.That(result, Is.EqualTo(expected), $"Oczekiwano {expected}, a otrzymano {result}");
                    }
                }
            }
        }
        [Test, Order(4)]
        public void TestNWD()
        {
            Assert.That(RSA.NWD(new BigInteger(2), new BigInteger(17)), Is.EqualTo(new BigInteger(1)));
            Assert.That(RSA.NWD(new BigInteger(5987), new BigInteger(23087)), Is.EqualTo(new BigInteger(1)));
            Assert.That(RSA.NWD(new BigInteger(10000), new BigInteger(9000)), Is.EqualTo(new BigInteger(1000)));
            Assert.That(RSA.NWD(new BigInteger(169), new BigInteger(13)), Is.EqualTo(new BigInteger(13)));
        }
        [Test, Order(3)]
        public void TestNWDExceptionNotPositiveValues()
        {
            Exception exception = Assert.Throws<Exception>(() => RSA.NWD(-2, 10));
            Assert.That(exception.Message, Is.EqualTo("Nie mozna obliczyc NWD jesli jakas wartosc nie jest dodatnia"));
            Exception exception2 = Assert.Throws<Exception>(() => RSA.NWD(10, -1939));
            Assert.That(exception2.Message, Is.EqualTo("Nie mozna obliczyc NWD jesli jakas wartosc nie jest dodatnia"));
            Exception exception3 = Assert.Throws<Exception>(() => RSA.NWD(5, 0));
            Assert.That(exception3.Message, Is.EqualTo("Nie mozna obliczyc NWD jesli jakas wartosc nie jest dodatnia"));
        }
        [Test, Order(5)]
        public void TestFindE()
        {
            Assert.That(RSA.FindE(new BigInteger(60)), Is.EqualTo(new BigInteger(7)));
            Assert.That(RSA.FindE(new BigInteger(192)), Is.EqualTo(new BigInteger(5)));
            Assert.That(RSA.FindE(new BigInteger(138192796)), Is.EqualTo(new BigInteger(3)));
            Assert.That(RSA.FindE(new BigInteger(152240256)), Is.EqualTo(new BigInteger(5)));
            Assert.That(RSA.FindE(new BigInteger(131760000)), Is.EqualTo(new BigInteger(7)));
        }
        [Test, Order(6)]
        public void TestInverseModulo()
        {
            Assert.That(RSA.InverseModulo(new BigInteger(5), new BigInteger(7)), Is.EqualTo(new BigInteger(3)));
            Assert.That(RSA.InverseModulo(new BigInteger(7), new BigInteger(120)), Is.EqualTo(new BigInteger(103)));
            Assert.That(RSA.InverseModulo(new BigInteger(5), new BigInteger(192)), Is.EqualTo(new BigInteger(77)));
            Exception exception = Assert.Throws<Exception>(() => RSA.InverseModulo(new BigInteger(5), new BigInteger(120)));
            Assert.That(exception.Message, Is.EqualTo("Nie istnieje odwrotnosc modularna dla liczby 5 i modulu 120"));
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
                int value = MyRandom.ChoseRandomNumberFromList(0, 20, random);
                Assert.Contains(value, listWithAllPossibleValues);
            }
        }
        [Test]
        public void TestFindPrimeFromRange()
        {
            List<int> expected = new List<int> { 2, 3, 5, 7, 11, 13, 17, 19 };
            List<int> result = MyRandom.FindPrimeFromRange(0, 20);
            Assert.That(result, Is.EqualTo(expected), $"Listy nie sa takie same.\nExpected = {expected}\nResult = {result}");

            List<int> expected2 = new List<int> { 10007, 10009, 10037, 10039, 10061, 10067, 10069, 10079, 10091, 10093, 10099 };
            List<int> result2 = MyRandom.FindPrimeFromRange(10000, 10100);
            Assert.That(result2, Is.EqualTo(expected2), $"Listy nie sa takie same.\nExpected = {expected2}\nResult = {result2}");
        }
        [Test]
        public void TestCheckIfPrime()
        {
            Assert.That(MyRandom.CheckIfPrime(2), Is.True);
            Assert.That(MyRandom.CheckIfPrime(3), Is.True);
            Assert.That(MyRandom.CheckIfPrime(5), Is.True);
            Assert.That(MyRandom.CheckIfPrime(7), Is.True);
            Assert.That(MyRandom.CheckIfPrime(97), Is.True);
            Assert.That(MyRandom.CheckIfPrime(23087), Is.True);
            Assert.That(MyRandom.CheckIfPrime(-2), Is.False);
            Assert.That(MyRandom.CheckIfPrime(0), Is.False);
            Assert.That(MyRandom.CheckIfPrime(1), Is.False);
            Assert.That(MyRandom.CheckIfPrime(4), Is.False);
            Assert.That(MyRandom.CheckIfPrime(169), Is.False);
        }
    }
}