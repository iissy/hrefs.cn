using ASY.Hrefs.BLL.IService;
using ASY.Hrefs.Model.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace hrefs.cn.Components
{
    [ViewComponent(Name = "Article")]
    public class ArticleViewComponent : ViewComponent
    {
        private IArticleService _articleService;
        public ArticleViewComponent(IArticleService articleService)
        {
            _articleService = articleService;
        }

        public async Task<IViewComponentResult> InvokeAsync(int size, int skip)
        {
            var items = await GetItemsAsync(size, skip);
            return View(items);
        }

        private Task<IEnumerable<Article>> GetItemsAsync(int size, int skip)
        {
            var list = _articleService.ListArticleByPaging(size, skip, "id,title");
            return Task<IEnumerable<Article>>.Factory.StartNew(() => { return list; });
        }
    }
}