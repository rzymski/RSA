using System.Numerics;

namespace RSANamespace
{
    public static class Encryption
    {
        public static List<BigInteger> EncryptMessage(string message, KeyValuePair<BigInteger, BigInteger> publicKey)
        {
            List<BigInteger> result = new List<BigInteger>();
            for(int i = 0; i < message.Length; i++)
                result.Add(RSA.Encrypt((int)message[i], publicKey));
            return result;
        }

        public static string DecryptMessage(List<BigInteger> message, KeyValuePair<BigInteger, BigInteger> privateKey)
        {
            string result = "";
            for(int i=0; i<message.Count; i++)
            {
                BigInteger asciiResult = RSA.Decrypt(message[i], privateKey);
                if (asciiResult > 255) throw new Exception($"Kod ascii przekroczyl wartosc 255 przy odszyfrowywaniu i wynosi {asciiResult}");
                result += Convert.ToChar((int)asciiResult);
            }
            return result;
        }
    }

    public static class RSA
    {
        //funkcja tworzaca publicznt klucz na podstawie p i q
        public static KeyValuePair<BigInteger, BigInteger> CreatePublicKey(BigInteger p, BigInteger q)
        {
            //sprawdzenie czy p i q sa pierwsze
            if (!MyRandom.checkIfPrime((int)p) || !MyRandom.checkIfPrime((int)q)) throw new Exception("P lub Q nie jest liczba pierwsza. Blad tworzenia klucza publicznego");
            BigInteger n = RSA.CountN(p, q);
            BigInteger phiN = RSA.EulerFunction(p, q);
            BigInteger e = RSA.FindE(phiN);
            KeyValuePair<BigInteger, BigInteger> publicKey = new KeyValuePair<BigInteger, BigInteger>(n, e);
            return publicKey;
        }
        //funkcja tworzaca prywatny klucz na podstawie p i q
        public static KeyValuePair<BigInteger, BigInteger> CreatePrivateKey(BigInteger p, BigInteger q)
        {
            if (!MyRandom.checkIfPrime((int)p) || !MyRandom.checkIfPrime((int)q)) throw new Exception("P lub Q nie jest liczba pierwsza. Blad tworzenia klucza prywatnego");
            BigInteger n = RSA.CountN(p, q);
            BigInteger phiN = RSA.EulerFunction(p, q);
            BigInteger e = RSA.FindE(phiN);
            BigInteger d = RSA.InverseModulo(e, phiN);
            KeyValuePair<BigInteger, BigInteger> privateKey = new KeyValuePair<BigInteger, BigInteger>(n, d);
            return privateKey;
        }
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
        //Funkcja zwracajaca najwiekszy wspolny dzielnik dwoch liczb
        public static BigInteger NWD(BigInteger a, BigInteger b)
        {
            if(a <= 0 ||  b <= 0) throw new Exception("Nie mozna obliczyc NWD jesli jakas wartosc nie jest dodatnia");
            while (b != 0)
            {
                BigInteger c = a % b;
                a = b;
                b = c;
            }
            return a;
        }
        // Funkcja znajdujaca najmniejsze pasujace e
        public static BigInteger FindE(BigInteger phiN) //phiN to wynik EulerFunction (p-1)*(q-1)
        {
            if(phiN < 4) throw new Exception("PhiN jest za male nie mozliwe jest znalezienie e");
            BigInteger e = 3; //najniejszy możliwy wynik gdzie 1 < e < phiN i e jest zawsze nieparzyste, bo phiN jest zawsze parzyste
            while (NWD(e, phiN) != 1)
                e+=2;
            if (e > phiN) throw new Exception("Nie znaleziono odpowieniego e");
            return e;
        }
        //Funkcja zwracająca odwrotność modularną
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
    }

    public static class MyRandom
    {
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
}
