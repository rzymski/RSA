// p i q powinny być losowymi liczbami pierwszymi
using System.Numerics;

BigInteger p = 97;
BigInteger q = 79;


BigInteger n = CountN(p, q);
BigInteger phiN = EulerFunction(p, q);
BigInteger e = FindE(phiN);
BigInteger d = InverseModulo(e, phiN);

Console.WriteLine("N =" + n);
Console.WriteLine("Funkcja Eulera phi(n) = " + phiN);
Console.WriteLine("E = " + e);
Console.WriteLine("D = " + d);

KeyValuePair<BigInteger, BigInteger> publicKey = new KeyValuePair<BigInteger, BigInteger>(n, e);
KeyValuePair<BigInteger, BigInteger> privateKey = new KeyValuePair<BigInteger, BigInteger>(n, d);

//m to blok wiadomosci i nie moze byc wiekszy niz n 
BigInteger m = 5000;
//zaszyfrowanie wiadomosci
BigInteger c = Encrypt(m, publicKey);
Console.WriteLine("C = " + c);
//odszyfrowanie wiadomosci
BigInteger m2 = Decrypt(c, privateKey);
Console.WriteLine("M =" + m2);

//funkcja szyfrujaca wzor c =  (m^e) mod n
BigInteger Encrypt(BigInteger m, KeyValuePair<BigInteger, BigInteger> publicK)
{
    BigInteger n = publicK.Key;
    BigInteger e = publicK.Value;
    if (m > n) throw new Exception("Nie dozwolona operacja m nie moze byc wieksze od n"); //zalozenie m < n
    BigInteger result = 1;
    for (int i = 0; i < e; i++)
    {
        result *= m; //Zeby uniknac wielkich liczb naprzemian potegowanie i modulo
        result %= n;
    }
    return result;
}
//funkcja odszyfrujaca wzor m =  (c^d) mod n
BigInteger Decrypt(BigInteger c, KeyValuePair<BigInteger, BigInteger> privateK)
{
    BigInteger n = privateK.Key;
    BigInteger d = privateK.Value;
    if (c > n) throw new Exception("Nie dozwolona operacja c nie moze byc wieksze od n"); //zalozenie m < n
    BigInteger result = 1;
    for(int i = 0; i < d; i++) 
    {
        result *= c; //Zeby uniknac wielkich liczb naprzemian potegowanie i modulo
        result %= n;
    }
    return result;
}
//Funkcja zwraca n = p * q
BigInteger CountN(BigInteger p, BigInteger q)
{
    return p * q;
}
//Funkcja zwraca phiN = (p - 1) * (q - 1)
BigInteger EulerFunction(BigInteger p, BigInteger q)
{
    return (p - 1) * (q - 1);
}
//Funkcja zwracajaca najmniejszy wspolny dzielnik dwoch liczb
BigInteger NWD(BigInteger a, BigInteger b)
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
BigInteger FindE(BigInteger phiN) //phiN to wynik EulerFunction (p-1)*(q-1)
{
    BigInteger e = 2; //najniejszy możliwy wynik gdzie 1 < e < phiN
    while (NWD(e, phiN) != 1)
    {
        e++;
    }
    return e;
}
//Funkcja zwracająca odwrotność modularną
BigInteger InverseModulo(BigInteger number, BigInteger modulo)
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
void ChangeValues(ref BigInteger a, ref BigInteger b)
{
    BigInteger temp = a;
    a = b;
    b = temp;
}