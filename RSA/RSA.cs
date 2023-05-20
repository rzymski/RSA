using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace RSANamespace
{
    public static class RSA
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
        //Funkcja zwracajaca najwiekszy wspolny dzielnik dwoch liczb
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
            BigInteger e = 3; //najniejszy możliwy wynik gdzie 1 < e < phiN i e jest zawsze nieparzyste, bo phiN jest zawsze parzyste
            while (NWD(e, phiN) != 1)
            {
                e+=2;
            }
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
