using ASY.Hrefs.Model.Models;
using System.Collections.Generic;

namespace ASY.Hrefs.BLL.IService
{
    public interface ICusLinkService
    {
        int SaveCusLink(CusLink cusLink);
        int DeleteCusLink(string id);
        CusLink GetCusLink(string id, string fields = "*");
        IEnumerable<CusLink> PagerCusLinkList(int size, int offset, string pcatid, string title, string url, out int total);
        IEnumerable<CusLink> GetTopCusLink(int size, string fields = "*");
    }
}