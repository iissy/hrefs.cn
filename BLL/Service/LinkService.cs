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
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ArticleProto, Article>(MemberList.None).ForAllMembers(opt => opt.Condition((source, destination, sourceMember, destMember) => sourceMember != null));
                cfg.CreateMap<LinkProto, Link>(MemberList.None).ForAllMembers(opt => opt.Condition((source, destination, sourceMember, destMember) => sourceMember != null));
            });
            configuration.AssertConfigurationIsValid();
            _mapper = configuration.CreateMapper();
            _client = HrefsDispatcher.Instance(remote.Value.Url);
        }

        public IEnumerable<Link> GetAllLink()
        {
            var result = _client.GetAllLink(new Empty());
            var list = result.Items.Select(p => _mapper.Map<Link>(p));
            return list;
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
    }
}