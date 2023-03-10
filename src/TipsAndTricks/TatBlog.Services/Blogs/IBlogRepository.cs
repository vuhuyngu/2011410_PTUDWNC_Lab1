using TatBlog.Core.DTO;
using TatBlog.Core.Entities;

namespace TatBlog.Services.Blogs;

public interface IBlogRepository
{
    Task<Post> GetPostAsync(
        int year,
        int month,
        string slug,
        CancellationToken cancellationToken = default);

    Task<IList<Post>> GetPopularArticleAsync(
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
}
