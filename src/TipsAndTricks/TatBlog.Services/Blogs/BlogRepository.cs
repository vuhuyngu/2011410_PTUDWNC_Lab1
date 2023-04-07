﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TatBlog.Core.Contracts;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;
using TatBlog.Data.Contexts;
using TatBlog.Services.Extensions;

namespace TatBlog.Services.Blogs
{
    public class BlogRepository : IBlogRepository
    {
        private readonly BlogDbContext _context;

        public BlogRepository(BlogDbContext context)
        {
            _context = context;
        }



        // Lấy danh sách chủ đề và đếm số lượng bài viết trong từng chủ đề
        public async Task<IList<CategoryItem>> GetCategoriesAsync(bool showOnMenu = false, CancellationToken cancellationToken = default)
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

        // Lấy danh sách từ khóa/ thẻ và phân trang theo các tham số pagingParams
        public async Task<IPagedList<TagItem>> GetPagedTagsAsync(IPagingParams pagingParams, CancellationToken cancellationToken = default)
        {
            var tagQuery = _context.Set<Tag>()
                .Select(x => new TagItem()
                {
                    Id = x.Id,
                    Name = x.Name,
                    UrlSlug = x.UrlSlug,
                    Description = x.Description,
                    PostCount = x.Posts.Count(p => p.Published)
                });

            return await tagQuery.ToPagedListAsync(pagingParams, cancellationToken);
        }

        // Tìm Top n bài viết phổ biến được nhiều người xem nhất
        public async Task<IList<Post>> GetPopularArticlesAsync(int numPosts, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Post>()
                .Include(x => x.Author)
                .Include(x => x.Category)
                .OrderByDescending(p => p.ViewCount)
                .Take(numPosts)
                .ToListAsync(cancellationToken);
        }

        // Tìm bài viết có tên định danh là 'slug' và được đăng vào tháng 'month' năm 'year'
        //public async Task<Post> GetPostAsync(int year, int month, string slug, CancellationToken cancellationToken = default)
        //{
        //    IQueryable<Post> postsQuery = _context.Set<Post>()
        //        .Include(x => x.Category)
        //        .Include(x => x.Author);

        //    if (year > 0)
        //    {
        //        postsQuery = postsQuery.Where(x => x.PostedDate.Year == year);
        //    }

        //    if (month > 0)
        //    {
        //        postsQuery = postsQuery.Where(x => x.PostedDate.Month == month);
        //    }

        //    if (!string.IsNullOrWhiteSpace(slug))
        //    {
        //        postsQuery = postsQuery.Where(x => x.UrlSlug == slug);
        //    }

        //    return await postsQuery.FirstOrDefaultAsync(cancellationToken);
        //}

        //===============================================================//
        public async Task<Post> GetPostAsync(
             string slug,
             CancellationToken cancellationToken = default)
        {
            var postQuery = new PostQuery()
            {
                PublishedOnly = false,
                TitleSlug = slug
            };

            return await FilterPosts(postQuery).FirstOrDefaultAsync(cancellationToken);
        }
        //===============================================================//


        // Tăng số lượt xem của một bài viết
        public async Task IncreaseViewCountAsync(int postId, CancellationToken cancellationToken = default)
        {
            await _context.Set<Post>()
                .Where(x => x.Id == postId)
                .ExecuteUpdateAsync(p => p.SetProperty(x => x.ViewCount, x => x.ViewCount + 1), cancellationToken);
        }

        // Kiểm tra tên định danh của bài viết đã có hay chưa
        public async Task<bool> IsPostSlugExistedAsync(int postId, string slug, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Post>().AnyAsync(x => x.Id != postId && x.UrlSlug == slug, cancellationToken);
        }

        // 1.a) Tìm thẻ theo tên định danh
        //public async Task<IList<Tag>> GetTagByUrlSlug(string slug, CancellationToken cancellationToken = default)
        //{
        //    return await _context.Set<Tag>()
        //        .Where(x => x.UrlSlug == slug)
        //        .ToListAsync(cancellationToken);
        //}

