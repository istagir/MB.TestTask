using MB.TestTask.Models;

namespace MB.TestTask.Authentication
{
    public class ServerContext : IServerContext
    {
        public UserInfo CurrentUser { get; set; }
        public byte[] Token { get; set; }
    }
}
