using SmoothNotes.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace SmoothNotes.Services.Storage
{
    public static class CryptographService
    {
        private static string containerName = "RSAContainer";

        /// <summary>
        /// Converts password to usable byte[] for encryption
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private static async Task<byte[]> GetKeyBytes(string arg)
        {
            //Password is minimum 8 characters

            // Padding characters
            List<string> filler = new List<string> { "0", "j", "f", "i", "5", "o", "j", "f", "i", "e", "7", "j", "i", "f", "j", "a", "o", "e", "8", "f", "k", "n", "u", "9", "u", "e", "1", "h", "d", "a", "k", "d" };
            Console.WriteLine(filler.Count);
            int i = 1;

            // Password larger than 32 characters
            if (arg.Length > 32)
            {
                arg = arg.Substring(0, 32);
            }

            // Password smaller than 32 characters, until it is 32 characters
            while (arg.Length < 32)
            {
                arg += filler[i];
                i++;
            }

            // Returns padded password
            return await ConvertToByteArray(arg);
        }

        public static async Task<List<Note>> DecryptNotes(List<Note> notes)
        {
            foreach (var item in notes)
            {
                item.Text = await RSADecrypt(item.Text, await ReadEncodedKey(await AesDecrypt(ValueParserService.profile.Prk, await SecureStorage.GetAsync(ValueParserService.profile.Name))), false);
            }
            return notes;
        }

        /// <summary>
        /// Decrypt RSA data
        /// </summary>
        /// <param name="DataToDecrypt">data</param>
        /// <param name="Key">Private key</param>
        /// <param name="DoOAEPPadding">bool for padding settings - default should be false</param>
        /// <returns></returns>
        private static async Task<string> RSADecrypt(string DataToDecrypt, string Key, bool DoOAEPPadding)
        {
            try
            {
                byte[] data = Convert.FromBase64String(DataToDecrypt);
                byte[] decryptedData;
                //Create a new instance of RSACryptoServiceProvider.
                using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
                {
                    //Import the RSA Key information. This needs
                    //to include the private key information.
                    //RSA.ImportParameters(RSAKeyInfo);
                    RSA.FromXmlString(Key);

                    //Decrypt the passed byte array and specify OAEP padding.  
                    //OAEP padding is only available on Microsoft Windows XP or
                    //later.  
                    decryptedData = RSA.Decrypt(data, DoOAEPPadding);
                }
                return await ConvertToString(decryptedData);
            }
            //Catch and display a CryptographicException  
            //to the console.
            catch (CryptographicException e)
            {
                Console.WriteLine(e.ToString());

                return null;
            }
        }

        /// <summary>
        /// Creating, Adding private and public key to a new profile being created.
        /// </summary>
        /// <param name="args">object for creating new profile</param>
        /// <returns></returns>
        internal static async Task<Register> AddKeys(Register args)
        {
            List<string> pair = new List<string>();
            pair = await GenKeyPair();

            string ePrK = "";
            ePrK = await AesEncrypt(pair[0], args.Pw);


            args.Prk = ePrK;
            args.PuK = Convert.ToBase64String(await HexStringToByteArray(pair[1]));

            return args;
        }

        /// <summary>
        /// Quick translation for reading the puk key
        /// Puk key is a byte[], but is converted to hex string and then base64 for storage
        /// </summary>
        /// <param name="puK">public key</param>
        /// <returns></returns>
        public static async Task<string> ReadPuKKey(string puK)
        {
            return await ReadEncodedKey(await ByteArrayToHexString(Convert.FromBase64String(puK)));
        }

        private static async Task<string> ConvertToString(byte[] bytes)
        {
            return new string(bytes.Select(Convert.ToChar).ToArray());
        }

        private static async Task<string> ReadEncodedKey(string arg)
        {
            return Encoding.Unicode.GetString(await HexStringToByteArray(arg));
        }

        private static async Task<byte[]> HexStringToByteArray(string hex)
        {
            byte[] ba = Enumerable.Range(0, hex.Length / 2).Select(x => Convert.ToByte(hex.Substring(x * 2, 2), 16)).ToArray();
            return ba;
        }

        private static async Task<string> ByteArrayToHexString(byte[] ba)
        {
            return BitConverter.ToString(ba).Replace("-", "");
        }

        private static async Task<byte[]> ConvertToByteArray(string input)
        {
            return input.Select(Convert.ToByte).ToArray();
        }

        /// <summary>
        /// Encrypts data with AES process
        /// </summary>
        /// <param name="Text">data</param>
        /// <param name="Key">key for encryption</param>
        /// <returns>string containing IV and encrypted code</returns>
        /// <exception cref="ArgumentNullException"></exception>
        private static async Task<string> AesEncrypt(string Text, string Key)
        {
            //Check values
            if (Text == null || Text.Length <= 0) throw new ArgumentNullException("Text");
            if (Key == null || Key.Length <= 0) throw new ArgumentNullException("Key");

            byte[] eData;
            string final;

            //Create an Aes object with the specified key and IV
            using (Aes aes = Aes.Create())
            {
                aes.Key = await GetKeyBytes(Key);

                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, aes.CreateEncryptor(aes.Key, aes.IV), CryptoStreamMode.Write))
                    {
                        using (StreamWriter sw = new StreamWriter(cs))
                        {
                            //Writing the data to the stream
                            sw.Write(Text);
                        }
                        eData = ms.ToArray();
                    }
                }

                final = Convert.ToBase64String(await HexStringToByteArray(await ByteArrayToHexString(aes.IV) + await ByteArrayToHexString(eData)));
            }


            // Return IV and the encrypted data as hex string
            return final;
        }

        /// <summary>
        /// Decrypts AES encrypted data
        /// </summary>
        /// <param name="cText">encrypted data</param>
        /// <param name="Key">key for decryption</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        private static async Task<string> AesDecrypt(string cText, string Key)
        {
            //Check values
            if (cText == null || cText.Length <= 0) throw new ArgumentNullException("cText");
            if (Key == null || Key.Length <= 0) throw new ArgumentNullException("Key");

            string dData;
            string hex = await ByteArrayToHexString(Convert.FromBase64String(cText));
            string iv = hex.Substring(0, 32);
            byte[] data = await HexStringToByteArray(hex.Substring(32));

            using (Aes aes = Aes.Create())
            {
                aes.Key = await GetKeyBytes(Key);
                aes.IV = await HexStringToByteArray(iv);

                using (MemoryStream ms = new MemoryStream(data))
                {
                    using (CryptoStream cs = new CryptoStream(ms, aes.CreateDecryptor(aes.Key, aes.IV), CryptoStreamMode.Read))
                    {
                        using (StreamReader sr = new StreamReader(cs))
                        {
                            // Read the decrypted bytes and place them in a string
                            dData = sr.ReadToEnd();
                        }
                    }
                }
            }

            // Return the decrypted string
            return dData;
        }

        /// <summary>
        /// Encrypts data with RSA
        /// </summary>
        /// <param name="DataToEncrypt">data</param>
        /// <param name="Key">public key</param>
        /// <param name="DoOAEPPadding">bool for padding settings - default should be false</param>
        /// <returns></returns>
        public static async Task<string> RSAEncrypt(string DataToEncrypt, string Key, bool DoOAEPPadding)
        {
            byte[] data = await ConvertToByteArray(DataToEncrypt);

            byte[] encryptedData;
            //Create a new instance of RSACryptoServiceProvider.
            using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
            {
                RSA.FromXmlString(Key);
                //Import the RSA Key information. This only needs
                //toinclude the public key information.
                //RSA.ImportParameters(RSAKeyInfo);

                //Encrypt the passed byte array and specify OAEP padding.  
                //OAEP padding is only available on Microsoft Windows XP or
                //later.  
                encryptedData = RSA.Encrypt(data, DoOAEPPadding);
            }
            return Convert.ToBase64String(encryptedData);
        }

        /// <summary>
        /// Generates private and public key
        /// </summary>
        /// <returns>list of string</returns>
        private static async Task<List<string>> GenKeyPair()
        {
            // Create the CspParameters object and set the key container
            // name used to store the RSA key pair.
            var parameters = new CspParameters
            {
                KeyContainerName = containerName
            };

            List<string> result = new List<string>();
            // Create a new instance of RSACryptoServiceProvider that accesses
            // the key container MyKeyContainerName.
            using (var rsa = new RSACryptoServiceProvider(parameters)
            {
                PersistKeyInCsp = false
            })
            {
                //Private Key
                result.Add(await EncodeKey(rsa.ToXmlString(true)));
                //Public Key
                result.Add(await EncodeKey(rsa.ToXmlString(false)));

                // Call Clear to release resources and delete the key from the container.
                rsa.Clear();
            }

            return result;
        }

        private static async Task<string> EncodeKey(string arg)
        {
            return await ByteArrayToHexString(Encoding.Unicode.GetBytes(arg));
        }

    }
}
