using ASY.Hrefs.Model.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ASY.Hrefs.BLL.IService
{
    public interface IArticleService
    {
        int Save(Article item);
    }
}