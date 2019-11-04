using ASY.Hrefs.Model.Models;
using System.Collections.Generic;

namespace ASY.Hrefs.DAL.IRepository
{
    public interface ICusLinkRepository
    {
        int SaveCusLink(CusLink cusLink);
        int DeleteCusLink(string id);
        CusLink GetCusLink(string id, string fields = "*");
        IEnumerable<CusLink> PagerCusLinkList(int size, int offset, string catid, string title, string url, out int total);
        IEnumerable<CusLink> GetTopCusLink(int size, string fields = "*");
        IEnumerable<CusLink> ListCusLinkByPaging(int size, int skip, string fields = "*");
        int UpdatedCusLinkVisited(string id);
    }
}