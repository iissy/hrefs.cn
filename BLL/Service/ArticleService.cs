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
    public class ArticleService: IArticleService
    {
        private IMapper _mapper;
        private MicroServices.Hrefs.HrefsClient _client;
        public ArticleService(IOptions<RemoteService> remote)
        {
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Article, ArticleProto>(MemberList.None).ForAllMembers(opt => opt.Condition((source, destination, sourceMember, destMember) => sourceMember != null));
                cfg.CreateMap<Link, LinkProto>(MemberList.None).ForAllMembers(opt => opt.Condition((source, destination, sourceMember, destMember) => sourceMember != null));
                cfg.CreateMap<Account, AccountProto>(MemberList.None).ForAllMembers(opt => opt.Condition((source, destination, sourceMember, destMember) => sourceMember != null));

                cfg.CreateMap<ArticleProto, Article>(MemberList.None).ForAllMembers(opt => opt.Condition((source, destination, sourceMember, destMember) => sourceMember != null));
                cfg.CreateMap<LinkProto, Link>(MemberList.None).ForAllMembers(opt => opt.Condition((source, destination, sourceMember, destMember) => sourceMember != null));
                cfg.CreateMap<AccountProto, Account>(MemberList.None).ForAllMembers(opt => opt.Condition((source, destination, sourceMember, destMember) => sourceMember != null));
            });
            configuration.AssertConfigurationIsValid();
            _mapper = configuration.CreateMapper();
            _client = HrefsDispatcher.Instance(remote.Value.Url);
        }

        public int DeleteArticle(string id)
        {
            var reply = _client.DeleteArticle(new GlobalRequest { Id = id });
            return reply.Result;
        }

        public Article GetArticle(string id, string fields = "*")
        {
            var result = _client.GetArticle(new GlobalRequest { Id = id, Fields = fields });
            return _mapper.Map<Article>(result);
        }

        public IEnumerable<Article> ListArticleByPaging(int size, int skip, string fields = "*")
        {
            var result = _client.ListArticleByPaging(new GlobalRequest { Size = size, Skip = skip, Fields = fields });
            var list = result.Items.Select(p => _mapper.Map<Article>(p));
            return list;
        }

        public IEnumerable<Article> PagerArticleList(int size, int skip, string id, string title, string catalog, out int total, string fields = "*")
        {
            var result = _client.PagerArticleList(new GlobalRequest { Size = size, Skip = skip, Title = title, Fields = fields });
            var list = result.Items.Select(p => _mapper.Map<Article>(p));
            total = result.Total;
            return list;
        }

        public int SaveArticle(Article article)
        {
            var item = _mapper.Map<ArticleProto>(article);
            var result = _client.SaveArticle(item);
            return result.Result;
        }
    }
}