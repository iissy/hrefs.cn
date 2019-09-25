using ASY.Hrefs.BLL.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace hrefs.cn.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private IArticleService _articleService;
        public HomeController(ILogger<HomeController> logger, IArticleService articleService)
        {
            _articleService = articleService;
            _logger = logger;
        }

        public IActionResult Index()
        {
            var result = _articleService.ListArticleByPaging(10, 0, "id,title");
            return View();
        }
    }
}