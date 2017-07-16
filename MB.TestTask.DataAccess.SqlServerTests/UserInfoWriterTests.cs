using MB.TestTask.DataAccess.SqlServer;
using MB.TestTask.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Transactions;

namespace MB.TestTask.DataAccess.SqlServerTests
{
    [TestClass]
    public class UserInfoWriterTests
    {
        [TestMethod]
        public void Add_Test()
        {
            var connStr = @"Data Source=127.0.0.1\SQLEXPRESS;Initial Catalog=MD.TestTask.db;User ID=sa;Password=sapassword";
            var writer = new UserInfoWriter(connStr);

            var newUser = new UserInfo
            {
                Login = "testUser22@mail.ru",
                Name = "Тестовый Пользователь 2",
                PasswordHash = "454545454545454545"
            };

            using (var transaction = new TransactionScope())
            {
                var newId = writer.Add(newUser);
                Assert.AreNotEqual(newId, default(long));
            }
        }

        [TestMethod]
        public void Update_Test()
        {
            var connStr = @"Data Source=127.0.0.1\SQLEXPRESS;Initial Catalog=MD.TestTask.db;User ID=sa;Password=sapassword";
            var writer = new UserInfoWriter(connStr);

            var updatedUser = new UserInfo
            {
                Login = "testUser1@mail.ru",
                Name = "UPDATE_TEST",
                PasswordHash = "45454545_UPDATE_TEST_4545454545",
                LastSignInTime = DateTime.Now,
                SignInCount = 1
            };

            using (var transaction = new TransactionScope())
            {
                writer.Update(updatedUser);
            }
        }
    }
}