        // 1.c) Lấy danh sách các thẻ kèm theo số bài viết chứa thẻ đó
        public async Task<IList<TagItem>> GetTagsAsync(CancellationToken cancellationToken = default)
        {
            IQueryable<Tag> tags = _context.Set<Tag>();

            return await tags
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

        // 1.d) Xóa thẻ theo Id
        //public async Task RemoveTagById(int id, CancellationToken cancellationToken = default)
        //{
        //    int count = await _context.Database.ExecuteSqlRawAsync($"DELETE FROM dbo.PostTags WHERE TagsId = {id}");

        //    if (count > 0)
        //    {
        //        await _context.Database.ExecuteSqlRawAsync($"DELETE FROM dbo.Tags WHERE Id = {id}");
        //        await _context.SaveChangesAsync();
        //    }
        //}

        // 1.e) Tìm một chuyên mục theo tên định danh
        //public async Task<IList<Category>> GetCategoryByUrlSlug(string slug, CancellationToken cancellationToken = default)
        //{
        //    return await _context.Set<Category>()
        //        .Where(x => x.UrlSlug == slug)
        //        .ToListAsync(cancellationToken);
        //}

        // 1.f) Tìm một chuyên mục theo Id
        //public async Task<IList<Category>> GetCategoryById(int id, CancellationToken cancellationToken = default)
        //{
        //    return await _context.Set<Category>()
        //        .Where(x => x.Id == id)
        //        .ToListAsync(cancellationToken);
        //}

        // 1.g) Thêm hoặc cập nhật một chuyên mục
        //public async Task AddOrUpdateCategory(Category category, CancellationToken cancellationToken = default)
        //{
        //    if (await IsCategorySlugExistedAsync(category.Id, category.UrlSlug))
        //    {
        //        var oldCategory = await _context.Categories.FirstOrDefaultAsync(x => x.UrlSlug.Equals(category.UrlSlug), cancellationToken);
        //        category.Id = oldCategory.Id;
        //        _context.Entry(oldCategory).CurrentValues.SetValues(category);
        //        await _context.SaveChangesAsync();
        //    }
        //    else
        //    {
        //        _context.Add(category);
        //        await _context.SaveChangesAsync();
        //    }
        //}

        // 1.h) Xóa một chuyên mục
        /*public async Task<bool> DeleteCategoryAsync(
        int categoryId, CancellationToken cancellationToken = default)
        {
            var category = await _context.Set<Category>().FindAsync(categoryId);

            if (category is null) return false;

            _context.Set<Category>().Remove(category);
            var rowsCount = await _context.SaveChangesAsync(cancellationToken);

            return rowsCount > 0;
        }*/

        // 1.i) Kiểm tra sự tồn tại của slug thuộc một chuyên mục
        //public async Task<bool> IsCategorySlugExistedAsync(int categoryId, string slug, CancellationToken cancellationToken = default)
        //{
        //    return await _context.Set<Category>().AnyAsync(x => x.Id == categoryId && x.UrlSlug == slug, cancellationToken);
        //}

        //public async Task<bool> IsCategorySlugExistedAsync(string slug, CancellationToken cancellationToken = default)
        //{
        //    return await _context.Set<Category>().AnyAsync(x => x.UrlSlug == slug, cancellationToken);
        //}

        #region Solution

        // 2.c) Tìm một tác giả theo tên định danh slug
        /*public async Task<Author> GetAuthorAsync(string slug, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Author>()
                .FirstOrDefaultAsync(a => a.UrlSlug == slug, cancellationToken);
        }*/

        // 2.b) Tìm tác giả theo Id
        /*public async Task<Author> GetAuthorByIdAsync(int authorId)
        {
            return await _context.Set<Author>().FindAsync(authorId);
        }*/

        /*public async Task<IList<AuthorItem>> GetAuthorsAsync(CancellationToken cancellationToken = default)
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
        }*/

        //public async Task<IList<Post>> GetPostsAsync(
        //    PostQuery condition,
        //    int pageNumber,
        //    int pageSize,
        //    CancellationToken cancellationToken = default)
        //{
        //    return await FilterPosts(condition)
        //        .OrderByDescending(x => x.PostedDate)
        //        .Skip((pageNumber - 1) * pageSize)
        //        .Take(pageSize)
        //        .ToListAsync(cancellationToken: cancellationToken);
        //}

        // 1.r) Đếm số lượng bài viết thỏa mãn điều kiện (postquery)
        //public async Task<int> CountPostsAsync(
        //    PostQuery condition, CancellationToken cancellationToken = default)
        //{
        //    return await FilterPosts(condition).CountAsync(cancellationToken: cancellationToken);
        //}

        // 1.k) Đếm số lượng bài viết trong N tháng gần nhất
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

        /*public async Task<Category> GetCategoryAsync(
            string slug, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Category>()
                .FirstOrDefaultAsync(x => x.UrlSlug == slug, cancellationToken);
        }*/

        /*public async Task<Category> GetCategoryByIdAsync(int categoryId)
        {
            return await _context.Set<Category>().FindAsync(categoryId);
        }*/

        // 1.j) Lấy và phân trang danh sách chuyên mục
        /*public async Task<IPagedList<CategoryItem>> GetPagedCategoriesAsync(
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
        }*/

        /*public async Task<Category> CreateOrUpdateCategoryAsync(
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
        }*/

        /*public async Task<bool> IsCategorySlugExistedAsync(
            int categoryId, string categorySlug,
            CancellationToken cancellationToken = default)
        {
            return await _context.Set<Category>()
                .AnyAsync(x => x.Id != categoryId && x.UrlSlug == categorySlug, cancellationToken);
        }*/


        /*public async Task<Tag> GetTagAsync(
            string slug, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Tag>()
                .FirstOrDefaultAsync(x => x.UrlSlug == slug, cancellationToken);
        }*/

        /*public async Task<bool> DeleteTagAsync(
            int tagId, CancellationToken cancellationToken = default)
        {
            //var tag = await _context.Set<Tag>().FindAsync(tagId);

            //if (tag == null) return false;

            //_context.Set<Tag>().Remove(tag);
            //return await _context.SaveChangesAsync(cancellationToken) > 0;

            return await _context.Set<Tag>()
                .Where(x => x.Id == tagId)
                .ExecuteDeleteAsync(cancellationToken) > 0;
        }*/

        /*public async Task<bool> CreateOrUpdateTagAsync(
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
        }*/

        //public async Task<Post> GetPostAsync(
        //    string slug,
        //    CancellationToken cancellationToken = default)
        //{
        //    var postQuery = new PostQuery()
        //    {
        //        PublishedOnly = false,
        //        TitleSlug = slug
        //    };

        //    return await FilterPosts(postQuery).FirstOrDefaultAsync(cancellationToken);
        //}

        // i.l) Tìm bài viết theo Id
        public async Task<Post> GetPostByIdAsync(
            int postId, bool includeDetails = false,
            CancellationToken cancellationToken = default)
        {
            if (!includeDetails)
            {
                return await _context.Set<Post>().FindAsync(postId);
            }

            return await _context.Set<Post>()
                .Include(x => x.Category)
                .Include(x => x.Author)
                .Include(x => x.Tags)
                .FirstOrDefaultAsync(x => x.Id == postId, cancellationToken);
        }

        /*public async Task<bool> TogglePublishedFlagAsync(
            int postId, CancellationToken cancellationToken = default)
        {
            var post = await _context.Set<Post>().FindAsync(postId);

            if (post is null) return false;

            post.Published = !post.Published;
            await _context.SaveChangesAsync(cancellationToken);

            return post.Published;
        }*/

        // Hiển thị TOP 5 bài viết ngẫu nhiên
        /*public async Task<IList<Post>> GetRandomArticlesAsync(
            int numPosts, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Post>()
                .OrderBy(x => Guid.NewGuid())
                .Take(numPosts)
                .ToListAsync(cancellationToken);
        }
        // Hiển thị TOP 4 tác giả có nhiều bài viết nhất.
        public async Task<IList<Author>> GetPopularAuthorsAsync(int numAuthor, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Author>()
                .OrderByDescending(p => p.Posts)
                .Take(numAuthor)
                .ToListAsync(cancellationToken);
        }
        */

        /*Task<List<CategoryItem>> IBlogRepository.GetCategoriesAsync(bool showOnMenu, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }*/

        // 1.q) Tìm tất cả bài viết thỏa mãn điều kiện (postquery)
        /*public Task<IList<Post>> GetPostsAsync(PostQuery condition, int pageNumber, int pageSize, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }*/

        // 1.s) Tìm và phân trang các bài viết thỏa mãn điều kiện (postquery)
        public async Task<IPagedList<Post>> GetPagedPostsAsync(
            PostQuery condition,
            int pageNumber = 1,
            int pageSize = 10,
            CancellationToken cancellationToken = default)
        {
            return await FilterPosts(condition).ToPagedListAsync(
                pageNumber, pageSize,
                nameof(Post.PostedDate), "DESC",
                cancellationToken);
        }

        // 1.t) Tìm và phân trang các bài viết thỏa mãn điều kiện (postquery) + trả về IPagedList<T> 
        public async Task<IPagedList<T>> GetPagedPostsAsync<T>(
            PostQuery condition,
            IPagingParams pagingParams,
            Func<IQueryable<Post>, IQueryable<T>> mapper)
        {
            var posts = FilterPosts(condition);
            var projectedPosts = mapper(posts);

            return await projectedPosts.ToPagedListAsync(pagingParams);
        }

        // 1.m) Thêm/Cập nhật bài viết
        //public async Task<Post> CreateOrUpdatePostAsync(
        //    Post post, IEnumerable<string> tags,
        //    CancellationToken cancellationToken = default)
        //{
        //    if (post.Id > 0)
        //    {
        //        await _context.Entry(post).Collection(x => x.Tags).LoadAsync(cancellationToken);
        //    }
        //    else
        //    {
        //        post.Tags = new List<Tag>();
        //    }

        //    var validTags = tags.Where(x => !string.IsNullOrWhiteSpace(x))
        //        .Select(x => new
        //        {
        //            Name = x,
        //            Slug = x.GenerateSlug()
        //        })
        //        .GroupBy(x => x.Slug)
        //        .ToDictionary(g => g.Key, g => g.First().Name);

        //    foreach (var kv in validTags)
        //    {
        //        if (post.Tags.Any(x => string.Compare(x.UrlSlug, kv.Key, StringComparison.InvariantCultureIgnoreCase) == 0)) continue;

        //        var tag = await GetTagAsync(kv.Key, cancellationToken) ?? new Tag()
        //        {
        //            Name = kv.Value,
        //            Description = kv.Value,
        //            UrlSlug = kv.Key
        //        };

        //        post.Tags.Add(tag);
        //    }

        //    post.Tags = post.Tags.Where(t => validTags.ContainsKey(t.UrlSlug)).ToList();

        //    if (post.Id > 0)
        //        _context.Update(post);
        //    else
        //        _context.Add(post);

        //    await _context.SaveChangesAsync(cancellationToken);

        //    return post;
        //}

        //==========================================================//
        // 1.o) Lấy ngẫu nhiên N bài viết
        public async Task<IList<Post>> GetRandomPostsAsync(int randomOfPosts, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Post>()
                .Include(a => a.Author)
                .Include(c => c.Category)
                .Include(t => t.Tags)
                .Where(x => x.Published)
                .OrderBy(x => Guid.NewGuid())
                .Take(randomOfPosts)
                .ToListAsync(cancellationToken);
        }




        public async Task<Tag> GetTagSlugAsync(string slug, CancellationToken cancellationToken = default)
        {
            IQueryable<Tag> tagsQuery = _context.Set<Tag>();


            if (!string.IsNullOrEmpty(slug))
            {
                tagsQuery = tagsQuery.Where(x => x.UrlSlug == slug);
            }

            return await tagsQuery.FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<bool> DeleteTagByNameAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Tag>()
                .Where(x => x.Id == id)
                .ExecuteDeleteAsync(cancellationToken) > 0;
        }



        public async Task<Category> GetCategoryBySlugAsync(string slug, CancellationToken cancellationToken = default)
        {
            IQueryable<Category> categories = _context.Set<Category>();

            return await categories
                .FirstOrDefaultAsync(x => x.UrlSlug.ToLower().Contains(slug.ToLower()), cancellationToken);
        }

        public async Task<Category> GetCategoryByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            IQueryable<Category> categories = _context.Set<Category>();

            return await categories
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public async Task<bool> IsCategorySlugExistedAsync(string slug, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Category>()
                .AnyAsync(x => x.UrlSlug.CompareTo(slug) == 0, cancellationToken);
        }

        public async Task<IPagedList<CategoryItem>> GetPagedCategoriesAsync(IPagingParams pagingParams, CancellationToken cancellationToken = default)
        {
            var categoryQuery = _context.Set<Category>()
                .Select(x => new CategoryItem()
                {
                    Id = x.Id,
                    Name = x.Name,
                    UrlSlug = x.UrlSlug,
                    Description = x.Description,
                    PostCount = x.Posts.Count(p => p.Published)
                });

            return await categoryQuery.ToPagedListAsync(pagingParams, cancellationToken);
        }

        /*public async Task<IList<Author>> GetAuthorsAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Set<Author>()
                .ToListAsync(cancellationToken);
        }*/

        public async Task<Post> GetPostByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Post>().FindAsync(id);
        }

