using ASY.Hrefs.BLL.IService;
using ASY.Hrefs.Model.Models;
using ASY.Hrefs.Util.UIHelpers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;

namespace hrefs.cn.Controllers
{
    [Authorize]
    public class MainController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private IWebHostEnvironment _env;
        private IArticleService _articleService;
        private ILinkService _linkService;
        private IAccountService _accountService;
        public MainController(ILogger<HomeController> logger, IWebHostEnvironment env, IArticleService articleService, ILinkService linkService, IAccountService accountService)
        {
            _articleService = articleService;
            _linkService = linkService;
            _accountService = accountService;
            _logger = logger;
            _env = env;
        }

        [AllowAnonymous]
        [Route("login")]
        public IActionResult Login()
        {
            return View("Index");
        }

        [Route("main/{**path}", Name ="main")]
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
            int total;
            var list = _articleService.PagerArticleList(size, (pageno - 1) * size, id, title, catalog, out total, "id,title,origin,visited,catalog,createTime");
            return Json(new { total, list });
        }

        [Route("article/get/{id}")]
        public JsonResult GetArticle(string id)
        {
            var result = _articleService.GetArticle(id);
            return Json(result);
        }

        [Route("article/delete/{id}")]
        public JsonResult DeleteArticle(string id)
        {
            int result = _articleService.DeleteArticle(id);
            return Json(new { result });
        }

        [HttpPost]
        [Route("link/save")]
        public JsonResult SaveLink(Link link)
        {
            int result = _linkService.SaveLink(link);
            return Json(new { result });
        }

        [Route("link/list/{size}/{pageno}")]
        public JsonResult List(int size, int pageno, string linktype, string title, string url)
        {
            int total;
            var list = _linkService.PagerLinkList(size, (pageno - 1) * size, linktype, title, url, out total);
            return Json(new { total, list });
        }

        [Route("link/get/{id}")]
        public JsonResult GetLink(string id)
        {
            var result = _linkService.GetLink(id);
            return Json(result);
        }

        [Route("link/delete/{id}")]
        public JsonResult DeleteLink(string id)
        {
            int result = _linkService.DeleteLink(id);
            return Json(new { result });
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("login")]
        public async Task<IActionResult> Login(string UserId, string Password)
        {
            Password = MD5Helpers.ComputeHash(Password);
            Account account = _accountService.GetLogin(UserId, Password);
            if (account?.Id > 0 && account?.Status == 0)
            {
                ClaimsIdentity identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                identity.AddClaim(new Claim(ClaimTypes.Email, account.UserId, ClaimValueTypes.String));
                identity.AddClaim(new Claim(ClaimTypes.Name, account.UserName, ClaimValueTypes.String));
                await HttpContext.SignInAsync(new ClaimsPrincipal(identity), new AuthenticationProperties { ExpiresUtc = DateTime.UtcNow.AddHours(24) });

                return RedirectToRoute("main");
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        [Route("upload")]
        public JsonResult Upload()
        {
            IFormFile file = Request.Form.Files[0];
            Uploader uploader = new Uploader(_env.WebRootPath, file.FileName, file.Length, file.ContentType);
            if (string.IsNullOrWhiteSpace(uploader.FilePath) || string.IsNullOrWhiteSpace(uploader.FileUrl))
            {
                return Json(new { status = false, url = uploader.Message });
            }
            else
            {
                Stream sm = file.OpenReadStream();
                byte[] bytes = new byte[sm.Length];
                sm.Read(bytes, 0, bytes.Length);

                using (FileStream fs = System.IO.File.Create(uploader.FilePath))
                {
                    using (BinaryWriter bw = new BinaryWriter(fs))
                    {
                        bw.Write(bytes);
                    }
                }

                return Json(new { status = uploader.Status, url = uploader.FileUrl, path = uploader.FilePath, ok = uploader.Status, data = uploader.FileUrl, msg = uploader.Message });
            }
        }
    }
}