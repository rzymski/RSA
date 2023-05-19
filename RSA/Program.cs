Console.WriteLine("Hello, World!");

//Console.WriteLine(EulerFunction(11, 7));

int p = 13;//13;
int q = 11;// 97;

int n = CountN(p, q);
int phiN = EulerFunction(p, q);
int e = FindE(phiN);
int d = InverseModulo(e, phiN);

Console.WriteLine("N =" + n);
Console.WriteLine("Funkcja Eulera phiN = " + phiN);
Console.WriteLine("E = " + e);
Console.WriteLine("D = " + d);

int CountN(int p, int q)
{
    return p * q;
}
int EulerFunction(int p, int q) //zwraca phiN
{
    return (p - 1) * (q - 1);
}

int NWD(int a, int b)
{
    while(b != 0)
    {
        int c = a % b;
        a = b;
        b = c;
        //Console.WriteLine($"A = {a}  B = {b}  C = {c}");
    }
    return a;
}

// phiN to wynik EulerFunction
int FindE(int phiN)
{
    int e = 2; //najniejszy możliwy wynik gdzie 1 < e < phiN
    while(NWD(e, phiN) != 1)
    {
        e++;
    }
    return e;
}

//Funkcja zwracająca odwrotność modularną wykorzystuje Rozszerzony algorytm Euklidesa
int InverseModulo(int number, int modulo)
{
    int u = 1, w = number, x = 0, z = modulo;
    while (w != 0)
    {
        if (w < z)
        {
            ChangeValues(ref u, ref x);
            ChangeValues(ref w, ref z);
        }
        int q = w / z;
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

void ChangeValues(ref int a, ref int b)
{
    int temp = a;
    a = b;
    b = temp;
}