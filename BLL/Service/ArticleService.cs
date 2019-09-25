using ASY.Hrefs.BLL.Dispatcher;
using ASY.Hrefs.BLL.IService;
using ASY.Hrefs.Model.Models;
using MicroServices;
using System;
using System.Collections.Generic;
using System.Text;

namespace ASY.Hrefs.BLL.Service
{
    public class ArticleService: IArticleService
    {
        public int Save(Article article)
        {
            var client = HrefsDispatcher.Instance();
            var reply = client.SayHello(new HelloRequest { Name = "GreeterClient" });
            Console.WriteLine("Greeting: " + reply.Message);
            return 1;
        }
    }
}