using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using ASY.Hrefs.BLL.IService;
using ASY.Hrefs.Model.Models;
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
            var result = _articleService.Save(new Article() { Id = "min" });
            return View();
        }
    }
}