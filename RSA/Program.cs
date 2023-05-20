using System.Numerics;
using RSANamespace;

Random random = new Random();
// p i q powinny być losowymi liczbami pierwszymi
BigInteger p = MyRandom.choseRandomNumberFromList(100, 9000, random);
BigInteger q = MyRandom.choseRandomNumberFromList(100, 9000, random);
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
BigInteger m = 65;
//zaszyfrowanie wiadomosci
BigInteger c = RSA.Encrypt(m, publicKey);
Console.WriteLine("C = " + c);
//odszyfrowanie wiadomosci
BigInteger m2 = RSA.Decrypt(c, privateKey);
Console.WriteLine("M =" + m2);

BigInteger p2 = 97;
BigInteger q2 = 89;
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
Console.WriteLine($"\n\nWiadomosc = {message}\n");
List<BigInteger> encryptedMessage = Encryption.EncryptMessage(message, publicKey2);
Console.WriteLine(string.Join(", ", encryptedMessage));
string decryptedMessage = Encryption.DecryptMessage(encryptedMessage, privateKey2);
Console.WriteLine($"\nRozszyfrowana wiadomosc = {decryptedMessage}");


//wypisanie wszystkich liczb pierwszych z przedzialu od 0 do 100000
/*List<int> lista = MyRandom.findPrimeFromRange(0, 100000);
Console.WriteLine(string.Join(", ", lista));*/