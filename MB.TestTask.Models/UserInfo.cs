using System;

namespace MB.TestTask.Models
{
    public class UserInfo
    {
        public string Login { get; set; }
        public long? Id { get; set; }
        public string Name { get; set; }
        public string PasswordHash { get; set; }
        public DateTime? LastSignInTime { get; set; }
        public long SignInCount { get; set; }
    }
}
