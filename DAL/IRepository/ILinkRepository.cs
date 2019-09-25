using ASY.Hrefs.Model.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ASY.Hrefs.DAL.IRepository
{
    public interface ILinkRepository
    {
        IEnumerable<Link> GetAllLink();
    }
}