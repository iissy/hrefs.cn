using ASY.Hrefs.DAL.IRepository;
using ASY.Hrefs.Model.Models;
using ASY.Hrefs.Util.MiscHelpers;
using Dapper;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Data;

namespace ASY.Hrefs.DAL.Repository
{
    public class ArticleRepository : IArticleRepository
    {
        private string _connection;
        public ArticleRepository(IOptions<ConnectionStrings> conn)
        {
            _connection = conn.Value.Mysql;
        }

        public IEnumerable<Article> ListByPaging(int size, int skip, string fields = "*")
        {
            IEnumerable<Article> list;
            using (IDbConnection conn = SqlHelpers.CreateDbConnection(_connection))
            {
                string sql = string.Format($"SELECT {fields} FROM article ORDER BY catalog asc,createTime desc LIMIT @PageSize OFFSET @Offset");
                list = conn.Query<Article>(sql, new { PageSize = size, Offset = skip });
            }

            return list;
        }
    }
}