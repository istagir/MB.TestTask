namespace MB.TestTask.Authentication
{
    public class AuthenticationResult
    {
        public byte[] Token { get; set; }
        public long TokenExpirationMilliseconds { get; set; }
    }
}
