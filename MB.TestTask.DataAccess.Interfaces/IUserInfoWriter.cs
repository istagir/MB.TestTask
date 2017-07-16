using MB.TestTask.Models;

namespace MB.TestTask.DataAccess.Interfaces
{
    public interface IUserInfoWriter
    {
        long Add(UserInfo user);
        void Remove(long id);
        void Update(UserInfo user);
    }
}