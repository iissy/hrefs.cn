using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASY.Hrefs.DAL.IRepository;
using ASY.Hrefs.Model.Models;
using AutoMapper;
using Grpc.Core;
using Microsoft.Extensions.Logging;

namespace MicroServices
{
    public class HrefsService : Hrefs.HrefsBase
    {
        private IArticleRepository _articleRepository;
        private ILinkRepository _linkRepository;
        private readonly ILogger<HrefsService> _logger;
        private IMapper _mapper;
        public HrefsService(ILogger<HrefsService> logger, IArticleRepository articleRepository, ILinkRepository linkRepository)
        {
            _articleRepository = articleRepository;
            _linkRepository = linkRepository;
            _logger = logger;

            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Article, ArticleProto>(MemberList.None).ForAllMembers(opt => opt.Condition((source, destination, sourceMember, destMember) => sourceMember != null));
                cfg.CreateMap<Link, LinkProto>(MemberList.None).ForAllMembers(opt => opt.Condition((source, destination, sourceMember, destMember) => sourceMember != null));
            });
            configuration.AssertConfigurationIsValid();
            _mapper = configuration.CreateMapper();
        }

        public override Task<LinkListResponse> GetAllLink(Empty request, ServerCallContext context)
        {
            var result = _linkRepository.GetAllLink();
            LinkListResponse response = new LinkListResponse();
            foreach(var item in result)
            {
                response.Items.Add(_mapper.Map<LinkProto>(item));
            }

            return Task.FromResult(response);
        }

        public override Task<LinkListResponse> ListLinkByCat(GlobalRequest request, ServerCallContext context)
        {
            var result = _linkRepository.ListLinkByCat(request.Linktype);
            var response = new LinkListResponse();
            response.Items.AddRange(result.Select(p => _mapper.Map<LinkProto>(p)));
            return Task.FromResult(response);
        }

        public override Task<GlobalResponse> LinksVisitedCount(Empty request, ServerCallContext context)
        {
            var result = _linkRepository.LinksVisitedCount();
            return Task.FromResult(new GlobalResponse { Result = result });
        }

        public override Task<ArticleListResponse> ListArticleByPaging(GlobalRequest request, ServerCallContext context)
        {
            var result = _articleRepository.ListArticleByPaging(request.Size, request.Skip, request.Fields);
            var response = new ArticleListResponse();
            foreach(var item in result)
            {
                response.Items.Add(_mapper.Map<ArticleProto>(item));
            }

            return Task.FromResult(response);
        }

        public override Task<ArticleProto> GetArticle(GlobalRequest request, ServerCallContext context)
        {
            var result = _articleRepository.GetArticle(request.Id, request.Fields);
            var response = new ArticleProto();
            return Task.FromResult(_mapper.Map<ArticleProto>(result));
        }
    }
}