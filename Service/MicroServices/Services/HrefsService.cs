using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASY.Hrefs.DAL.IRepository;
using Grpc.Core;
using Microsoft.Extensions.Logging;

namespace MicroServices
{
    public class HrefsService : Hrefs.HrefsBase
    {
        private IArticleRepository _articleRepository;
        private ILinkRepository _linkRepository;
        private readonly ILogger<HrefsService> _logger;
        public HrefsService(ILogger<HrefsService> logger, IArticleRepository articleRepository, ILinkRepository linkRepository)
        {
            _articleRepository = articleRepository;
            _linkRepository = linkRepository;
            _logger = logger;
        }

        public override Task<LinkListResponse> GetAllLink(Empty request, ServerCallContext context)
        {
            var result = _linkRepository.GetAllLink();
            return Task.FromResult(new LinkListResponse());
        }

        public override Task<LinkListResponse> ListLinkByCat(GlobalRequest request, ServerCallContext context)
        {
            var result = _linkRepository.ListLinkByCat(request.Linktype);
            return Task.FromResult(new LinkListResponse());
        }

        public override Task<GlobalResponse> LinksVisitedCount(Empty request, ServerCallContext context)
        {
            var result = _linkRepository.LinksVisitedCount();
            return Task.FromResult(new GlobalResponse());
        }

        public override Task<ArticleListResponse> ListArticleByPaging(GlobalRequest request, ServerCallContext context)
        {
            var result = _articleRepository.ListArticleByPaging(request.Size, request.Skip, request.Fields);
            return Task.FromResult(new ArticleListResponse());
        }

        public override Task<ArticleProto> GetArticle(GlobalRequest request, ServerCallContext context)
        {
            var result = _articleRepository.GetArticle(request.Id, request.Fields);
            return Task.FromResult(new ArticleProto());
        }
    }
}