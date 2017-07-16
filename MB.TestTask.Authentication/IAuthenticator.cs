namespace MB.TestTask.Authentication
{
    public interface IAuthenticator
    {
        AuthenticationResult Authenticate(string login, string password);
    }
}
