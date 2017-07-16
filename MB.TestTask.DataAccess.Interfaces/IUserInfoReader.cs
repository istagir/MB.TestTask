using MB.TestTask.Models;
using System.Collections.Generic;

namespace MB.TestTask.DataAccess.Interfaces
{
    public interface IUserInfoReader
    {
        List<UserInfo> List();
        List<UserInfo> List(UserInfoFilter filter);
    }
}
