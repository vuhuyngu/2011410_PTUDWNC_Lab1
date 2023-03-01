namespace TatBlog.Data.Seeder;

public interface IDataSeeder
{
    void Initialize();
}

namespace TatBlog.Data.Seeders;

public class DataSeeder : IDataSeeder
{
    private readonly BlogDBContext _dbContext;

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
            new() {nameof = ".NET Core", Description = ".NET Core", UrlSlug"}
            new() {nameof = ".NET Core", Description = ".NET Core", UrlSlug"}
            new() {nameof = ".NET Core", Description = ".NET Core", UrlSlug"}
            new() {nameof = ".NET Core", Description = ".NET Core", UrlSlug"}
            new() {nameof = ".NET Core", Description = ".NET Core", UrlSlug"}
        };

        _dbContext.AddRange(categories);
        _dbContext.SaveChanges();

        return categories;
    }

    private IList<Tag> AddTags() 
    {
        var tags = new List<Tags>()
        {
            new() {nameof = "Google", Description = Google application", UrlSlug"}
            new() {nameof = "Google", Description = Google application", UrlSlug"}
            new() {nameof = "Google", Description = Google application", UrlSlug"}
            new() {nameof = "Google", Description = Google application", UrlSlug"}
            new() {nameof = "Google", Description = Google application", UrlSlug"}
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
    }
}
