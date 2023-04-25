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
            },
	    new()
            {
                FullName ="Johnny Bravo",
                UrlSlug ="Johnny-Bravo",
                Email ="JohnnyBravo@gmotip.com",
                JoinedDate = new DateTime(2010, 9, 06)
            },
	    new()
            {
                FullName ="Omasion Edie",
                UrlSlug ="Omasion-Edie",
                Email ="OmasionEdieh@gmotip.com",
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
	    new() {Name = ".NET Core 7.1", Description = ".NET Core 7.1", UrlSlug="asp-dot-net-core-t", ShowOnMenu=true},
            new() {Name = ".NET Core 7.2", Description = ".NET Core 7.2", UrlSlug="architecture-t", ShowOnMenu=false},
            new() {Name = ".NET Core 7.3", Description = ".NET Core 7.3", UrlSlug="mess-info-t", ShowOnMenu=false},
	    new() {Name = ".NET Core 8.0", Description = ".NET Core 8.0", UrlSlug="asp-dot-net-core-h", ShowOnMenu=true},
            new() {Name = ".NET Core 8.2", Description = ".NET Core 8.2", UrlSlug="architecture-h", ShowOnMenu=false},
            new() {Name = ".NET Core 8.6", Description = ".NET Core 8.6", UrlSlug="mess-info-h", ShowOnMenu=false},
	    new() {Name = ".NET Core 9.0", Description = ".NET Core 9.0", UrlSlug="asp-dot-net-core-n", ShowOnMenu=true},
            

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
	    new() {Name = "Word", Description = "Word application", UrlSlug="word-clc-a"},
            new() {Name = "Excel", Description = "Excel application", UrlSlug="asp-dot-net-a"},
            new() {Name = "Access", Description = "Access application", UrlSlug="razor-page-a"},
            new() {Name = "Powerpoint", Description = "Powerpoint application", UrlSlug="deep-learning-a"},
            new() {Name = "Notepad", Description = "Notepad application", UrlSlug="neural-network-a"},
	    new() {Name = "Visual Studio", Description = "Visual Studio application", UrlSlug="google-clc-c"},
            new() {Name = "Visual Studio Code", Description = "Visual Studio Code application", UrlSlug="asp-dot-net-c"},
            new() {Name = "Control Panel", Description = "Control Panel application", UrlSlug="razor-page-c"},
            new() {Name = "Discord", Description = "Discord application", UrlSlug="deep-learning-c"},
            new() {Name = "Foxit PDF Reader", Description = "Foxit PDF Reader application", UrlSlug="neural-network-c"},
	    new() {Name = "Paint", Description = "Paint application", UrlSlug="google-clc-w"},
            new() {Name = "Unikey", Description = "Unikey application", UrlSlug="asp-dot-net-w"},
            new() {Name = "VLC Media Player", Description = "VLC Media Player application", UrlSlug="razor-page-w"},
            new() {Name = "Zalo", Description = "Zalo application", UrlSlug="deep-learning-w"},
            new() {Name = "My PC", Description = "My PC application", UrlSlug="neural-network-w"},
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
                Category = categories[0],
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
                Category = categories[1],
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
            },

	    new()
            {
                Title = "C Sharp - C#",
                ShortDescription = "David and friends has a great repos",
                Description = "Here is a few great DON'T and DO examples",
                Meta = "Abc def",
                UrlSlug = "aspnet-core-diagnostic-scenarios",
                Published = true,
                PostedDate = new DateTime(2021, 9, 29, 10, 20, 0),
                ModifiedDate = null,
                ViewCount = 10,
                Author = authors[0],
                Category = categories[3],
                Tags = new List<Tag>()
                {
                    tags[0]
                }
            },
            new()
            {
                Title = "React JS",
                ShortDescription = "David and friends has a great repos " ,
                Description = "Here's a few great DON'T and DO examples ",
                Meta = "David and friends has a great repository filled ",
                UrlSlug ="aspnet-core-diagnostic-scenarios",
                Published = true,
                PostedDate = new DateTime (2022, 9, 9, 10, 20, 0),
                ModifiedDate = null,
                Author= authors[0],
                ViewCount = 19,
                Category = categories[4],
                Tags = new List<Tag>()
                { tags[2] }
            },
            new()
            {
                Title = "Node JS",
                ShortDescription = "David and friends has a great repos " ,
                Description = "Here's a few great DON'T and DO examples ",
                Meta = "David and friends has a great repository filled ",
                UrlSlug ="aspnet-core-diagnostic-scenarios",
                Published = true,
                PostedDate = new DateTime (2022, 7, 7, 10, 20, 0),
                ModifiedDate = null,
                Author= authors[1],
                ViewCount = 19,
                Category = categories[5],
                Tags = new List<Tag>()
                { tags[2] }
            },
	    new()
            {
                Title = "My SQL",
                ShortDescription = "David and friends has a great repos",
                Description = "Here is a few great DON'T and DO examples",
                Meta = "Abc def",
                UrlSlug = "aspnet-core-diagnostic-scenarios",
                Published = true,
                PostedDate = new DateTime(2021, 9, 28, 10, 20, 0),
                ModifiedDate = null,
                ViewCount = 10,
                Author = authors[0],
                Category = categories[6],
                Tags = new List<Tag>()
                {
                    tags[0]
                }
            },
            new()
            {
                Title = "SQL Server",
                ShortDescription = "David and friends has a great repos " ,
                Description = "Here's a few great DON'T and DO examples ",
                Meta = "David and friends has a great repository filled ",
                UrlSlug ="aspnet-core-diagnostic-scenarios",
                Published = true,
                PostedDate = new DateTime (2022, 8, 9, 10, 20, 0),
                ModifiedDate = null,
                Author= authors[0],
                ViewCount = 19,
                Category = categories[7],
                Tags = new List<Tag>()
                { tags[2] }
            },
            new()
            {
                Title = "MongoDB",
                ShortDescription = "David and friends has a great repos " ,
                Description = "Here's a few great DON'T and DO examples ",
                Meta = "David and friends has a great repository filled ",
                UrlSlug ="aspnet-core-diagnostic-scenarios",
                Published = true,
                PostedDate = new DateTime (2022, 7, 8, 10, 20, 0),
                ModifiedDate = null,
                Author= authors[1],
                ViewCount = 19,
                Category = categories[8],
                Tags = new List<Tag>()
                { tags[2] }
            },

	    new()
            {
                Title = "Python 2.0",
                ShortDescription = "David and friends has a great repos",
                Description = "Here is a few great DON'T and DO examples",
                Meta = "Abc def",
                UrlSlug = "aspnet-core-diagnostic-scenarios",
                Published = true,
                PostedDate = new DateTime(2021, 9, 27, 10, 20, 0),
                ModifiedDate = null,
                ViewCount = 10,
                Author = authors[0],
                Category = categories[9],
                Tags = new List<Tag>()
                {
                    tags[0]
                }
            },
            new()
            {
                Title = "PHP",
                ShortDescription = "David and friends has a great repos " ,
                Description = "Here's a few great DON'T and DO examples ",
                Meta = "David and friends has a great repository filled ",
                UrlSlug ="aspnet-core-diagnostic-scenarios",
                Published = true,
                PostedDate = new DateTime (2022, 1, 9, 10, 20, 0),
                ModifiedDate = null,
                Author= authors[0],
                ViewCount = 19,
                Category = categories[1],
                Tags = new List<Tag>()
                { tags[2] }
            },
            new()
            {
                Title = "C++",
                ShortDescription = "David and friends has a great repos " ,
                Description = "Here's a few great DON'T and DO examples ",
                Meta = "David and friends has a great repository filled ",
                UrlSlug ="aspnet-core-diagnostic-scenarios",
                Published = true,
                PostedDate = new DateTime (2022, 7, 1, 10, 20, 0),
                ModifiedDate = null,
                Author= authors[1],
                ViewCount = 19,
                Category = categories[2],
                Tags = new List<Tag>()
                { tags[2] }
            },
	    new()
            {
                Title = "Python 3.0",
                ShortDescription = "David and friends has a great repos",
                Description = "Here is a few great DON'T and DO examples",
                Meta = "Abc def",
                UrlSlug = "aspnet-core-diagnostic-scenarios",
                Published = true,
                PostedDate = new DateTime(2021, 9, 26, 10, 20, 0),
                ModifiedDate = null,
                ViewCount = 10,
                Author = authors[0],
                Category = categories[3],
                Tags = new List<Tag>()
                {
                    tags[0]
                }
            },
            new()
            {
                Title = "WordPress",
                ShortDescription = "David and friends has a great repos " ,
                Description = "Here's a few great DON'T and DO examples ",
                Meta = "David and friends has a great repository filled ",
                UrlSlug ="aspnet-core-diagnostic-scenarios",
                Published = true,
                PostedDate = new DateTime (2022, 7, 1, 10, 20, 0),
                ModifiedDate = null,
                Author= authors[0],
                ViewCount = 19,
                Category = categories[4],
                Tags = new List<Tag>()
                { tags[2] }
            },
            new()
            {
                Title = "Angular",
                ShortDescription = "David and friends has a great repos " ,
                Description = "Here's a few great DON'T and DO examples ",
                Meta = "David and friends has a great repository filled ",
                UrlSlug ="aspnet-core-diagnostic-scenarios",
                Published = true,
                PostedDate = new DateTime (2022, 1, 9, 10, 20, 0),
                ModifiedDate = null,
                Author= authors[1],
                ViewCount = 19,
                Category = categories[5],
                Tags = new List<Tag>()
                { tags[2] }
            },

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
                Category = categories[0],
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
                Category = categories[1],
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
            },

	    new()
            {
                Title = "C Sharp - C#",
                ShortDescription = "David and friends has a great repos",
                Description = "Here is a few great DON'T and DO examples",
                Meta = "Abc def",
                UrlSlug = "aspnet-core-diagnostic-scenarios",
                Published = true,
                PostedDate = new DateTime(2021, 9, 29, 10, 20, 0),
                ModifiedDate = null,
                ViewCount = 10,
                Author = authors[0],
                Category = categories[3],
                Tags = new List<Tag>()
                {
                    tags[0]
                }
            },
            new()
            {
                Title = "React JS",
                ShortDescription = "David and friends has a great repos " ,
                Description = "Here's a few great DON'T and DO examples ",
                Meta = "David and friends has a great repository filled ",
                UrlSlug ="aspnet-core-diagnostic-scenarios",
                Published = true,
                PostedDate = new DateTime (2022, 9, 9, 10, 20, 0),
                ModifiedDate = null,
                Author= authors[0],
                ViewCount = 19,
                Category = categories[4],
                Tags = new List<Tag>()
                { tags[2] }
            },
            new()
            {
                Title = "Node JS",
                ShortDescription = "David and friends has a great repos " ,
                Description = "Here's a few great DON'T and DO examples ",
                Meta = "David and friends has a great repository filled ",
                UrlSlug ="aspnet-core-diagnostic-scenarios",
                Published = true,
                PostedDate = new DateTime (2022, 7, 7, 10, 20, 0),
                ModifiedDate = null,
                Author= authors[1],
                ViewCount = 19,
                Category = categories[5],
                Tags = new List<Tag>()
                { tags[2] }
            },
	    new()
            {
                Title = "My SQL",
                ShortDescription = "David and friends has a great repos",
                Description = "Here is a few great DON'T and DO examples",
                Meta = "Abc def",
                UrlSlug = "aspnet-core-diagnostic-scenarios",
                Published = true,
                PostedDate = new DateTime(2021, 9, 28, 10, 20, 0),
                ModifiedDate = null,
                ViewCount = 10,
                Author = authors[0],
                Category = categories[6],
                Tags = new List<Tag>()
                {
                    tags[0]
                }
            },
            new()
            {
                Title = "SQL Server",
                ShortDescription = "David and friends has a great repos " ,
                Description = "Here's a few great DON'T and DO examples ",
                Meta = "David and friends has a great repository filled ",
                UrlSlug ="aspnet-core-diagnostic-scenarios",
                Published = true,
                PostedDate = new DateTime (2022, 8, 9, 10, 20, 0),
                ModifiedDate = null,
                Author= authors[0],
                ViewCount = 19,
                Category = categories[7],
                Tags = new List<Tag>()
                { tags[2] }
            },
            new()
            {
                Title = "MongoDB",
                ShortDescription = "David and friends has a great repos " ,
                Description = "Here's a few great DON'T and DO examples ",
                Meta = "David and friends has a great repository filled ",
                UrlSlug ="aspnet-core-diagnostic-scenarios",
                Published = true,
                PostedDate = new DateTime (2022, 7, 8, 10, 20, 0),
                ModifiedDate = null,
                Author= authors[1],
                ViewCount = 19,
                Category = categories[8],
                Tags = new List<Tag>()
                { tags[2] }
            },

	    new()
            {
                Title = "Python 2.0",
                ShortDescription = "David and friends has a great repos",
                Description = "Here is a few great DON'T and DO examples",
                Meta = "Abc def",
                UrlSlug = "aspnet-core-diagnostic-scenarios",
                Published = true,
                PostedDate = new DateTime(2021, 9, 27, 10, 20, 0),
                ModifiedDate = null,
                ViewCount = 10,
                Author = authors[0],
                Category = categories[9],
                Tags = new List<Tag>()
                {
                    tags[0]
                }
            },
            new()
            {
                Title = "PHP",
                ShortDescription = "David and friends has a great repos " ,
                Description = "Here's a few great DON'T and DO examples ",
                Meta = "David and friends has a great repository filled ",
                UrlSlug ="aspnet-core-diagnostic-scenarios",
                Published = true,
                PostedDate = new DateTime (2022, 1, 9, 10, 20, 0),
                ModifiedDate = null,
                Author= authors[0],
                ViewCount = 19,
                Category = categories[1],
                Tags = new List<Tag>()
                { tags[2] }
            },
            new()
            {
                Title = "C++",
                ShortDescription = "David and friends has a great repos " ,
                Description = "Here's a few great DON'T and DO examples ",
                Meta = "David and friends has a great repository filled ",
                UrlSlug ="aspnet-core-diagnostic-scenarios",
                Published = true,
                PostedDate = new DateTime (2022, 7, 1, 10, 20, 0),
                ModifiedDate = null,
                Author= authors[1],
                ViewCount = 19,
                Category = categories[2],
                Tags = new List<Tag>()
                { tags[2] }
            },
	    new()
            {
                Title = "Python 3.0",
                ShortDescription = "David and friends has a great repos",
                Description = "Here is a few great DON'T and DO examples",
                Meta = "Abc def",
                UrlSlug = "aspnet-core-diagnostic-scenarios",
                Published = true,
                PostedDate = new DateTime(2021, 9, 26, 10, 20, 0),
                ModifiedDate = null,
                ViewCount = 10,
                Author = authors[0],
                Category = categories[3],
                Tags = new List<Tag>()
                {
                    tags[0]
                }
            },
            new()
            {
                Title = "WordPress",
                ShortDescription = "David and friends has a great repos " ,
                Description = "Here's a few great DON'T and DO examples ",
                Meta = "David and friends has a great repository filled ",
                UrlSlug ="aspnet-core-diagnostic-scenarios",
                Published = true,
                PostedDate = new DateTime (2022, 7, 1, 10, 20, 0),
                ModifiedDate = null,
                Author= authors[0],
                ViewCount = 19,
                Category = categories[4],
                Tags = new List<Tag>()
                { tags[2] }
            },
            new()
            {
                Title = "Angular",
                ShortDescription = "David and friends has a great repos " ,
                Description = "Here's a few great DON'T and DO examples ",
                Meta = "David and friends has a great repository filled ",
                UrlSlug ="aspnet-core-diagnostic-scenarios",
                Published = true,
                PostedDate = new DateTime (2022, 1, 9, 10, 20, 0),
                ModifiedDate = null,
                Author= authors[1],
                ViewCount = 19,
                Category = categories[2],
                Tags = new List<Tag>()
                { tags[2] }
            },
        };

            _dbContext.AddRange(posts);
            _dbContext.SaveChanges();
            return posts;
        }
    }
}