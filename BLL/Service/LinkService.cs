using ASY.Hrefs.BLL.Dispatcher;
using ASY.Hrefs.BLL.IService;
using ASY.Hrefs.Model.Models;
using System;
using System.Collections.Generic;
using System.Text;
using MicroServices;

namespace ASY.Hrefs.BLL.Service
{
    public class LinkService : ILinkService
    {
        public IEnumerable<Link> GetAllLink()
        {
            var client = HrefsDispatcher.Instance();
            var reply = client.GetAllLink(new Empty());

            return new List<Link>();
        }
    }
}