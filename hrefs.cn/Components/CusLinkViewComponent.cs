using ASY.Hrefs.BLL.IService;
using ASY.Hrefs.Model.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace hrefs.cn.Components
{
    [ViewComponent(Name = "CusLink")]
    public class CusLinkViewComponent : ViewComponent
    {
        private ICusLinkService _cusLinkService;
        public CusLinkViewComponent(ICusLinkService cusLinkService)
        {
            _cusLinkService = cusLinkService;
        }

        public async Task<IViewComponentResult> InvokeAsync(int size)
        {
            var items = await GetItemsAsync(size);
            return View(items);
        }

        private Task<IEnumerable<CusLink>> GetItemsAsync(int size)
        {
            var list = _cusLinkService.GetTopCusLink(size, "id,title,url,linktype");
            return Task<IEnumerable<CusLink>>.Factory.StartNew(() => { return list; });
        }
    }
}