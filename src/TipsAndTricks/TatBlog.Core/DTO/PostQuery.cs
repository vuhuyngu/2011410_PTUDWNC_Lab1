using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TatBlog.Core.Contracts;

namespace TatBlog.Core.DTO
{
    public class PostQuery : IPostQuery
    {
        public int AuthorId { get; set; }
        public int CategoryId { get; set; }

        public string CategorySlug { get; set; }
        public string AuthorSlug { get; set; }
        public string TagSlug { get; set; }
        /*public PostQuery() { }*/

        public int PostedYear { get; set; }
        public int PostedMonth { get; set; }

        public bool PublishedOnly { get; set; }
        public bool NotPublished { get; set; }
        public string Keyword { get; set; }

        public int Year { get; set; }
        public int Month { get; set; }
        /*public int Day { get; set; }
        public int Hour { get; set; }
        public int Minute { get; set; }
        public int Second { get; set; }*/

        public string Tag { get; set; }
        public string TitleSlug { get; set; }
    }
}
