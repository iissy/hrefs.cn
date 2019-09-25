using ASY.Hrefs.Model.Models;
using System.Collections.Generic;

namespace ASY.Hrefs.DAL.IRepository
{
    public interface IArticleRepository
    {
        IEnumerable<Article> ListByPaging(int size, int skip, string fields = "*");
    }
}