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

        public IEnumerable<Link> ListLinkByCat(string catid)
        {
            IEnumerable<Link> list;
            using (IDbConnection conn = SqlHelpers.CreateDbConnection(_connection))
            {
                string sql = string.Format($"SELECT title,id,visited,brief FROM link where catid = @catid ORDER BY visited desc");
                list = conn.Query<Link>(sql, new { catid });

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

        public IEnumerable<Link> PagerLinkList(int size, int offset, string catid, string title, string url, out int total)
        {
            string sqlwhere = "where 1=1";
            if (!string.IsNullOrWhiteSpace(catid))
            {
                sqlwhere += $" and catid = @catid";
            }
            if (!string.IsNullOrWhiteSpace(title))
            {
                sqlwhere += $" and title like '%{title}%'";
            }
            if (!string.IsNullOrWhiteSpace(url))
            {
                sqlwhere += $" and url like '%{url}%'";
            }

            IEnumerable<Link> list;
            total = 0;
            using (IDbConnection conn = SqlHelpers.CreateDbConnection(_connection))
            {
                string sql = string.Format($"SELECT * FROM link {sqlwhere} ORDER BY createtime desc LIMIT @size OFFSET @offset");
                list = conn.Query<Link>(sql, new { catid, size, offset });

                total = conn.QueryFirstOrDefault<int>($"select count(*) from link {sqlwhere}", new { catid });
            }

            return list;
        }

        public int SaveLink(Link link)
        {
            int result = 0;
            using (IDbConnection conn = SqlHelpers.CreateDbConnection(_connection))
            {
                Guid guid = new Guid();
                if (Guid.TryParse(link.Id, out guid))
                {
                    result = conn.Execute("update link set " +
                        "title=@Title," +
                        "url=@Url," +
                        "icon=@Icon," +
                        "catid=@Catid," +
                        "linktype=@LinkType," +
                        "brief=@Brief where id=@Id", link);
                }
                else
                {
                    link.Id = Guid.NewGuid().ToString();
                    result = conn.Execute("INSERT INTO link(id,icon,linktype,catid,title,url,brief)" +
                        "values(@Id,@Icon,@LinkType,@Catid,@Title,@Url,@Brief)", link);
                }
            }

            return result;
        }

        public int DeleteLink(string id)
        {
            int result = 0;
            using (IDbConnection conn = SqlHelpers.CreateDbConnection(_connection))
            {
                result = conn.Execute("delete from link where Id = @Id", new { Id = id });
            }

            return result;
        }

        public IEnumerable<LinkCat> LinkCatList()
        {
            IEnumerable<LinkCat> list;
            using (IDbConnection conn = SqlHelpers.CreateDbConnection(_connection))
            {
                string sql = string.Format($"SELECT id,catname FROM linkcat ORDER BY id desc");
                list = conn.Query<LinkCat>(sql);
            }

            return list;
        }

        public LinkCat GetLinkCat(string id)
        {
            var linkCat = new LinkCat();
            using (IDbConnection conn = SqlHelpers.CreateDbConnection(_connection))
            {
                string sql = string.Format("SELECT * FROM linkcat WHERE Id = @Id");
                linkCat = conn.QueryFirstOrDefault<LinkCat>(sql, new { Id = id });
            }

            return linkCat;
        }
    }
}