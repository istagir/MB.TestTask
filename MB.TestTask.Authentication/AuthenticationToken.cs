using MB.TestTask.Models;
using System;

namespace MB.TestTask.Authentication
{
    public class AuthenticationToken
    {
        public UserInfo User { get; set; }
        public DateTime Expires { get; set; }
    }
}
