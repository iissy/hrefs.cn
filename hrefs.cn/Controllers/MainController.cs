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
using System.Net.Mail;
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
        private ICusLinkService _cusLinkService;
        public MainController(ILogger<HomeController> logger, IWebHostEnvironment env, IArticleService articleService, ILinkService linkService, IAccountService accountService, ICusLinkService cusLinkService)
        {
            _articleService = articleService;
            _linkService = linkService;
            _accountService = accountService;
            _cusLinkService = cusLinkService;
            _logger = logger;
            _env = env;
        }

        [AllowAnonymous]
        [Route("login")]
        public IActionResult Login()
        {
            return View("Index");
        }

        [AllowAnonymous]
        [Route("sendemail")]
        public IActionResult SendEmail()
        {
            return View("Index");
        }

        [AllowAnonymous]
        [Route("sendemail")]
        [HttpPost]
        public JsonResult SendEmail(string sto, string sToSubject)
        {
            try
            {
                string sSmtp = "smtp.qq.com";
                string sPort = "25";
                string sFrom = "pinbor@iissy.com";
                string sAccount = "pinbor@iissy.com";
                string sPass = "********";

                SmtpClient client = new SmtpClient();
                client.Host = sSmtp;
                client.UseDefaultCredentials = false;
                client.Port = Convert.ToInt16(sPort);
                client.Credentials = new System.Net.NetworkCredential(sAccount, sPass);

                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                MailMessage message = new MailMessage(sFrom, sto);
                message.Subject = sToSubject;
                message.Body = "......";
                message.BodyEncoding = System.Text.Encoding.UTF8;
                message.IsBodyHtml = true;
                message.Headers.Add("Return-Path", "web@reasonables.com");
                message.From = new MailAddress("johnsmith@reasonables.com", "John Smith");
                message.Sender = new MailAddress("pinbor@iissy.com", "何敏");
                message.ReplyToList.Add("web@reasonables.com");
                client.Send(message);
            }
            catch
            {
                return Json(new { status = false });
            }

            return Json(new { status = true });
        }

        [AllowAnonymous]
        [Route("editdistance")]
        public IActionResult EditDistance()
        {
            return View("Index");
        }

        [AllowAnonymous]
        [Route("editdistance1")]
        [HttpGet]
        public JsonResult EditDistance1()
        {
            int result = -1;
            try
            {
                var file1 = FileToBinary(_env.WebRootPath + "/favicon1.png");
                var file2 = FileToBinary(_env.WebRootPath + "/favicon2.png");

                result = Levenshtein.CalculateDistance(file1, file2, 1);    
            }
            catch
            {
                return Json(new { result });
            }

            return Json(new { result });
        }

        [AllowAnonymous]
        [Route("editdistance2")]
        [HttpPost]
        public JsonResult EditDistance2(string str1, string str2)
        {
            int result = -1;
            try
            {
                result = Levenshtein.CalculateDistance(str1, str2, 1);
            }
            catch
            {
                return Json(new { result });
            }

            return Json(new { result });
        }

        public string FileToBinary(string FilePath)
        {
            FileStream fs = new FileStream(FilePath, FileMode.Open, FileAccess.Read);
            int fileLength = Convert.ToInt32(fs.Length);
            byte[] fileByteArray = new byte[fileLength];
            BinaryReader br = new BinaryReader(fs);
            for (int i = 0; i < fileLength; i++)
            {
                br.Read(fileByteArray, 0, fileLength);
            }
            string strData = Convert.ToBase64String(fileByteArray);
            return strData;
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
            var linkCat = _linkService.GetLinkCat(link.Catid);
            if(linkCat == null)
            {
                return Json(new { result = 0 });
            }
            else
            {
                link.LinkType = linkCat.Catname;
                int result = _linkService.SaveLink(link);
                return Json(new { result });
            }
        }

        [Route("link/list/{size}/{pageno}")]
        public JsonResult PagerLinkList(int size, int pageno, string catid, string title, string url)
        {
            int total;
            var list = _linkService.PagerLinkList(size, (pageno - 1) * size, catid, title, url, out total);
            return Json(new { total, list });
        }

        [Route("link/cat/list")]
        public JsonResult LinkCatList()
        {
            var list = _linkService.LinkCatList();
            return Json(list);
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

        [HttpPost]
        [Route("cuslink/save")]
        public JsonResult SaveCusLink(CusLink cusLink)
        {
            var linkCat = _linkService.GetLinkCat(cusLink.Catid);
            if (linkCat == null)
            {
                return Json(new { result = 0 });
            }
            else
            {
                cusLink.LinkType = linkCat.Catname;
                int result = _cusLinkService.SaveCusLink(cusLink);
                return Json(new { result });
            }            
        }

        [Route("cuslink/list/{size}/{pageno}")]
        public JsonResult PagerCusLinkList(int size, int pageno, string catid, string title, string url)
        {
            int total;
            var list = _cusLinkService.PagerCusLinkList(size, (pageno - 1) * size, catid, title, url, out total);
            return Json(new { total, list });
        }

        [Route("cuslink/get/{id}")]
        public JsonResult GetCusLink(string id)
        {
            var result = _cusLinkService.GetCusLink(id);
            return Json(result);
        }

        [Route("cuslink/delete/{id}")]
        public JsonResult DeleteCusLink(string id)
        {
            int result = _cusLinkService.DeleteCusLink(id);
            return Json(new { result });
        }

        [Route("cuslink/top")]
        public JsonResult GetTopCusLink(int size)
        {
            var list = _cusLinkService.GetTopCusLink(size, "*");
            return Json(list);
        }
    }
}