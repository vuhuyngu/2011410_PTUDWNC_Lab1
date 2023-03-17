using TatBlog.Core.Entities;
using TatBlog.Data.Contexts;

namespace TatBlog.Data.Seeders
{
    public class DataSeeder : IDataSeeder
    {
        private readonly BlogDbContext _dbContext;

        public DataSeeder(BlogDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public void Initialize()
        {
            _dbContext.Database.EnsureCreated();

            if (_dbContext.Posts.Any()) return;

            var authors = AddAuthors();
            var categories = AddCategories();
            var tags = AddTags();
            var posts = AddPosts(authors, categories, tags);
        }
        private IList<Author> AddAuthors()
        {
            var authors = new List<Author>()
        {
            new()
            {
                FullName = "Jason Mouth",
                UrlSlug = "jason-mouth",
                Email = "json@gmail.com",
                JoinedDate = new DateTime(2022, 10, 21)
            },
            new()
            {
                FullName = "Jessica Wonder",
                UrlSlug = "jessica-wonder",
                Email = "jessica65@motip.com",
                JoinedDate = new DateTime(2020, 4, 19)
            },
            new()
            {
                FullName ="Kathy Smith",
                UrlSlug ="Kathy-Smith",
                Email ="KathySmith@gmotip.com",
                JoinedDate = new DateTime(2010, 9, 06)
            }

        };

            _dbContext.Authors.AddRange(authors);
            _dbContext.SaveChanges();

            return authors;
        }
        private IList<Category> AddCategories()
        {
            var categories = new List<Category>()
        {
            new() {Name = ".NET Core 5.0", Description = ".NET Core 5.0", UrlSlug="asp-dot-net-core", ShowOnMenu=true},
            new() {Name = ".NET Core 6.0", Description = ".NET Core 6.0", UrlSlug="architecture", ShowOnMenu=false},
            new() {Name = ".NET Core 7.0", Description = ".NET Core 7.0", UrlSlug="mess-info", ShowOnMenu=false},
            

        };

            _dbContext.AddRange(categories);
            _dbContext.SaveChanges();

            return categories;
        }

        private IList<Tag> AddTags()
        {
            var tags = new List<Tag>()
        {
            new() {Name = "Google", Description = "Google application", UrlSlug="google-clc"},
            new() {Name = "Mozilla", Description = "Mozilla application", UrlSlug="asp-dot-net"},
            new() {Name = "Microsoft Edge", Description = "Microsoft Edge application", UrlSlug="razor-page"},
            new() {Name = "Bing", Description = "Bing application", UrlSlug="deep-learning"},
            new() {Name = "CocCoc", Description = "CocCoc application", UrlSlug="neural-network"},
        };

            _dbContext.AddRange(tags);
            _dbContext.SaveChanges();

            return tags;
        }


        private IList<Post> AddPosts(
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
                Meta = "Abc def",
                UrlSlug = "aspnet-core-diagnostic-scenarios",
                Published = true,
                PostedDate = new DateTime(2021, 9, 30, 10, 20, 0),
                ModifiedDate = null,
                ViewCount = 10,
                Author = authors[0],
                Category = categories[1],
                Tags = new List<Tag>()
                {
                    tags[0]
                }
            },
            new()
            {
                Title = "HTML, CSS",
                ShortDescription = "David and friends has a great repos " ,
                Description = "Here's a few great DON'T and DO examples ",
                Meta = "David and friends has a great repository filled ",
                UrlSlug ="aspnet-core-diagnostic-scenarios",
                Published = true,
                PostedDate = new DateTime (2022, 7, 9, 10, 20, 0),
                ModifiedDate = null,
                Author= authors[0],
                ViewCount = 19,
                Category = categories[2],
                Tags = new List<Tag>()
                { tags[2] }
            },
            new()
            {
                Title = "Javascript",
                ShortDescription = "David and friends has a great repos " ,
                Description = "Here's a few great DON'T and DO examples ",
                Meta = "David and friends has a great repository filled ",
                UrlSlug ="aspnet-core-diagnostic-scenarios",
                Published = true,
                PostedDate = new DateTime (2022, 7, 9, 10, 20, 0),
                ModifiedDate = null,
                Author= authors[1],
                ViewCount = 19,
                Category = categories[2],
                Tags = new List<Tag>()
                { tags[2] }
            }
        };

            _dbContext.AddRange(posts);
            _dbContext.SaveChanges();
            return posts;
        }
    }
}