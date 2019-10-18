using ASY.Hrefs.Model.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ASY.Hrefs.BLL.IService
{
    public interface ILinkService
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