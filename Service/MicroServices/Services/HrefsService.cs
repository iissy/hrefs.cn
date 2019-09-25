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
        public HrefsService(ILogger<HrefsService> logger)
        {
            _logger = logger;
        }

        public override Task<LinkListResponse> GetAllLink(Empty request, ServerCallContext context)
        {
            var result = _linkRepository.GetAllLink();
            return Task.FromResult(new LinkListResponse());
        }

        public override Task<ArticleListResponse> ListArticleByPaging(GlobalRequest request, ServerCallContext context)
        {
            var result = _articleRepository.ListByPaging(request.Size, request.Skip, request.Fields);
            return Task.FromResult(new ArticleListResponse());
        }
    }
}