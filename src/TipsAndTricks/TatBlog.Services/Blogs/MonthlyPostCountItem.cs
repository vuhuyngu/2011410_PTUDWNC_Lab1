namespace TatBlog.Services.Blogs
{
    internal class MonthlyPostCountItem
    {
        public MonthlyPostCountItem()
        {
        }

        public int Year { get; set; }
        public int Month { get; set; }
        public int PostCount { get; set; }
    }
}