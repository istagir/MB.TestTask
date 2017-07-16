using MB.TestTask.Models;
using System.Collections.Generic;

namespace MB.TestTask.Services.Interfaces
{
    public interface IUserService
    {
        UserInfo Add(UserInfo newUser);
        List<UserInfo> GetList();
        List<UserInfo> GetList(UserInfoFilter filter);
    }
}
