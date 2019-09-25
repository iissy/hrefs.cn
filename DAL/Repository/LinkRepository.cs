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
    }
}