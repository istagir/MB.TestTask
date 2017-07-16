using System;

namespace MB.TestTask.Authentication
{
    public class BCryptHashingService : IHashingService
    {
        private readonly string _salt;

        public BCryptHashingService(string salt)
        {
            if (string.IsNullOrEmpty(salt))
            {
                throw new ArgumentNullException(nameof(salt));
            }
            _salt = salt;
        }

        public string GetHash(string str)
        {
            return BCrypt.Net.BCrypt.HashPassword(str, _salt);
        }
    }
}
