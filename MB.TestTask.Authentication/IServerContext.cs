using MB.TestTask.Models;

namespace MB.TestTask.Authentication
{
    public interface IServerContext
    {
        UserInfo CurrentUser { get; set; }
        byte[] Token { get; set; }
    }
}
