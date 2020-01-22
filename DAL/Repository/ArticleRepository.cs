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

        public int DeleteArticle(string id)
        {
            int result = 0;
            using (IDbConnection conn = SqlHelpers.CreateDbConnection(_connection))
            {
                result = conn.Execute("delete from article where Id = @Id", new { Id = id });
            }

            return result;
        }

        public Article GetArticle(string id, string fields = "*")
        {
            var article = new Article();
            using (IDbConnection conn = SqlHelpers.CreateDbConnection(_connection))
            {
                string sql = string.Format("SELECT {0} FROM article WHERE Id = @Id", fields);
                article = conn.QueryFirstOrDefault<Article>(sql, new { Id = id });
            }

            return article;
        }

        public IEnumerable<Article> ListArticleByPaging(int size, int skip, string fields = "*")
        {
            IEnumerable<Article> list;
            using (IDbConnection conn = SqlHelpers.CreateDbConnection(_connection))
            {
                string sql = string.Format($"SELECT {fields} FROM article ORDER BY createTime desc LIMIT @PageSize OFFSET @Offset");
                list = conn.Query<Article>(sql, new { PageSize = size, Offset = skip });
            }

            return list;
        }

        public IEnumerable<Article> PagerArticleList(int size, int skip, string id, string title, out int total, string fields = "*")
        {
            string sqlWhere = $"where 1=1";
            if (!string.IsNullOrWhiteSpace(id))
            {
                sqlWhere = $"where id = @id";
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(title))
                {
                    sqlWhere += $" and title like '%{title}%'";
                }
            }

            IEnumerable<Article> list;
            using (IDbConnection conn = SqlHelpers.CreateDbConnection(_connection))
            {
                string sql = string.Format($"SELECT {fields} FROM article {sqlWhere} ORDER BY createTime desc LIMIT @PageSize OFFSET @Offset");
                list = conn.Query<Article>(sql, new { id, title, PageSize = size, Offset = skip });

                total = conn.QueryFirstOrDefault<int>($"select count(*) FROM article {sqlWhere}", new { id, title });
            }

            return list;
        }

        public int SaveArticle(Article article)
        {
            int result = 0;
            using (IDbConnection conn = SqlHelpers.CreateDbConnection(_connection))
            {
                object exists = conn.ExecuteScalar("select id from article where Id = @Id", new { Id = article.Id });
                if (exists == null && article.AddOrEdit)
                {
                    result = conn.Execute("INSERT INTO article(id,icon,title,brief,body)" +
                    "values(@Id,@Icon,@Title,@Brief,@Body)", article);
                }
                else if (exists != null && !article.AddOrEdit)
                {
                    result = conn.Execute("update article set " +
                        "icon=@Icon," +
                        "title=@Title," +
                        "brief=@Brief," +
                        "body=@Body where Id=@Id", article);
                }
            }

            return result;
        }

        public int UpdatedArticleVisited(string id)
        {
            int result = 0;
            using (IDbConnection conn = SqlHelpers.CreateDbConnection(_connection))
            {
                result = conn.Execute("update article set visited=visited+1 where id=@id", new { id });
            }

            return result;
        }
    }
}