// p i q powinny być losowymi liczbami pierwszymi
using System.Numerics;

BigInteger p = 11;//97;
BigInteger q = 7;// 89;

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
BigInteger m = 89;
//zaszyfrowanie wiadomosci
BigInteger c = pow(m, e) % n;
Console.WriteLine("C = " + c);





//odszyfrowanie wiadomosci
BigInteger m2 = pow(c, d) % n;
Console.WriteLine("M =" + m2);

Console.WriteLine("\n" + pow(c, d));

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
//funkcja zwracająca potęge liczby o danej podstawie i wykladniku
BigInteger pow(BigInteger baseNumber, BigInteger power)
{
    BigInteger result = 1;
    for (int i = 0; i < power; i++)
        result *= baseNumber;
    return result;
}