using ASY.Hrefs.BLL.Dispatcher;
using ASY.Hrefs.BLL.IService;
using ASY.Hrefs.Model.Models;
using MicroServices;
using System.Collections.Generic;

namespace ASY.Hrefs.BLL.Service
{
    public class ArticleService: IArticleService
    {
        public IEnumerable<Article> ListArticleByPaging(int size, int skip, string fields = "*")
        {
            var client = HrefsDispatcher.Instance();
            var result = client.ListArticleByPaging(new GlobalRequest { Size = size, Skip = size, Fields = fields });

            return new List<Article>();
        }
    }
}