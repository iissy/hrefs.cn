using ASY.Hrefs.Model.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ASY.Hrefs.BLL.IService
{
    public interface ILinkService
    {
        IEnumerable<Link> GetAllLink();
        int LinksVisitedCount();
        IEnumerable<Link> ListLinkByCat(string linktype);
    }
}