﻿using ASY.Hrefs.Model.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ASY.Hrefs.BLL.IService
{
    public interface IArticleService
    {
        IEnumerable<Article> ListArticleByPaging(int size, int skip, string fields = "*");
    }
}