namespace MB.TestTask.Authentication
{
    public interface IHashingService
    {
        string GetHash(string str);
    }
}
