using MB.TestTask.DataAccess.SqlServer;
using MB.TestTask.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace MB.TestTask.DataAccess.SqlServerTests
{
    [TestClass]
    public class UserInfoReaderTests
    {
        [TestMethod]
        public void List_Filtering_Test()
        {
            var connStr = @"Data Source=127.0.0.1\SQLEXPRESS;Initial Catalog=MD.TestTask.db;User ID=sa;Password=sapassword";
            var reader = new UserInfoReader(connStr);
            var filter = new UserInfoFilter
            {
                Logins = new List<string> { @"testUser12@mail.ru" }
            };

            var result = reader.List(filter);
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(@"testUser12@mail.ru", result.First().Login);
        }

        [TestMethod]
        public void List_OrderingAndPaging_Test()
        {
            var connStr = @"Data Source=127.0.0.1\SQLEXPRESS;Initial Catalog=MD.TestTask.db;User ID=sa;Password=sapassword";
            var reader = new UserInfoReader(connStr);
            var filter = new UserInfoFilter
            {
                OrderBy = new List<OrderingField>
                {
                    { new OrderingField { Field = nameof(UserInfo.SignInCount), Ascend = false } },
                    { new OrderingField { Field = nameof(UserInfo.LastSignInTime), Ascend = true } }
                },
                Page = 1,
                PageSize = 3
            };

            var result = reader.List(filter);
            Assert.IsNotNull(result);
        }
    }
}
