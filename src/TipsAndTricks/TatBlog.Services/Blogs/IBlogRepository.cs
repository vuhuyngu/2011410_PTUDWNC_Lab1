using TatBlog.Core.DTO;
using TatBlog.Core.Entities;

namespace TatBlog.Services.Blogs;

public interface IBlogRepository
{
    // C, bài 1, câu a: Tìm một thẻ tag theo tên định danh "slug"
    Task<Tag> GetTagAsync(
        string slug,
        CancellationToken cancellationToken = default);


    // Tìm bài viết có tên định danh "slug" và được đăng vào ngày/tháng/năm
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

    // C, bài 1, câu b:
    Task<List<TagItem>> GetTagsAsync(
        bool showOnMenu = false,
        CancellationToken cancellationToken = default);
}
