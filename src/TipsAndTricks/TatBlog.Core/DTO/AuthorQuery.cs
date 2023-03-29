using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TatBlog.Core.Contracts;

namespace TatBlog.Core.DTO
{
    public class AuthorQuery : IAuthorQuery
    {
        public string Keyword { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Author { get; set; }

        public int Month { get; set; }

        public int Year { get; set; }

        public int Day { get; set; }
    }
}
