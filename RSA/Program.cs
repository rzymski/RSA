using System.Numerics;
using RSANamespace;

Random random = new Random();
// p i q powinny być losowymi liczbami pierwszymi
BigInteger p = MyRandom.ChoseRandomNumberFromList(100, 9000, random);//199999;
BigInteger q = MyRandom.ChoseRandomNumberFromList(100, 9000, random);//109841;
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
Console.WriteLine($"Klucz publiczny (n, e) = ({publicKey.Key},{publicKey.Value})");
Console.WriteLine($"Klucz prywatny (n, d) = ({privateKey.Key},{privateKey.Value})");
//m to blok wiadomosci i nie moze byc wiekszy niz n 
//np. mozna zmieniac znaki na wartosci ascii czyli 'A' = 65
BigInteger m = 65;
Console.WriteLine("Blok m = " + m);
//zaszyfrowanie bloku wiadomosci
BigInteger c = RSA.Encrypt(m, publicKey);
Console.WriteLine("Zaszyfrowanie bloku m:\nC = " + c);
//odszyfrowanie bloku wiadomosci
BigInteger m2 = RSA.Decrypt(c, privateKey);
Console.WriteLine("Odszyfrowanie bloku m:\nM =" + m2);

BigInteger p2 = 199;
BigInteger q2 = 251;
BigInteger n2 = RSA.CountN(p2, q2);
BigInteger phiN2 = RSA.EulerFunction(p2, q2);
BigInteger e2 = RSA.FindE(phiN2);
BigInteger d2 = RSA.InverseModulo(e2, phiN2);
KeyValuePair<BigInteger, BigInteger> publicKey2 = new KeyValuePair<BigInteger, BigInteger>(n2, e2);
KeyValuePair<BigInteger, BigInteger> privateKey2 = new KeyValuePair<BigInteger, BigInteger>(n2, d2);
Console.WriteLine("\n\n\nP = " + p2);
Console.WriteLine("q = " + q2);
Console.WriteLine("N =" + n2);
Console.WriteLine("Funkcja Eulera phi(n) = " + phiN2);
Console.WriteLine("E = " + e2);
Console.WriteLine("D = " + d2);
Console.WriteLine($"Klucz publiczny (n, e) = ({publicKey2.Key},{publicKey2.Value})");
Console.WriteLine($"Klucz prywatny (n, d) = ({privateKey2.Key},{privateKey2.Value})");
string message = "POTEGA I MOC";
Console.WriteLine($"\nWiadomosc = {message}\n");
List<BigInteger> encryptedMessage = Encryption.EncryptMessage(message, publicKey2);
Console.WriteLine(string.Join(", ", encryptedMessage));
string decryptedMessage = Encryption.DecryptMessage(encryptedMessage, privateKey2);
Console.WriteLine($"\nRozszyfrowana wiadomosc = {decryptedMessage}\n\n\n");


//wypisanie wszystkich liczb pierwszych z przedzialu
/*List<int> lista = MyRandom.FindPrimeFromRange(100000, 200000);
Console.WriteLine(string.Join(", ", lista));*/