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
    public class AccountService : IAccountService
    {
        private IMapper _mapper;
        private MicroServices.Hrefs.HrefsClient _client;
        public AccountService(IOptions<RemoteService> remote)
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

        public int DeleteAccount(string id)
        {
            var reply = _client.DeleteAccount(new GlobalRequest { Id = id });
            return reply.Result;
        }

        public Account GetAccount(string id, string fields = "*")
        {
            var result = _client.GetAccount(new GlobalRequest { Id = id, Fields = fields });
            return _mapper.Map<Account>(result);
        }

        public Account GetLogin(string userid, string password, string fields = "*")
        {
            var result = _client.GetLogin(new GlobalRequest { Id = userid, Password = password, Fields = fields });
            return _mapper.Map<Account>(result);
        }

        public IEnumerable<Account> PagerAccountList(int size, int skip, string username, out int total, string fields = "*")
        {
            var result = _client.PagerAccountList(new GlobalRequest { Size = size, Skip = skip, Fields = fields });
            var list = result.Items.Select(p => _mapper.Map<Account>(p));
            total = result.Total;
            return list;
        }

        public int SaveAccount(Account account)
        {
            var item = _mapper.Map<AccountProto>(account);
            var result = _client.SaveAccount(item);
            return result.Result;
        }
    }
}