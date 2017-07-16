using MB.TestTask.DataAccess.Interfaces;
using MB.TestTask.Models;
using MB.TestTask.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MB.TestTask.ServicesTests
{
    [TestClass]
    public class UserServiceTests
    {
        /// <summary>
        /// Проверка добавления нового корректного пользователя
        /// </summary>
        [TestMethod]
        public void Add_ValidUser_Test()
        {
            var userInfoReader = new Mock<IUserInfoReader>();
            var userInfoWriter = new Mock<IUserInfoWriter>();

            userInfoReader
                .Setup(r => r.List(It.IsAny<UserInfoFilter>()))
                .Returns(new List<UserInfo>());
            userInfoWriter
                .Setup(w => w.Add(It.IsAny<UserInfo>()))
                .Returns(999);

            var service = new UserService(
                userInfoReader.Object,
                userInfoWriter.Object);
            var newUser = new UserInfo { Login = "unittest@mail.ru", PasswordHash = "333333" };

            var result = service.Add(newUser);

            Assert.IsNotNull(result);
            Assert.AreEqual(999, result.Id);
            Assert.AreEqual(newUser.Login, result.Login);
            Assert.AreEqual(newUser.PasswordHash, result.PasswordHash);
        }

        /// <summary>
        /// Проверка добавления нового пользователя с логином, который уже занят
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(System.ArgumentException))]
        public void Add_InvalidUser_Test()
        {
            var userInfoReader = new Mock<IUserInfoReader>();
            var userInfoWriter = new Mock<IUserInfoWriter>();

            userInfoReader
                .Setup(r => r.List(It.IsAny<UserInfoFilter>()))
                .Returns(new List<UserInfo> { new UserInfo { Login = "unittest@mail.ru", PasswordHash = "888888888" } });
            userInfoWriter
                .Setup(w => w.Add(It.IsAny<UserInfo>()))
                .Returns(999);

            var service = new UserService(
                userInfoReader.Object,
                userInfoWriter.Object);
            var newUser = new UserInfo { Login = "unittest@mail.ru", PasswordHash = "333333" };

            var result = service.Add(newUser);
        }

        /// <summary>
        /// Проверка метода GetList()
        /// </summary>
        [TestMethod]
        public void Add_GetList()
        {
            var userInfoReader = new Mock<IUserInfoReader>();
            var userInfoWriter = new Mock<IUserInfoWriter>();
            var existingUsers = GetRepository(50).ToList();

            userInfoReader
                .Setup(r => r.List(It.IsAny<UserInfoFilter>()))
                .Returns(existingUsers);

            var service = new UserService(
                userInfoReader.Object,
                userInfoWriter.Object);

            var result = service.GetList();

            Assert.IsNotNull(result);
            Assert.AreEqual(existingUsers.Count, result.Count);
        }

        /// <summary>
        /// Проверка метода GetList(UserInfoFilter filter)
        /// </summary>
        [TestMethod]
        public void Add_GetListByFilter_Test()
        {
            var userInfoReader = new Mock<IUserInfoReader>();
            var userInfoWriter = new Mock<IUserInfoWriter>();
            var existingUsers = GetRepository(50).ToList();

            userInfoReader
                .Setup(r => r.List(It.IsAny<UserInfoFilter>()))
                .Returns(existingUsers);

            var service = new UserService(
                userInfoReader.Object,
                userInfoWriter.Object);
            var filter = new UserInfoFilter
            {
                OrderBy = new List<OrderingField> { { new OrderingField { Field = "Id", Ascend = false } } },
                Page = 1,
                PageSize = 20
            };
            var result = service.GetList(filter);

            Assert.IsNotNull(result);
            Assert.AreEqual(existingUsers.Count, result.Count);
        }

        private IEnumerable<UserInfo> GetRepository(int repoSize)
        {
            Random rnd = new Random();
            for (int i = 0; i <= repoSize; i++)
            {
                var num = rnd.Next(0, 10000);
                yield return new UserInfo
                {
                    Id = i,
                    Login = $"unittest{num}@mail.ru",
                    Name = $"ТестовыйПользователь{num}",
                    LastSignInTime = DateTime.Now.AddSeconds(num),
                    SignInCount = num
                };
            }
        }
    }
}