        //==========================================================//



        private IQueryable<Post> FilterPosts(PostQuery condition)
        {
            IQueryable<Post> posts = _context.Set<Post>()
                .Include(x => x.Category)
                .Include(x => x.Author)
                .Include(x => x.Tags);

            if (condition.PublishedOnly)
            {
                posts = posts.Where(x => x.Published);
            }

            if (condition.NotPublished)
            {
                posts = posts.Where(x => !x.Published);
            }

            if (condition.CategoryId > 0)
            {
                posts = posts.Where(x => x.CategoryId == condition.CategoryId);
            }

            if (!string.IsNullOrWhiteSpace(condition.CategorySlug))
            {
                posts = posts.Where(x => x.Category.UrlSlug == condition.CategorySlug);
            }

            if (condition.AuthorId > 0)
            {
                posts = posts.Where(x => x.AuthorId == condition.AuthorId);
            }

            if (!string.IsNullOrWhiteSpace(condition.AuthorSlug))
            {
                posts = posts.Where(x => x.Author.UrlSlug == condition.AuthorSlug);
            }

            if (!string.IsNullOrWhiteSpace(condition.TagSlug))
            {
                posts = posts.Where(x => x.Tags.Any(t => t.UrlSlug == condition.TagSlug));
            }

            if (!string.IsNullOrWhiteSpace(condition.Keyword))
            {
                posts = posts.Where(x => x.Title.Contains(condition.Keyword) ||
                                         x.ShortDescription.Contains(condition.Keyword) ||
                                         x.Description.Contains(condition.Keyword) ||
                                         x.Category.Name.Contains(condition.Keyword) ||
                                         x.Tags.Any(t => t.Name.Contains(condition.Keyword)));
            }

            if (condition.Year > 0)
            {
                posts = posts.Where(x => x.PostedDate.Year == condition.Year);
            }

            if (condition.Month > 0)
            {
                posts = posts.Where(x => x.PostedDate.Month == condition.Month);
            }

            if (!string.IsNullOrWhiteSpace(condition.TitleSlug))
            {
                posts = posts.Where(x => x.UrlSlug == condition.TitleSlug);
            }

            return posts;

            //    //// Compact version
            //    //return _context.Set<Post>()
            //    //	.Include(x => x.Category)
            //    //	.Include(x => x.Author)
            //    //	.Include(x => x.Tags)
            //    //	.WhereIf(condition.PublishedOnly, x => x.Published)
            //    //	.WhereIf(condition.NotPublished, x => !x.Published)
            //    //	.WhereIf(condition.CategoryId > 0, x => x.CategoryId == condition.CategoryId)
            //    //	.WhereIf(!string.IsNullOrWhiteSpace(condition.CategorySlug), x => x.Category.UrlSlug == condition.CategorySlug)
            //    //	.WhereIf(condition.AuthorId > 0, x => x.AuthorId == condition.AuthorId)
            //    //	.WhereIf(!string.IsNullOrWhiteSpace(condition.AuthorSlug), x => x.Author.UrlSlug == condition.AuthorSlug)
            //    //	.WhereIf(!string.IsNullOrWhiteSpace(condition.TagSlug), x => x.Tags.Any(t => t.UrlSlug == condition.TagSlug))
            //    //	.WhereIf(!string.IsNullOrWhiteSpace(condition.Keyword), x => x.Title.Contains(condition.Keyword) ||
            //    //	                                                             x.ShortDescription.Contains(condition.Keyword) ||
            //    //	                                                             x.Description.Contains(condition.Keyword) ||
            //    //	                                                             x.Category.Name.Contains(condition.Keyword) ||
            //    //	                                                             x.Tags.Any(t => t.Name.Contains(condition.Keyword)))
            //    //	.WhereIf(condition.Year > 0, x => x.PostedDate.Year == condition.Year)
            //    //	.WhereIf(condition.Month > 0, x => x.PostedDate.Month == condition.Month)
            //    //	.WhereIf(!string.IsNullOrWhiteSpace(condition.TitleSlug), x => x.UrlSlug == condition.TitleSlug);
            //}

            #endregion


        }

        public Task CreateOrUpdatePostAsync(object post, List<string> list)
        {
            throw new NotImplementedException();
        }

        public Task GetPostAsync(PostQuery postQuery)
        {
            throw new NotImplementedException();
        }

        public Task<IList<MonthPostCount>> CountMonthPostsAsync(int numMonths, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeletePostById(int id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}