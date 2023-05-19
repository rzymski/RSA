// p i q powinny być losowymi liczbami pierwszymi
long p = 7;//97;
long q = 11;// 89;

long n = CountN(p, q);
long phiN = EulerFunction(p, q);
long e = FindE(phiN);
long d = InverseModulo(e, phiN);

Console.WriteLine("N =" + n);
Console.WriteLine("Funkcja Eulera phi(n) = " + phiN);
Console.WriteLine("E = " + e);
Console.WriteLine("D = " + d);

KeyValuePair<long, long> publicKey = new KeyValuePair<long, long>(n, e);
KeyValuePair<long, long> privateKey = new KeyValuePair<long, long>(n, d);

//m to blok wiadomosci i nie moze byc wiekszy niz n 
long m = 89;
//zaszyfrowanie wiadomosci
long c = pow(m, e) % n;
Console.WriteLine("C = " + c);
//odszyfrowanie wiadomosci
long m2 = pow(c, d) % n;
Console.WriteLine("M =" + m2);

//Funkcja zwraca n = p * q
long CountN(long p, long q)
{
    return p * q;
}
//Funkcja zwraca phiN = (p - 1) * (q - 1)
long EulerFunction(long p, long q)
{
    return (p - 1) * (q - 1);
}
//Funkcja zwracajaca najmniejszy wspolny dzielnik dwoch liczb
long NWD(long a, long b)
{
    while (b != 0)
    {
        long c = a % b;
        a = b;
        b = c;
        //Console.WriteLine($"A = {a}  B = {b}  C = {c}");
    }
    return a;
}
// Funkcja znajdujaca najmniejsze pasujace e
long FindE(long phiN) //phiN to wynik EulerFunction (p-1)*(q-1)
{
    long e = 2; //najniejszy możliwy wynik gdzie 1 < e < phiN
    while (NWD(e, phiN) != 1)
    {
        e++;
    }
    return e;
}
//Funkcja zwracająca odwrotność modularną
long InverseModulo(long number, long modulo)
{
    long u = 1, w = number, x = 0, z = modulo;
    while (w != 0)
    {
        if (w < z)
        {
            ChangeValues(ref u, ref x);
            ChangeValues(ref w, ref z);
        }
        long q = w / z;
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
void ChangeValues(ref long a, ref long b)
{
    long temp = a;
    a = b;
    b = temp;
}
//funkcja zwracająca potęge liczby o danej podstawie i wykladniku
long pow(long baseNumber, long power)
{
    long result = 1;
    for (int i = 0; i < power; i++)
        result *= baseNumber;
    return result;
}