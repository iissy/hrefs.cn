using ASY.Hrefs.DAL.IRepository;
using ASY.Hrefs.Model.Models;
using ASY.Hrefs.Util.MiscHelpers;
using Dapper;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Data;

namespace ASY.Hrefs.DAL.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private string _connection;
        public AccountRepository(IOptions<ConnectionStrings> conn)
        {
            _connection = conn.Value.Mysql;
        }

        public Account GetAccount(string id, string fields = "*")
        {
            var account = new Account();
            using (IDbConnection conn = SqlHelpers.CreateDbConnection(_connection))
            {
                string sql = string.Format("SELECT {0} FROM account WHERE Id = @Id", fields);
                account = conn.QueryFirstOrDefault<Account>(sql, new { Id = id });
            }

            return account;
        }

        public IEnumerable<Account> PagerAccountList(int size, int skip, string username, out int total, string fields = "*")
        {
            IEnumerable<Account> list;
            using (IDbConnection conn = SqlHelpers.CreateDbConnection(_connection))
            {
                string sql = string.Format($"SELECT {fields} FROM account ORDER BY id desc LIMIT @PageSize OFFSET @Offset");
                list = conn.Query<Account>(sql, new { PageSize = size, Offset = skip });

                total = conn.QueryFirstOrDefault<int>($"select count(*) FROM account");
            }

            return list;
        }

        public Account GetLogin(string userid, string password, string fields = "*")
        {
            Account account;
            using (IDbConnection conn = SqlHelpers.CreateDbConnection(_connection))
            {
                string sql = string.Format($"SELECT * FROM account where userid=@userid and password=@password");
                account = conn.QueryFirstOrDefault<Account>(sql, new { userid, password });
            }

            return account;
        }

        public int SaveAccount(Account account)
        {
            throw new System.NotImplementedException();
        }

        public int DeleteAccount(string id)
        {
            int result = 0;
            using (IDbConnection conn = SqlHelpers.CreateDbConnection(_connection))
            {
                result = conn.Execute("delete from article where Id = @Id", new { Id = id });
            }

            return result;
        }
    }
}