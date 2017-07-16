using Dapper;
using MB.TestTask.DataAccess.Interfaces;
using MB.TestTask.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace MB.TestTask.DataAccess.SqlServer
{
    public class UserInfoReader : IUserInfoReader
    {
        private readonly string _connectionString;
        private readonly string _baseQuery = @"
select 
       id as Id,
	   login as Login,
	   pass_hash as PasswordHash,
	   name as Name,
	   last_sign_in_time as LastSignInTime,
	   sign_in_count as SignInCount
  from [dbo].[User]
 where (1=1)";

        public UserInfoReader(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentNullException(nameof(connectionString));
            }

            _connectionString = connectionString;
        }

        public List<UserInfo> List()
        {
            return List(new UserInfoFilter());
        }

        public List<UserInfo> List(UserInfoFilter filter)
        {
            var query = _baseQuery;
            var parameters = new DynamicParameters();

            if (filter.Ids != null && filter.Ids.Any())
            {
                query += " and id in @p_ids";
                parameters.Add("p_ids", filter.Ids);
            }

            if (filter.Logins != null && filter.Logins.Any())
            {
                query += " and login in @p_logins";
                parameters.Add("p_logins", filter.Logins);
            }

            if (filter.OrderBy != null && filter.OrderBy.Any())
            {
                var orderby = new List<string>();
                foreach (var o in filter.OrderBy)
                {
                    orderby.Add(GetDbFieldName(o.Field) + (o.Ascend ? " asc" : " desc"));
                };
                query += " order by " + string.Join(",", orderby);
            }
            else
            {
                query += " order by id";
            }

            if (filter.Page.HasValue && filter.PageSize.HasValue)
            {
                var rowFrom = filter.Page.Value * filter.PageSize.Value;
                var rowNext = filter.PageSize.Value;
                query += $" offset {rowFrom} rows fetch next {rowNext} rows only";
            }

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                return connection.Query<UserInfo>(query, parameters).ToList();
            }
        }

        private string GetDbFieldName(string source)
        {
            string result = string.Empty;
            if (source.ToLower() == nameof(UserInfo.Id).ToLower())
            {
                result = "id";
            }
            else if (source.ToLower() == nameof(UserInfo.Login).ToLower())
            {
                result = "login";
            }
            else if (source.ToLower() == nameof(UserInfo.Name).ToLower())
            {
                result = "name";
            }
            else if (source.ToLower() == nameof(UserInfo.LastSignInTime).ToLower())
            {
                result = "last_sign_in_time";
            }
            else if (source.ToLower() == nameof(UserInfo.SignInCount).ToLower())
            {
                result = "sign_in_count";
            }
            else
            {
                throw new ArgumentException($"Can not order by field with name=/'{source}/'");
            }
            return result;
        }
    }
}
