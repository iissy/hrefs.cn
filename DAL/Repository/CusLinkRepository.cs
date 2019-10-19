using ASY.Hrefs.DAL.IRepository;
using ASY.Hrefs.Model.Models;
using ASY.Hrefs.Util.MiscHelpers;
using Dapper;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;

namespace ASY.Hrefs.DAL.Repository
{
    public class CusLinkRepository : ICusLinkRepository
    {
        private string _connection;
        public CusLinkRepository(IOptions<ConnectionStrings> conn)
        {
            _connection = conn.Value.Mysql;
        }

        public CusLink GetCusLink(string id, string fields = "*")
        {
            var cusLink = new CusLink();
            using (IDbConnection conn = SqlHelpers.CreateDbConnection(_connection))
            {
                string sql = string.Format("SELECT {0} FROM cuslink WHERE Id = @Id", fields);
                cusLink = conn.QueryFirstOrDefault<CusLink>(sql, new { Id = id });
            }

            return cusLink;
        }

        public IEnumerable<CusLink> PagerCusLinkList(int size, int offset, string pcatid, string title, string url, out int total)
        {
            string sqlwhere = "where 1=1";
            if (!string.IsNullOrWhiteSpace(pcatid))
            {
                sqlwhere += $" and pcatid = @pcatid";
            }
            if (!string.IsNullOrWhiteSpace(title))
            {
                sqlwhere += $" and title like '%{title}%'";
            }
            if (!string.IsNullOrWhiteSpace(url))
            {
                sqlwhere += $" and url like '%{url}%'";
            }

            IEnumerable<CusLink> list;
            total = 0;
            using (IDbConnection conn = SqlHelpers.CreateDbConnection(_connection))
            {
                string sql = string.Format($"SELECT * FROM cuslink {sqlwhere} ORDER BY adddate desc LIMIT @size OFFSET @offset");
                list = conn.Query<CusLink>(sql, new { pcatid, size, offset });

                total = conn.QueryFirstOrDefault<int>($"select count(*) from cuslink {sqlwhere}", new { pcatid });
            }

            return list;
        }

        public int SaveCusLink(CusLink cusLink)
        {
            int result = 0;
            using (IDbConnection conn = SqlHelpers.CreateDbConnection(_connection))
            {
                if (cusLink.Id > 0)
                {
                    cusLink.Updatedate = DateTime.Now;
                    result = conn.Execute("update cuslink set " +
                        "title=@Title," +
                        "url=@Url," +
                        "catid=@Catid," +
                        "pcatid=@PCatid," +
                        "linktype=@LinkType," +
                        "updatedate=@Updatedate where id=@Id", cusLink);
                }
                else
                {
                    cusLink.Adddate = DateTime.Now;
                    result = conn.Execute("INSERT INTO cuslink(title,url,status,catid,pcatid,linktype,adddate)" +
                        "values(@Title,@Url,@Status,@Catid,@PCatid,@LinkType,@Adddate)", cusLink);
                }
            }

            return result;
        }

        public int DeleteCusLink(string id)
        {
            int result = 0;
            using (IDbConnection conn = SqlHelpers.CreateDbConnection(_connection))
            {
                result = conn.Execute("delete from cuslink where Id = @Id", new { Id = id });
            }

            return result;
        }

        public IEnumerable<CusLink> GetTopCusLink(int size, string fields = "*")
        {
            IEnumerable<CusLink> list;
            using (IDbConnection conn = SqlHelpers.CreateDbConnection(_connection))
            {
                string sql = string.Format("SELECT {0} FROM cuslink ORDER BY adddate desc LIMIT @size", fields);
                list = conn.Query<CusLink>(sql, new { size });
            }

            return list;
        }
    }
}