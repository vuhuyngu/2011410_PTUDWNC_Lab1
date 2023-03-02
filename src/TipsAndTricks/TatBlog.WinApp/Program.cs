using TatBlog.Data.Contexts;
using TatBlog.Data.Seeders;
using TatBlog.Services.Blogs;

/*var context = new BlogDbContext();*/

/*var seeder = new DataSeeder(context);

seeder.Initialize();

var authors = context.Authors.ToList();

Console.WriteLine("{0,-4}{1,-30}{2,-30}{3,12}", "ID", "Full Name", "Email", "Joined Date");

foreach (var author in authors)
{
    Console.WriteLine("{0,-4}{1,-30}{2,-30}{3,12:MM/dd/yyyy}",
        author.Id, author.FullName, author.Email, author.JoinedDate);
}*/

/*var posts = context.Posts
    .Where(p => p.Published)
    .OrderBy(p => p.Title)
    .Select(p => new
    {
        Id = p.Id,
        Title = p.Title,
        ViewCount = p.ViewCount,
        PostedDate = p.PostedDate,
        Author = p.Author.FullName,
        Category = p.Category.Name,
    })
    .ToList();*/

/*IBlogRepository blogRepo = new BlogRepository(context);

var posts = await BlogRepo.GetPopularArticlesAsync(3);

foreach (var post in posts)
{
    Console.WriteLine("ID: {0}", post.Id);
    Console.WriteLine("Title: {0}", post.Title);
    Console.WriteLine("View: {0}", post.ViewCount);
    Console.WriteLine("Date: {0:MM/dd/yyyy}", post.PostedDate);
    Console.WriteLine("Author: {0}", post.Author);
    Console.WriteLine("Category: {0}", post.Category);
    Console.WriteLine("".PadRight(80, '-'));
}*/

/* C, bài 1, câu a:
foreach (var tag in tags)
{
    Console.WriteLine("ID: {0}", tag.Id);
    Console.WriteLine("Name: {0}", tag.Name);
    Console.WriteLine("Description: {0}", tag.Description);
    Console.WriteLine("",PadRight(80, '-'));
}*/

var context = new BlogDbContext();

IBlogRepository blogRepo = new BlogRepository(context);

var categories = await blogRepo.GetCategoriesAsync();

Console.WriteLine("{0,-5}{1,-50}{2,10}",
    "ID", "Name", "Count");

foreach (var item in categories)
{
    Console.WriteLine("{ 0,-5}{ 1,-50}{ 2,10})",
        item.Id, item.Name, item.PostCount);
}

// C, bài 1, câu b:
var tags = await blogRepo.GetTagsAsync();

Console.WriteLine("{0,-5}{1,-50}{2,10}",
    "ID", "Name", "Count", "TagOrdinalNums");

foreach (var item1 in tags)
{
    Console.Writeline("{0,-5}{1,-50}{2,10}",
        item1.Id, item1.Name, item1.TagOrdinalNums, item1.PostCount);
}
