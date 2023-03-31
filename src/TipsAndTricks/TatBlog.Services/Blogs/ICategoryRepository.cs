using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TatBlog.Core.Contracts;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;

namespace TatBlog.Services.Blogs
{
    public interface ICategoryRepository
    {
        Task<Author> GetCategoryBySlugAsync(
        string slug,
        CancellationToken cancellationToken = default);

        Task<IPagedList<CategoryItem>>
            GetPagedCategoriesAsync(IPagingParams pagingParams, string name = null,
            CancellationToken cancellation  = default);

        Task<Category> GetCachedCategoryByIdAsync(
            int categoryId,
            CancellationToken cancellationToken = default);

        Task<bool> IsCategorySlugExistedAsync(
            int categoryId,
            string slug,
            CancellationToken cancellationToken = default);

        Task<bool> AddOrUpdateAsync(
            Category category,
            CancellationToken cancellationToken = default);

        Task<bool> DeleteCategoryAsync(
            int categoryId,
            CancellationToken cancellationToken = default);
    }
}
