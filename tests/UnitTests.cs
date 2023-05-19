using System.Numerics;
using System.Security.Cryptography.X509Certificates;

namespace tests
{
    public static class MainFunctions
    {
        //funkcja szyfrujaca wzor c =  (m^e) mod n
        public static BigInteger Encrypt(BigInteger m, KeyValuePair<BigInteger, BigInteger> publicKey)
        {
            BigInteger n = publicKey.Key;
            BigInteger e = publicKey.Value;
            if (m >= n) throw new Exception("Nie dozwolona operacja m nie moze byc wieksze od n"); //zalozenie m < n
            BigInteger result = 1;
            for (int i = 0; i < e; i++)
            {
                result *= m; //Zeby uniknac wielkich liczb naprzemian potegowanie i modulo
                result %= n;
            }
            return result;
        }
        //funkcja odszyfrujaca wzor m =  (c^d) mod n
        public static BigInteger Decrypt(BigInteger c, KeyValuePair<BigInteger, BigInteger> privateKey)
        {
            BigInteger n = privateKey.Key;
            BigInteger d = privateKey.Value;
            if (c >= n) throw new Exception("Nie dozwolona operacja c nie moze byc wieksze od n"); //zalozenie m < n
            BigInteger result = 1;
            for (int i = 0; i < d; i++)
            {
                result *= c; //Zeby uniknac wielkich liczb naprzemian potegowanie i modulo
                result %= n;
            }
            return result;
        }
        //Funkcja zwraca n = p * q
        public static BigInteger CountN(BigInteger p, BigInteger q)
        {
            return p * q;
        }
        //Funkcja zwraca phiN = (p - 1) * (q - 1)
        public static BigInteger EulerFunction(BigInteger p, BigInteger q)
        {
            return (p - 1) * (q - 1);
        }
        //Funkcja zwracajaca najmniejszy wspolny dzielnik dwoch liczb
        public static BigInteger NWD(BigInteger a, BigInteger b)
        {
            while (b != 0)
            {
                BigInteger c = a % b;
                a = b;
                b = c;
                //Console.WriteLine($"A = {a}  B = {b}  C = {c}");
            }
            return a;
        }
        // Funkcja znajdujaca najmniejsze pasujace e
        public static BigInteger FindE(BigInteger phiN) //phiN to wynik EulerFunction (p-1)*(q-1)
        {
            BigInteger e = 2; //najniejszy mo¿liwy wynik gdzie 1 < e < phiN
            while (NWD(e, phiN) != 1)
            {
                e++;
            }
            return e;
        }
        //Funkcja zwracaj¹ca odwrotnoœæ modularn¹
        public static BigInteger InverseModulo(BigInteger number, BigInteger modulo)
        {
            BigInteger u = 1, w = number, x = 0, z = modulo;
            while (w != 0)
            {
                if (w < z)
                {
                    ChangeValues(ref u, ref x);
                    ChangeValues(ref w, ref z);
                }
                BigInteger q = w / z;
                u -= q * x;
                w -= q * z;
            }
            //teoretycznie nie powinien nigdy wystapic ten wyjatek, bo liczby 'number' i 'modulo' sa wzglednie pierwsze wedlug zalozenia i zawsze istnieje odwrotnosc modularna
            if (z != 1)
                throw new Exception($"Nie istnieje odwrotnosc modularna dla liczby {number} i modulu {modulo}");
            if (x < 0)
                x += modulo;
            return x;
        }
        //Funkcja zamieniajaca wartosci dwoch zmiennych
        public static void ChangeValues(ref BigInteger a, ref BigInteger b)
        {
            BigInteger temp = a;
            a = b;
            b = temp;
        }
        //funkcja wybierajaca losowa liczbe z listy liczb pierwszych ktore znajduja sie w zakresie pomiedzy min i max
        public static int choseRandomNumberFromList(int minNumber, int maxNumber, Random random)
        {
            List<int> list = new List<int>();
            list = findPrimeFromRange(minNumber, maxNumber);
            int randomIndex = random.Next(0, list.Count);
            return list[randomIndex];
        }
        //funkcja znajdujaca liczby pierwsze z danego zakresu
        public static List<int> findPrimeFromRange(int minNumber, int maxNumber)
        {
            List<int> result = new List<int>();
            for (int i = minNumber; i < maxNumber; i++)
                if (checkIfPrime(i))
                    result.Add(i);
            return result;
        }
        //funkcja sprawdzajaca czy dana liczba jest pierwsza
        public static bool checkIfPrime(int number)
        {
            if (number < 2)
                return false;
            if (number == 2 || number == 3)
                return true;
            if (number % 2 == 0)
                return false;
            int sqrt = (int)Math.Sqrt(number);
            for (int i = 3; i <= sqrt; i += 2)
                if (number % i == 0)
                    return false;
            return true;
        }
    }

    public class Tests
    {
        [SetUp]
        public void Setup()
        {
            
        }

        [Test]
        public void TestExceptionMBiggerThanN() 
        {
            KeyValuePair<BigInteger, BigInteger> publicKey = new KeyValuePair<BigInteger, BigInteger>(35, 5);
            KeyValuePair<BigInteger, BigInteger> privateKey = new KeyValuePair<BigInteger, BigInteger>(35, 5);
            Assert.Throws<Exception>(()  => MainFunctions.Encrypt(1000, publicKey), "Oczekiwano rzucenia wyjatku");
            Assert.Throws<Exception>(() => MainFunctions.Decrypt(35, privateKey), "Oczekiwano rzucenia wyjatku");
        }

        [Test]
        public void TestCountN()
        {
            for(int i = 0; i < 1000; i++)
            {
                for(int j = 0; j < 1000; j++)
                {
                    if(MainFunctions.checkIfPrime(i) && MainFunctions.checkIfPrime(j))
                    {
                        BigInteger expected = i * j;
                        BigInteger result = MainFunctions.CountN(i, j);
                        Assert.AreEqual(expected, result, $"Oczekiwano {expected}, a otrzymano {result}");
                    }
                }
            }
        }
    }
}