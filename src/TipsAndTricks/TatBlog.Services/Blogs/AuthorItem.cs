namespace TatBlog.Services.Blogs
{
    public class AuthorItem
    {
        public string FullName { get; internal set; }
        public string Email { get; internal set; }
        public int Id { get; internal set; }
        public DateTime JoinedDate { get; internal set; }
        public string ImageUrl { get; internal set; }
        public string UrlSlug { get; internal set; }
        public string Notes { get; internal set; }
        public int PostCount { get; internal set; }
    }
}