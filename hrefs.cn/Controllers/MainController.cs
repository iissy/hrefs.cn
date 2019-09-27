using ASY.Hrefs.BLL.IService;
using ASY.Hrefs.Model.Models;
using ASY.Hrefs.Util.UIHelpers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace hrefs.cn.Controllers
{
    [Authorize]
    public class MainController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private IArticleService _articleService;
        private ILinkService _linkService;
        private IAccountService _accountService;
        public MainController(ILogger<HomeController> logger, IArticleService articleService, ILinkService linkService, IAccountService accountService)
        {
            _articleService = articleService;
            _linkService = linkService;
            _accountService = accountService;
            _logger = logger;
        }

        [AllowAnonymous]
        [Route("main/login")]
        public IActionResult Login()
        {
            return View("Index");
        }

        [Route("main/{**path}")]
        public IActionResult Index()
        {
            return View();
        }

        [Route("account/list/{size}/{pageno}")]
        public JsonResult PagerAccountList(int size, int pageno)
        {
            int total;
            var list = _accountService.PagerAccountList(size, (pageno - 1) * size, null, out total, "*");
            return Json(new { total, list });
        }

        [HttpPost]
        [Route("article/save")]
        public JsonResult SaveArticle(Article Article)
        {
            int result = _articleService.SaveArticle(Article);
            return Json(new { result });
        }

        [Route("article/list/{size}/{pageno}")]
        public JsonResult PagerArticleList(int size, int pageno, string id, string title, string catalog)
        {
            int total = 0;
            var list = _articleService.PagerArticleList(size, (pageno - 1) * size, id, title, catalog, out total, "id,title,origin,visited,catalog,createTime");
            return Json(new { total, list });
        }

        [Route("article/get")]
        public JsonResult GetArticle(string id)
        {
            var result = _articleService.GetArticle(id);
            return Json(result);
        }

        [Route("article/delete")]
        public JsonResult DeleteArticle(string id)
        {
            int result = _articleService.DeleteArticle(id);
            return Json(new { result = result });
        }

        [HttpPost]
        [Route("link/save")]
        public JsonResult SaveLink(Link link)
        {
            int result = _linkService.SaveLink(link);
            return Json(new { result });
        }

        [Route("link/list/{size}/{pageno}")]
        public JsonResult List(string linktype, string title, string url, int size, int pageno)
        {
            int total = 0;
            var list = _linkService.PagerLinkList(size, (pageno - 1) * size, linktype, title, url, out total);
            return Json(new { total, list });
        }

        [Route("link/get")]
        public JsonResult GetLink(string id)
        {
            var result = _linkService.GetLink(id);
            return Json(result);
        }

        [Route("link/delete")]
        public JsonResult DeleteLink(string id)
        {
            int result = _linkService.DeleteLink(id);
            return Json(new { result = result });
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("login")]
        public async Task<JsonResult> Login(string UserId, string Password)
        {
            Password = MD5Helpers.ComputeHash(Password);
            Account account = _accountService.GetLogin(UserId, Password);
            if (account?.Id > 0 && account?.Status == 0)
            {
                ClaimsIdentity identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                identity.AddClaim(new Claim(ClaimTypes.Email, account.UserId, ClaimValueTypes.String));
                identity.AddClaim(new Claim(ClaimTypes.Name, account.UserName, ClaimValueTypes.String));
                await HttpContext.SignInAsync(new ClaimsPrincipal(identity), new AuthenticationProperties { ExpiresUtc = DateTime.UtcNow.AddHours(24) });
                return Json(new { result = account.Id });
            }
            else
            {
                return Json(new { result = 0 });
            }
        }
    }
}