using ASY.Hrefs.Model.Models;
using System.Collections.Generic;

namespace ASY.Hrefs.DAL.IRepository
{
    public interface IAccountRepository
    {
        int SaveAccount(Account account);
        Account GetAccount(string id, string fields = "*");
        Account GetLogin(string userid, string password, string fields = "*");
        int DeleteAccount(string id);
        IEnumerable<Account> PagerAccountList(int size, int skip, string username, out int total, string fields = "*");
    }
}