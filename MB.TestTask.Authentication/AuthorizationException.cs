using System;

namespace MB.TestTask.Authentication
{
    public class AuthorizationException : Exception
    {
        public AuthorizationException(string message)
            : base(message)
        {
        }
    }
}
