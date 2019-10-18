using ASY.Hrefs.Model.Models;
using System.Collections.Generic;

namespace ASY.Hrefs.DAL.IRepository
{
    public interface ILinkRepository
    {
        int SaveLink(Link link);
        int DeleteLink(string id);
        IEnumerable<Link> GetAllLink();
        int LinksVisitedCount();
        IEnumerable<Link> ListLinkByCat(string catid);
        int UpdatedLinkVisited(string id);
        Link GetLink(string id, string fields = "*");
        IEnumerable<Link> PagerLinkList(int size, int offset, string catid, string title, string url, out int total);
        IEnumerable<LinkCat> LinkCatList();
    }
}