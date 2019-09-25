using ASY.Hrefs.BLL.Dispatcher;
using ASY.Hrefs.BLL.IService;
using ASY.Hrefs.Model.Models;
using MicroServices;
using System.Collections.Generic;

namespace ASY.Hrefs.BLL.Service
{
    public class ArticleService: IArticleService
    {
        private MicroServices.Hrefs.HrefsClient _client;
        public ArticleService()
        {
            _client = HrefsDispatcher.Instance();
        }

        public Article GetArticle(string id, string fields = "*")
        {
            var result = _client.ListArticleByPaging(new GlobalRequest { Id = id, Fields = fields });

            return new Article();
        }

        public IEnumerable<Article> ListArticleByPaging(int size, int skip, string fields = "*")
        {
            var result = _client.ListArticleByPaging(new GlobalRequest { Size = size, Skip = size, Fields = fields });

            return new List<Article>();
        }
    }
}