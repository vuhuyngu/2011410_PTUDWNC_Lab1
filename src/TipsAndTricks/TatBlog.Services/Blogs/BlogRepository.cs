using System;
using Microsoft.EntityFrameworkCore;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;
using TatBlog.Data.Contexts;

namespace TatBlog.Services.Blogs;

public class BlogRepository : IBlogRepository
{
    private readonly BlogDbContext _context;
    private BlogDbContext context;
    private object x;

    public BlogRepository(BlogRepository context) => _context = context;

    public BlogRepository(BlogDbContext context)
    {
        this.context = context;
    }

    // C, bài 1, câu a: Tìm thẻ tag theo tên định danh slug
    public Task<Tag> GetTagAsync(
        string slug,
        CancellationToken cancellationToken = default)
    {
        IQueryable<GetTagAsync> tagsQuery = _context.Set<tagsQuery>()
            .Include(x => x.Category)
            .Include(x => x.Author)
            .Include(x => x.Post);

        if (!string.IsNullOrWhiteSpace(slug))
        {
            tagsQuery = tagsQuery.Where(x => x.UrlSlug == slug);
        }
    }

    // Tìm bài viết có tên định danh "slug" và được đăng vào ngày/tháng/năm
    public async Task<Post> GetPostAsync(
        int year,
        int month,
        string slug,
        CancellationToken cancellationToken = default)
    {
        IQueryable<Post> postsQuery = _context.Set<Post>()
            .Include(x => x.Category)
            .Include(x => x.Author);

        if (year > 0)
        {
            postsQuery = postsQuery.Where(x => x.PostedDate.Year == year);
        }

        if (month > 0)
        {
            postsQuery = postsQuery.Where(x => x.PostedDate.Month == month);
        }

        if (!string.IsNullOrWhiteSpace(slug))
        {
            postsQuery = postsQuery.Where(x => x.UrlSlug == slug);
        }

        return await postsQuery.FirstOrDefaultAsync(cancellationToken);
    }

    

    public async Task<IList<Post>> GetPopularArticlesAsync(
        int numPosts, CancellationToken cancellationToken = default)
    {
    return await _context.Set<Post>()
    .Include(x => x.Author)
    .Include(x => x.Category)
    .OrderByDescending(p => p.ViewCount)
    .Take(numPosts)
    .ToListAsync(cancellationToken);
    }

    public async Task<bool> IsPostSlugExistedAsync(
        int postId,
        string slug,
        CancellationToken cancellationToken = default)
    {
    return await _context.Set<Post>()
    .AnyAsync(x => x.Id != postId && x.UrlSlug == slug,
    cancellationToken);
    }

    public async Task IncreaseViewCountAsync(
        int postId,
        CancellationToken cancellationToken = default)
    {
    await _context.Set<Post>()
    .Where(x => x.Id == postId)
    .ExecuteUpdateAsync(p =>
    p.SetProperty(x => x.ViewCount, x => x.ViewCount + 1),
    cancellationToken);
    }

    public Task<IList<Post>> GetPopularArticleAsync(int numPosts, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task<IList<CategoryItem>> GetCategoriesAsync(
        bool showOnMenu = false,
        CancellationToken cancellationToken = default)
    {
        IQueryable<Category> categories = _context.Set<Category>();

        if (showOnMenu)
        {
            categories = categories.Where(x => x.ShowOnMenu);
        }

        return await categories
            .OrderBy(x => x.Name)
            .Select(x = new CategoryItem()
            {
                Id = x.Id,
                Name = x.Name,
                UrlSlug = x.UrlSlug,
                Description = x.Description,
                ShowOnMenu = x.ShowOnMenu,
                PostCount = x.Posts.Count(p => p.Published)
            })
            .ToListAsync(cancellationToken);
    }

    Task<List<CategoryItem>> IBlogRepository.GetCategoriesAsync(bool showOnMenu, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
