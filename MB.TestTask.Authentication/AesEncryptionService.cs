using System;
using System.IO;
using System.Security.Cryptography;

namespace MB.TestTask.Authentication
{
    public class AesEncryptionService : IEncryptionService
    {
        private byte[] _key;
        private byte[] _initializationVector;

        public AesEncryptionService(string key, string initializationVector)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key));
            }
            if (string.IsNullOrEmpty(initializationVector))
            {
                throw new ArgumentNullException(nameof(initializationVector));
            }

            _key = Convert.FromBase64String(key);
            _initializationVector = Convert.FromBase64String(initializationVector);
        }

        public byte[] Encrypt(string originalData)
        {
            using (var aes = new AesCryptoServiceProvider())
            {
                aes.Key = _key;
                aes.IV = _initializationVector;

                using (ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                        {
                            using (var streamWriter = new StreamWriter(cryptoStream))
                            {
                                streamWriter.Write(originalData);
                            }
                        }
                        return memoryStream.ToArray();
                    }
                }
            }
        }

        public string Decrypt(byte[] cipherData)
        {
            using (var aes = new AesCryptoServiceProvider())
            {
                aes.Key = _key;
                aes.IV = _initializationVector;

                using (var decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
                {
                    using (var memoryStream = new MemoryStream(cipherData))
                    {
                        using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                        {
                            using (var streamReader = new StreamReader(cryptoStream))
                            {
                                return streamReader.ReadToEnd();
                            }
                        }
                    }
                }
            }
        }
    }
}
