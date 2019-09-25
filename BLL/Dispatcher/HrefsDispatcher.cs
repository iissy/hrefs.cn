using Grpc.Net.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace ASY.Hrefs.BLL.Dispatcher
{
    public static class HrefsDispatcher
    {
        public static MicroServices.Hrefs.HrefsClient Instance(string url)
        {
            var channel = GrpcChannel.ForAddress(url);
            var client = new MicroServices.Hrefs.HrefsClient(channel);
            return client;
        }
    }
}