using MB.TestTask.DataAccess.Interfaces;
using MB.TestTask.Models;
using MB.TestTask.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MB.TestTask.Services
{
    public class UserService : IUserService
    {
        private readonly IUserInfoReader _userInfoReader;
        private readonly IUserInfoWriter _userInfoWriter;

        public UserService(
            IUserInfoReader userInfoReader,
            IUserInfoWriter userInfoWriter)
        {
            if (userInfoReader == null)
            {
                throw new ArgumentNullException(nameof(userInfoReader));
            };
            if (userInfoWriter == null)
            {
                throw new ArgumentNullException(nameof(userInfoWriter));
            };

            _userInfoReader = userInfoReader;
            _userInfoWriter = userInfoWriter;
        }

        public UserInfo Add(UserInfo newUser)
        {
            var filter = new UserInfoFilter { Logins = new List<string> { newUser.Login } };
            if (_userInfoReader.List(filter).Any())
            {
                throw new ArgumentException($"User with login = {newUser.Login} already exists.");
            };

            newUser.Id = _userInfoWriter.Add(newUser);
            return newUser;
        }

        public List<UserInfo> GetList()
        {
            return _userInfoReader.List(new UserInfoFilter());
        }

        public List<UserInfo> GetList(UserInfoFilter filter)
        {
            if (filter == null)
            {
                return _userInfoReader.List();
            }

            return _userInfoReader.List(filter);
        }
    }
}
