using ASY.Hrefs.BLL.IService;
using ASY.Hrefs.Model.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

namespace hrefs.cn.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private IArticleService _articleService;
        private ILinkService _linkService;
        private ICusLinkService _cusLinkService;
        public HomeController(ILogger<HomeController> logger, IArticleService articleService, ILinkService linkService, ICusLinkService cusLinkService)
        {
            _articleService = articleService;
            _linkService = linkService;
            _cusLinkService = cusLinkService;
            _logger = logger;
        }

        [Route("articles/{pageno:int=1}")]
        public IActionResult Articles(int pageno)
        {
            pageno = pageno <= 0 ? 1 : (pageno > 10 ? 10 : pageno);
            ViewBag.PageNumber = pageno;
            int size = 200;
            int offset = size * (pageno - 1);
            return View(_articleService.ListArticleByPaging(size, offset, "id,title,icon,brief,createTime"));
        }

        [Route("cuslinks/{pageno:int=1}")]
        public IActionResult CusLinks(int pageno)
        {
            pageno = pageno <= 0 ? 1 : (pageno > 10 ? 10 : pageno);
            ViewBag.PageNumber = pageno;
            int size = 200;
            int offset = size * (pageno - 1);
            return View(_cusLinkService.ListCusLinkByPaging(size, offset, "id,title,url,linktype"));
        }

        [Route("links/{catid}")]
        public IActionResult Links(string catid)
        {
            var list = _linkService.ListLinkByCat(catid);
            if(list.Any())
            {
                ViewBag.Cat = list.FirstOrDefault().LinkType;
                return View(list);
            }
            else
            {
                return NotFound();
            }
        }

        [Route("")]
        public IActionResult Index()
        {
            string backend = "NodeJS,PHP,DotNet,Golang,Java,Python";
            string frontend = "CSS,JQuery,Charts,Vue,前端框架,富文本编辑器,打包构建";
            string hot = "架构师,人工智能,区块链,大数据,数据库,运维工具";
            string other = "实用工具,其他,软件,协同工具,教程";

            var sites = _linkService.GetAllLink();
            var backendGroups = sites.Where(p => backend.Split(',').Contains(p.LinkType)).GroupBy(p => p.LinkType);
            var backendResult = from g in backendGroups
                                orderby g.Count() descending
                                select new KeyValuePair<string, IGrouping<string, Link>>(g.Key, g);

            var frontendGroups = sites.Where(p => frontend.Split(',').Contains(p.LinkType)).GroupBy(p => p.LinkType);
            var frontendResult = from g in frontendGroups
                                 orderby g.Count() descending
                                 select new KeyValuePair<string, IGrouping<string, Link>>(g.Key, g);

            var hotGroups = sites.Where(p => hot.Split(',').Contains(p.LinkType)).GroupBy(p => p.LinkType);
            var hotResult = from g in hotGroups
                            orderby g.Count() descending
                            select new KeyValuePair<string, IGrouping<string, Link>>(g.Key, g);

            var otherGroups = sites.Where(p => other.Split(',').Contains(p.LinkType)).GroupBy(p => p.LinkType);
            var otherResult = from g in otherGroups
                              orderby g.Count() descending
                              select new KeyValuePair<string, IGrouping<string, Link>>(g.Key, g);

            ViewBag.backend = backendResult;
            ViewBag.frontend = frontendResult;
            ViewBag.hot = hotResult;
            ViewBag.other = otherResult;
            ViewBag.common = sites.Where(p => p.LinkType == "公共");
            ViewBag.info = sites.Where(p => p.LinkType == "资讯");

            return View();
        }

        [Route("article/{id}")]
        public IActionResult Detail(string id)
        {
            _articleService.UpdatedArticleVisited(id);
            Article article = _articleService.GetArticle(id, "id,title,brief,body,createtime");
            return View(article);
        }

        [Route("link/{id}")]
        public void RedirectUrl(string id)
        {
            _linkService.UpdatedLinkVisited(id);
            var url = _linkService.GetLink(id, "url").Url;
            Response.Redirect(url);
        }

        [Route("cuslink/{id}")]
        public void RedirectCusUrl(string id)
        {
            _cusLinkService.UpdatedCusLinkVisited(id);
            var url = _cusLinkService.GetCusLink(id, "url").Url;
            Response.Redirect(url);
        }

        [Route("payme")]
        public IActionResult Payme()
        {
            return View(_linkService.LinksVisitedCount());
        }
    }
}