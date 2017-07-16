using Dapper;
using MB.TestTask.DataAccess.Interfaces;
using MB.TestTask.Models;
using System;
using System.Data.SqlClient;
using System.Linq;

namespace MB.TestTask.DataAccess.SqlServer
{
    public class UserInfoWriter : IUserInfoWriter
    {
        private readonly string _connectionString;
        private readonly string _addQuery = @"
insert into [dbo].[User] (login, pass_hash, name) output inserted.id
values (@p_login, @p_pass_hash, @p_name);";
        private readonly string _updateQuery = @"
update [dbo].[User] 
   set [pass_hash] = @p_pass_hash,
       [name] = @p_name,
	   [last_sign_in_time] = @p_last_sign_in_time,
	   [sign_in_count] = @p_sign_in_count
 where [login] = @p_login;";


        public UserInfoWriter(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentNullException(nameof(connectionString));
            }

            _connectionString = connectionString;
        }

        public long Add(UserInfo user)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var parameters = new DynamicParameters(new
                {
                    p_login = user.Login,
                    p_pass_hash = user.PasswordHash,
                    p_name = user.Name
                });
                var result = default(long);
                result = connection.Query<long>(_addQuery, parameters).Single();
                return result;
            }
        }

        public void Update(UserInfo user)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var parameters = new DynamicParameters(new
                {
                    p_login = user.Login,
                    p_pass_hash = user.PasswordHash,
                    p_name = user.Name,
                    p_last_sign_in_time = user.LastSignInTime,
                    p_sign_in_count = user.SignInCount
                });

                connection.Query(_updateQuery, parameters);
            }
        }

        public void Remove(long id)
        {
            throw new NotImplementedException();
        }

    }
}
