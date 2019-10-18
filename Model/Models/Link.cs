using System;

namespace ASY.Hrefs.Model.Models
{
    public class Link
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Icon { get; set; }
        public string Catid { get; set; }
        public string LinkType { get; set; }
        public string Visited { get; set; }
        public string Brief { get; set; }
        public string Url { get; set; }
        public DateTime CreateTime { get; set; }
    }

    public class LinkCat
    {
        public string Id { get; set; }
        public string Catname { get; set; }
    }
}