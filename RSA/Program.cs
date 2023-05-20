using System.Numerics;
using RSANamespace;

Random random = new Random();
// p i q powinny być losowymi liczbami pierwszymi
BigInteger p = MyRandom.choseRandomNumberFromList(5000, 6000, random);
BigInteger q = MyRandom.choseRandomNumberFromList(20000, 30000, random);
/*BigInteger p = 9001;//97;
BigInteger q = 19001;//89;*/
BigInteger n = RSA.CountN(p, q);
BigInteger phiN = RSA.EulerFunction(p, q);
BigInteger e = RSA.FindE(phiN);
BigInteger d = RSA.InverseModulo(e, phiN);
Console.WriteLine("P = " + p);
Console.WriteLine("q = " + q);
Console.WriteLine("N =" + n);
Console.WriteLine("Funkcja Eulera phi(n) = " + phiN);
Console.WriteLine("E = " + e);
Console.WriteLine("D = " + d);
KeyValuePair<BigInteger, BigInteger> publicKey = new KeyValuePair<BigInteger, BigInteger>(n, e);
KeyValuePair<BigInteger, BigInteger> privateKey = new KeyValuePair<BigInteger, BigInteger>(n, d);
//m to blok wiadomosci i nie moze byc wiekszy niz n 
BigInteger m = 9000;
//zaszyfrowanie wiadomosci
BigInteger c = RSA.Encrypt(m, publicKey);
Console.WriteLine("C = " + c);
//odszyfrowanie wiadomosci
BigInteger m2 = RSA.Decrypt(c, privateKey);
Console.WriteLine("M =" + m2);