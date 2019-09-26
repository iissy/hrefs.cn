using ASY.Hrefs.DAL.IRepository;
using ASY.Hrefs.Model.Models;
using ASY.Hrefs.Util.MiscHelpers;
using Dapper;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Data;

namespace ASY.Hrefs.DAL.Repository
{
    public class LinkRepository : ILinkRepository
    {
        private string _connection;
        public LinkRepository(IOptions<ConnectionStrings> conn)
        {
            _connection = conn.Value.Mysql;
        }

        public IEnumerable<Link> GetAllLink()
        {
            IEnumerable<Link> list;
            using (IDbConnection conn = SqlHelpers.CreateDbConnection(_connection))
            {
                string sql = string.Format($"SELECT icon,title,id,linkType FROM link ORDER BY linkType asc,visited desc");
                list = conn.Query<Link>(sql);
            }

            return list;
        }

        public IEnumerable<Link> ListLinkByCat(string linktype)
        {
            IEnumerable<Link> list;
            using (IDbConnection conn = SqlHelpers.CreateDbConnection(_connection))
            {
                string sql = string.Format($"SELECT title,id,visited,brief FROM link where linkType = @linkType ORDER BY visited desc");
                list = conn.Query<Link>(sql, new { linktype });

            }

            return list;
        }

        public int LinksVisitedCount()
        {
            int total = 0;
            using (IDbConnection conn = SqlHelpers.CreateDbConnection(_connection))
            {
                total = conn.QuerySingle<int>("select sum(visited) from link");
            }

            return total;
        }

        public int UpdatedLinkVisited(string id)
        {
            int result = 0;
            using (IDbConnection conn = SqlHelpers.CreateDbConnection(_connection))
            {
                result = conn.Execute("update link set visited = visited + 1 where Id = @Id", new { Id = id });
            }

            return result;
        }

        public Link GetLink(string id, string fields = "*")
        {
            var link = new Link();
            using (IDbConnection conn = SqlHelpers.CreateDbConnection(_connection))
            {
                string sql = string.Format("SELECT {0} FROM link WHERE Id = @Id", fields);
                link = conn.QueryFirstOrDefault<Link>(sql, new { Id = id });
            }

            return link;
        }
    }
}