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
    public class CusLinkService : ICusLinkService
    {
        private IMapper _mapper;
        private MicroServices.Hrefs.HrefsClient _client;
        public CusLinkService(IOptions<RemoteService> remote)
        {
            var configuration = Mapping.GetMapperConfiguration();
            _mapper = configuration.CreateMapper();
            _client = HrefsDispatcher.Instance(remote.Value.Url);
        }

        public int DeleteCusLink(string id)
        {
            var reply = _client.DeleteCusLink(new GlobalRequest { Id = id });
            return reply.Result;
        }

        public CusLink GetCusLink(string id, string fields = "*")
        {
            var result = _client.GetCusLink(new GlobalRequest { Id = id, Fields = fields });
            return _mapper.Map<CusLink>(result);
        }

        public IEnumerable<CusLink> GetTopCusLink(int size, string fields = "*")
        {
            var result = _client.GetTopCusLink(new GlobalRequest { Size = size, Fields = fields });
            var list = result.Items.Select(p => _mapper.Map<CusLink>(p));
            return list;
        }

        public IEnumerable<CusLink> ListCusLinkByPaging(int size, int skip, string fields = "*")
        {
            var result = _client.ListCusLinkByPaging(new GlobalRequest { Size = size, Skip = skip, Fields = fields });
            var list = result.Items.Select(p => _mapper.Map<CusLink>(p));
            return list;
        }

        public IEnumerable<CusLink> PagerCusLinkList(int size, int offset, string catid, string title, string url, out int total)
        {
            var result = _client.PagerCusLinkList(new GlobalRequest { Size = size, Offset = offset, Catid = catid ?? "", Title = title ?? "", Url = url ?? "" });
            var list = result.Items.Select(p => _mapper.Map<CusLink>(p));
            total = result.Total;
            return list;
        }

        public int SaveCusLink(CusLink cusLink)
        {
            var item = _mapper.Map<CusLinkProto>(cusLink);
            var result = _client.SaveCusLink(item);
            return result.Result;
        }
    }
}