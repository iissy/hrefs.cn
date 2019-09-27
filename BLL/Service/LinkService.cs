using ASY.Hrefs.BLL.Dispatcher;
using ASY.Hrefs.BLL.IService;
using ASY.Hrefs.Model.Models;
using ASY.Hrefs.Util.UIHelpers;
using AutoMapper;
using MicroServices;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;

namespace ASY.Hrefs.BLL.Service
{
    public class LinkService : ILinkService
    {
        private IMapper _mapper;
        private MicroServices.Hrefs.HrefsClient _client;
        public LinkService(IOptions<RemoteService> remote)
        {
            var configuration = Mapping.GetMapperConfiguration();
            _mapper = configuration.CreateMapper();
            _client = HrefsDispatcher.Instance(remote.Value.Url);
        }

        public int DeleteLink(string id)
        {
            var reply = _client.DeleteLink(new GlobalRequest { Id = id });
            return reply.Result;
        }

        public IEnumerable<Link> GetAllLink()
        {
            var result = _client.GetAllLink(new Empty());
            var list = result.Items.Select(p => _mapper.Map<Link>(p));
            return list;
        }

        public Link GetLink(string id, string fields = "*")
        {
            var result = _client.GetLink(new GlobalRequest { Id = id, Fields = fields });
            return _mapper.Map<Link>(result);
        }

        public int LinksVisitedCount()
        {
            var reply = _client.LinksVisitedCount(new Empty());
            return reply.Result;
        }

        public IEnumerable<Link> ListLinkByCat(string linktype)
        {
            var result = _client.ListLinkByCat(new GlobalRequest { Linktype = linktype });
            var list = result.Items.Select(p => _mapper.Map<Link>(p));
            return list;
        }

        public IEnumerable<Link> PagerLinkList(int size, int offset, string linktype, string title, string url, out int total)
        {
            var result = _client.PagerLinkList(new GlobalRequest { Size = size, Offset = offset, Linktype = linktype, Title = title, Url = url });
            var list = result.Items.Select(p => _mapper.Map<Link>(p));
            total = result.Total;
            return list;
        }

        public int SaveLink(Link link)
        {
            var item = _mapper.Map<LinkProto>(link);
            var result = _client.SaveLink(item);
            return result.Result;
        }

        public int UpdatedLinkVisited(string id)
        {
            var result = _client.UpdatedLinkVisited(new GlobalRequest { Id = id });
            return result.Result;
        }
    }
}