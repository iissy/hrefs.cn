using System;

namespace ASY.Hrefs.Model.Models
{
    public class Article
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Visited { get; set; }
        public string Brief { get; set; }
        public string Icon { get; set; }
        public string Body { get; set; }
        public bool AddOrEdit { get; set; }
        public DateTime CreateTime { get; set; }
    }
}