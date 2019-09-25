using ASY.Hrefs.Model.Models;
using System.Collections.Generic;

namespace ASY.Hrefs.DAL.IRepository
{
    public interface ILinkRepository
    {
        IEnumerable<Link> GetAllLink();
        int LinksVisitedCount();
        IEnumerable<Link> ListLinkByCat(string linktype);
    }
}