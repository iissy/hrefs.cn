using ASY.Hrefs.BLL.Dispatcher;
using ASY.Hrefs.BLL.IService;
using ASY.Hrefs.Model.Models;
using MicroServices;
using System.Collections.Generic;

namespace ASY.Hrefs.BLL.Service
{
    public class LinkService : ILinkService
    {
        private MicroServices.Hrefs.HrefsClient _client;
        public LinkService()
        {
            _client = HrefsDispatcher.Instance();
        }

        public IEnumerable<Link> GetAllLink()
        {
            var reply = _client.GetAllLink(new Empty());

            return new List<Link>();
        }

        public int LinksVisitedCount()
        {
            var reply = _client.LinksVisitedCount(new Empty());

            return reply.Result;
        }

        public IEnumerable<Link> ListLinkByCat(string linktype)
        {
            var reply = _client.ListLinkByCat(new GlobalRequest { Linktype = linktype });

            return new List<Link>();
        }
    }
}