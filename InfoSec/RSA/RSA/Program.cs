using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace RSA
{
    internal class Program
    {
        public static void Main(string[] args)
        {

            RsaTest rsaTest = new RsaTest();
            //rsaTest.Rsa1("hello");
            //rsaTest.Rsa2("hello");
            rsaTest.Rsa3("hello");
        }
    }

    class RsaTest {
        public void Rsa1(String msg)
        {
            byte[] data = Encoding.ASCII.GetBytes(msg);
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            byte[] cipher = rsa.Encrypt(data, true);
            string cipher_str = Convert.ToBase64String(cipher);
            Console.WriteLine(cipher_str);

            byte[] cipher1 = Convert.FromBase64String(cipher_str);
            byte[] palin1 = rsa.Decrypt(cipher1, true);
            string palin = Encoding.ASCII.GetString(palin1);
            Console.WriteLine(palin);

            Console.Read();
        }

        public void Rsa2(String msg)
        {
            CspParameters csp = new CspParameters();
            csp.KeyContainerName = "mykey";
            byte[] _msg = Encoding.ASCII.GetBytes(msg);
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(csp);
            byte[] cipher = rsa.Encrypt(_msg, true);
            String cipher_string = Convert.ToBase64String(cipher);
            Console.WriteLine(cipher_string);

            //解密
            CspParameters csp1 = new CspParameters();
            csp1.KeyContainerName = "mykey";
            RSACryptoServiceProvider rsa1 = new RSACryptoServiceProvider(csp);
            byte[] cipher1 = rsa1.Decrypt(cipher, true);
            string decryptedText = Encoding.ASCII.GetString(cipher1);
            Console.WriteLine(decryptedText);
        }

        public void Rsa3(String msg) {
            byte[] _msg = Encoding.ASCII.GetBytes(msg);
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            byte[] signature = rsa.SignData(_msg, "SHA1");
            string signature_str = Convert.ToBase64String(signature);
            Console.WriteLine(signature_str);

            byte[] signature1 = Convert.FromBase64String(signature_str);
            bool verification = rsa.VerifyData(_msg, "SHA1", signature1);
            Console.Write(verification);

            Console.Read();
        }
    }
}
