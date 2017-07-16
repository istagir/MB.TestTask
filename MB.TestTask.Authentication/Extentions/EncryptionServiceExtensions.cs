using Newtonsoft.Json;
using System.IO;
using System.Text;

namespace MB.TestTask.Authentication.Extensions
{
    public static class EncryptionServiceExtensions
    {
        public static byte[] Encrypt<T>(this IEncryptionService encryptionService, T originalObject)
        {
            var serializer = new JsonSerializer();
            var json = new StringBuilder();
            using (var stringWriter = new StringWriter(json))
            {
                serializer.Serialize(stringWriter, originalObject);
            }

            return encryptionService.Encrypt(json.ToString());
        }

        public static T Decrypt<T>(this IEncryptionService encryptionService, byte[] cipherData)
        {
            var json = encryptionService.Decrypt(cipherData);
            var serializer = new JsonSerializer();

            using (var stringReader = new StringReader(json))
            {
                return (T)serializer.Deserialize(stringReader, typeof(T));
            }
        }
    }
}
