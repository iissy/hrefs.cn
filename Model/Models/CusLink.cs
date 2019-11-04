using System;

namespace ASY.Hrefs.Model.Models
{
    public class CusLink
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public int Status { get; set; }
        public string LinkType { get; set; }
        public string Catid { get; set; }
        public DateTime Adddate { get; set; }
        public DateTime? Updatedate { get; set; }
        public string Visited { get; set; }
    }
}