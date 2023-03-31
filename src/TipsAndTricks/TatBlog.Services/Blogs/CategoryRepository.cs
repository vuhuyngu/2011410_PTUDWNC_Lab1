using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using TatBlog.Core.Contracts;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;
using TatBlog.Data.Contexts;
using TatBlog.Services.Extensions;

namespace TatBlog.Services.Blogs;

public class CategoryRepository : ICategoryRepository
{
    private readonly BlogDbContext _context;
    private readonly IMemoryCache _memoryCache;

    public CategoryRepository(BlogDbContext context, IMemoryCache memoryCache)
    {
        _context = context;
        _memoryCache = memoryCache;
    }

    public async Task<Author> GetCategoryBySlugAsync(
        string slug, CancellationToken cancellationToken = default)
    {
        return await _context.Set<Author>()
            .FirstOrDefaultAsync(a => a.UrlSlug == slug, cancellationToken);
    }

    public async Task<IPagedList<AuthorItem>> GetPagedCategoriesAsync(
        IPagingParams pagingParams,
        string name = null,
        CancellationToken cancellationToken = default)
    {
        var categoryQuery = _context.Set<Category>()
            .AsNoTracking();

        if (!string.IsNullOrWhiteSpace(name))
        {
            categoryQuery = categoryQuery.Where(x => x.Name.Contains(name));
        }

        return (IPagedList<AuthorItem>)await categoryQuery.Select(a => new CategoryItem()
        {
            Id = a.Id,
            Name = a.Name,
            UrlSlug = a.UrlSlug,
            Description = a.Description,
            ShowOnMenu = a.ShowOnMenu,
            PostCount = a.Posts.Count(p => p.Published)
        })
            .ToPagedListAsync(pagingParams, cancellationToken);
    }

    public async Task<IPagedList<T>> GetPagedAuthorsAsync<T>(
        Func<IQueryable<Author>, IQueryable<T>> mapper,
        IPagingParams pagingParams,
        string name = null,
        CancellationToken cancellationToken = default)
    {
        var authorQuery = _context.Set<Author>().AsNoTracking();

        if (!string.IsNullOrEmpty(name))
        {
            authorQuery = authorQuery.Where(x => x.FullName.Contains(name));
        }

        return await mapper(authorQuery)
            .ToPagedListAsync(pagingParams, cancellationToken);
    }

    public async Task<Category> GetCategoryByIdAsync(
        int categoryId)
    {
        return await _context.Set<Category>().FindAsync(categoryId);
    }

    public async Task<Category> GetCachedCategoryByIdAsync(
        int categoryId)
    {
        return await _memoryCache.GetOrCreateAsync(
            $"category.by-id.{categoryId}",
            async (entry) =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30);
                return await GetCategoryByIdAsync(categoryId);
            });
    }
    public async Task<bool> IsCategorySlugExistedAsync(
        int categoryId, 
        string slug, 
        CancellationToken cancellationToken = default)
    {
        return await _context.Categories
            .AnyAsync(x => x.Id != categoryId && x.UrlSlug == slug, cancellationToken);
    }

    public async Task<bool> AddOrUpdateAsync(
        Category category, CancellationToken cancellationToken = default)
    {
        if (category.Id > 0)
        {
            _context.Categories.Update(category);
            _memoryCache.Remove($"category.by-id.{category.Id}");
        }
        else
        {
            _context.Categories.Add(category);
        }

        return await _context.SaveChangesAsync(cancellationToken) > 0;
    }

    public Task<Category> GetCachedCategoryByIdAsync(int categoryId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> DeleteCategoryAsync(
        int categoryId, CancellationToken cancellationToken = default)
    {
        return await _context.Categories
            .Where(x => x.Id == categoryId)
            .ExecuteDeleteAsync(cancellationToken) > 0;
    }

    Task<IPagedList<CategoryItem>> ICategoryRepository.GetPagedCategoriesAsync(IPagingParams pagingParams, string name, CancellationToken cancellation)
    {
        throw new NotImplementedException();
    }
}
