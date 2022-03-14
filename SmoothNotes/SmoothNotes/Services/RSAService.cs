using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace SmoothNotes.Services
{
    public class RSAService
    {
        private static string _privateKey;
        private static string _publicKey;
        private static UnicodeEncoding _encoder = new UnicodeEncoding();
        private static void RSA()
        {
            var rsa = new RSACryptoServiceProvider();
        }
    }
}
