﻿using Microsoft.EntityFrameworkCore;
using TatBlog.Core.Contracts;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;
using TatBlog.Data.Contexts;
using TatBlog.Services.Extensions;


namespace TatBlog.Services.Blogs;

public class BlogRepository : IBlogRepository
{
    private readonly BlogDbContext _context;
    
    /*private object x;*/

    public BlogRepository(BlogDbContext context)
    {
        _context = context;
    }
    
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


    // Tìm Top N bài viết phổ được nhiều người xem nhất
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

    // Tăng số lượt xem của một bài viết
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

    public async Task<Author> GetAuthorAsync(string slug, CancellationToken cancellationToken = default)
    {
        return await _context.Set<Author>()
            .FirstOrDefaultAsync(a => a.UrlSlug == slug, cancellationToken);
    }

    public async Task<Author> GetAuthorByIdAsync(int authorId)
    {
        return await _context.Set<Author>().FindAsync(authorId);
    }

    public async Task<IList<AuthorItem>> GetAuthorsAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Set<Author>()
            .OrderBy(a => a.FullName)
            .Select(a => new AuthorItem()
            {
                Id = a.Id,
                FullName = a.FullName,
                Email = a.ToString(),
                JoinedDate = a.JoinedDate,
                ImageUrl = a.ImageUrl,
                UrlSlug = a.UrlSlug,
                Notes = a.Notes,
                PostCount = a.Posts.Count(p => p.Published)
            })
            .ToListAsync(cancellationToken);
    }






    public Task<IList<Post>> GetPopularArticleAsync(int numPosts, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    

    Task<List<CategoryItem>> IBlogRepository.GetCategoriesAsync(bool showOnMenu, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    

    public async Task<IList<Post>> GetPostsAsync(
        PostQuery condition,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        return await FilterPosts(condition)
            .OrderByDescending(x => x.PostedDate)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken: cancellationToken);
    }

    public async Task<int> CountPostsAsync(
        PostQuery condition, CancellationToken cancellationToken = default)
    {
        return await FilterPosts(condition).CountAsync(cancellationToken: cancellationToken);
    }

    public async Task<IList<MonthlyPostCountItem>> CountMonthlyPostsAsync(
        int numMonths, CancellationToken cancellationToken = default)
    {
        return await _context.Set<Post>()
            .GroupBy(x => new { x.PostedDate.Year, x.PostedDate.Month })
            .Select(g => new MonthlyPostCountItem()
            {
                Year = g.Key.Year,
                Month = g.Key.Month,
                PostCount = g.Count(x => x.Published)
            })
            .OrderByDescending(x => x.Year)
            .ThenByDescending(x => x.Month)
            .ToListAsync(cancellationToken);
    }

    public async Task<Category> GetCategoryAsync(
        string slug, CancellationToken cancellationToken = default)
    {
        return await _context.Set<Category>()
            .FirstOrDefaultAsync(x => x.UrlSlug == slug, cancellationToken);
    }

    public async Task<Category> GetCategoryByIdAsync(int categoryId)
    {
        return await _context.Set<Category>().FindAsync(categoryId);
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
            .Select(x => new CategoryItem()
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

    public async Task<IPagedList<CategoryItem>> GetPagedCategoriesAsync(
        IPagingParams pagingParams,
        CancellationToken cancellationToken = default)
    {
        var tagQuery = _context.Set<Category>()
            .Select(x => new CategoryItem()
            {
                Id = x.Id,
                Name = x.Name,
                UrlSlug = x.UrlSlug,
                Description = x.Description,
                ShowOnMenu = x.ShowOnMenu,
                PostCount = x.Posts.Count(p => p.Published)
            });

        return await tagQuery.ToPagedListAsync(pagingParams, cancellationToken);
    }

    public async Task<Category> CreateOrUpdateCategoryAsync(
        Category category, CancellationToken cancellationToken = default)
    {
        if (category.Id > 0)
        {
            _context.Set<Category>().Update(category);
        }
        else
        {
            _context.Set<Category>().Add(category);
        }

        await _context.SaveChangesAsync(cancellationToken);

        return category;
    }

    public async Task<bool> IsCategorySlugExistedAsync(
        int categoryId, string categorySlug,
        CancellationToken cancellationToken = default)
    {
        return await _context.Set<Category>()
            .AnyAsync(x => x.Id != categoryId && x.UrlSlug == categorySlug, cancellationToken);
    }

    public async Task<bool> DeleteCategoryAsync(
        int categoryId, CancellationToken cancellationToken = default)
    {
        var category = await _context.Set<Category>().FindAsync(categoryId);

        if (category is null) return false;

        _context.Set<Category>().Remove(category);
        var rowsCount = await _context.SaveChangesAsync(cancellationToken);

        return rowsCount > 0;
    }

    public async Task<Tag> GetTagAsync(
        string slug, CancellationToken cancellationToken = default)
    {
        return await _context.Set<Tag>()
            .FirstOrDefaultAsync(x => x.UrlSlug == slug, cancellationToken);
    }

    public async Task<IList<TagItem>> GetTagsAsync(
        CancellationToken cancellationToken = default)
    {
        return await _context.Set<Tag>()
            .OrderBy(x => x.Name)
            .Select(x => new TagItem()
            {
                Id = x.Id,
                Name = x.Name,
                UrlSlug = x.UrlSlug,
                Description = x.Description,
                PostCount = x.Posts.Count(p => p.Published)
            })
            .ToListAsync(cancellationToken);
    }

    public async Task<IPagedList<TagItem>> GetPagedTagsAsync(IPagingParams pagingParams, CancellationToken cancellationToken = default)
    {
        var tagQuery = _context.Set<Tag>()
            .OrderBy(x => x.Name)
            .Select(x => new TagItem()
            {
                Id = x.Id,
                Name = x.Name,
                UrlSlug = x.UrlSlug,
                Description = x.Description,
                PostCount = x.Posts.Count(p => p.Published)
            });

        return await tagQuery
            .ToPagedListAsync(pagingParams, cancellationToken);
    }

    public async Task<bool> DeleteTagAsync(
        int tagId, CancellationToken cancellationToken = default)
    {
        //var tag = await _context.Set<Tag>().FindAsync(tagId);

        //if (tag == null) return false;

        //_context.Set<Tag>().Remove(tag);
        //return await _context.SaveChangesAsync(cancellationToken) > 0;

        return await _context.Set<Tag>()
            .Where(x => x.Id == tagId)
            .ExecuteDeleteAsync(cancellationToken) > 0;
    }

    public async Task<bool> CreateOrUpdateTagAsync(
        Tag tag, CancellationToken cancellationToken = default)
    {
        if (tag.Id > 0)
        {
            _context.Set<Tag>().Update(tag);
        }
        else
        {
            _context.Set<Tag>().Add(tag);
        }

        return await _context.SaveChangesAsync(cancellationToken) > 0;
    }

    /*
    public async Task<Post> GetPostAsync(
        string slug,
        CancellationToken cancellationToken = default)
    {
        var postQuery = new PostQuery()
        {
            PublishedOnly = false,
            TltleSlug = slug,
        };

        return await FilterPosts(postQuery).FirstOrDefaultAsync(cancellationToken);
    }*/
}
