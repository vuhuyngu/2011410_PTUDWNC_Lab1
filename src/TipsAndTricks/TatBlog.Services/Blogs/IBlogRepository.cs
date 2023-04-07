using TatBlog.Core.Contracts;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;

namespace TatBlog.Services.Blogs;

public interface IBlogRepository
{
    /*Task<Post> GetPostAsync(
        int year,
        int month,
        string slug,
        CancellationToken cancellationToken = default);

    Task<IList<Post>> GetPopularArticlesAsync(
        int numPosts,
        CancellationToken cancellationToken = default);

    Task<bool> IsPostSlugExistedAsync(
        int postId, string slug,
        CancellationToken cancellationToken = default);

    Task IncreaseViewCountAsync(
        int postId,
        CancellationToken cancellationToken = default);

    Task<List<CategoryItem>> GetCategoriesAsync(
        bool showOnMenu = false,
        CancellationToken cancellationToken = default);
    Task<IPagedList<TagItem>> GetPagedTagsAsync(
        IPagingParams pagingParams, CancellationToken cancellationToken = default);
    Task<IList<Post>> GetPostsAsync(PostQuery condition, int pageNumber, int pageSize, CancellationToken cancellationToken = default);
*/
    Task<Post> GetPostAsync(
        string slug, 
        CancellationToken cancellationToken = default);

    Task<Post> GetPostByIdAsync(
        int postId, 
        bool includeDetails = false, 
        CancellationToken cancellationToken = default);

    Task<IList<MonthlyPostCountItem>> CountMonthlyPostsAsync(
        int numMonths, 
        CancellationToken cancellationToken = default);

    Task<IList<Post>> GetRandomPostsAsync(
        int randomOfPosts, 
        CancellationToken cancellationToken = default);

    Task<IList<Post>> GetPopularArticlesAsync(
        int numPosts, 
        CancellationToken cancellationToken = default);

    Task<bool> IsPostSlugExistedAsync(
        int postId, 
        string slug, 
        CancellationToken token = default);

    Task IncreaseViewCountAsync(
        int postId, 
        CancellationToken cancellationToken = default);

    Task<IList<CategoryItem>> GetCategoriesAsync(
        bool showOnMenu = false, 
        CancellationToken cancellationToken = default);

    Task<IPagedList<TagItem>> GetPagedTagsAsync(
        IPagingParams pagingParams,
        CancellationToken cancellationToken = default);

    Task<IPagedList<Post>> GetPagedPostsAsync(
        PostQuery condition,
        int pageNumber = 1,
        int pageSize = 10,
        CancellationToken cancellationToken = default);

    Task<IPagedList<T>> GetPagedPostsAsync<T>(
        PostQuery condition,
        IPagingParams pagingParams,
        Func<IQueryable<Post>,
        IQueryable<T>> mapper);

    Task<IPagedList<CategoryItem>> GetPagedCategoriesAsync(
        IPagingParams pagingParams,
        CancellationToken cancellationToken = default);

    Task<Tag> GetTagSlugAsync(
        string slug, 
        CancellationToken cancellationToken = default);

    Task<bool> DeleteTagByNameAsync(
        int id, 
        CancellationToken cancellationToken = default);

    Task<IList<TagItem>> GetTagsAsync(
        CancellationToken cancellationToken = default);

    Task<Category> GetCategoryBySlugAsync(
        string slug, 
        CancellationToken cancellationToken = default);

    Task<Category> GetCategoryByIdAsync(
        int id, 
        CancellationToken cancellationToken = default);

    Task<bool> IsCategorySlugExistedAsync(
        string slug, 
        CancellationToken cancellationToken = default);

    /*Task<IList<Author>> GetAuthorsAsync(
        CancellationToken cancellationToken = default);*/

        Task CreateOrUpdatePostAsync(object post, List<string> list);
    Task GetPostAsync(PostQuery postQuery);

    Task<IList<MonthPostCount>> CountMonthPostsAsync(
        int numMonths, CancellationToken cancellationToken = default);

    Task<bool> DeletePostById(
        int id, CancellationToken cancellationToken = default);

    Task<Tag> GetTagByIdAsync(
        int id, CancellationToken cancellationToken = default);

    Task<bool> DeleteTagByIdAsync(
        int id, CancellationToken cancellationToken = default);
}
