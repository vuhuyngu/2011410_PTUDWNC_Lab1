using TatBlog.Core.Entities;

namespace TatBlog.Services.Blogs;

public class BlogRepository : IBlogRepository
{
    public Task<Post> GetPostAsync(
        int year,
        int month,
        string slug,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<IList<Post>> GetPopularArticlesAsync(
        int numPosts, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<IList<Post>> GetPopularArticlesAsync(
        int numPosts, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<bool> IsPostSlugExistedAsync(
        int postId,
        string slug,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task IncreaseViewCountAsync(
        int postId,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
