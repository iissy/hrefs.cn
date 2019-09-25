using ASY.Hrefs.Model.Models;
using System.Collections.Generic;

namespace ASY.Hrefs.BLL.IService
{
    public interface IArticleService
    {
        IEnumerable<Article> ListArticleByPaging(int size, int skip, string fields = "*");
        Article GetArticle(string id, string fields = "*");
    }
}