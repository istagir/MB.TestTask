namespace MB.TestTask.Authentication
{
    public interface IEncryptionService
    {
        byte[] Encrypt(string originalData);

        string Decrypt(byte[] cipherData);
    }
}
