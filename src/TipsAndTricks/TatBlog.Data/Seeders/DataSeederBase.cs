using TatBlog.Core.Entities;

namespace TatBlog.Data.Seeders
{
    public class DataSeederBase
    {

        private static IList<Post> AddPosts(
            IList<Author> authors,
            IList<Category> categories,
            IList<Tag> tags)
        {
            var posts = new List<Post>()
        {
            new()
            {
                Title = "ASP.NET Core Diagnostic Scenarios",
                ShortDescription = "David and friends has a great repos",
                Description = "Here is a few great DON'T and DO examples",
                UrlSlug = "aspnet-core-...",
                Published = true,
                PostedDate = new DateTime(2021, 9, 30, 10, 20, 0),
                ModifiedDate = null,
                ViewCount = 10,
                Author = authors[0],
                Category = categories[0],
                Tags = new List<Tag>()
                {
                    tags[0]
                }
            }
        };

            _dbContext.AddRange(posts);
            _dbContext.SaveChanges();
            return posts;
        }
    }
}