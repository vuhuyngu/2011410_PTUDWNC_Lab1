using Microsoft.AspNetCore.Mvc;
using TatBlog.Services.Blogs;
using TatBlog.Core.DTO;
using TatBlog.Core.Contracts;

namespace TatBlog.WebApp.Controllers
{
    public class BlogController : Controller
    {
		private readonly IBlogRepository _blogRepository;

		public BlogController(IBlogRepository blogRepository)
		{
			_blogRepository = blogRepository;
		}

		public async Task<IActionResult> Index(
			[FromQuery(Name = "k")] string? keyword = null,
			[FromQuery(Name = "p")] int pageNumber = 1,
			[FromQuery(Name = "ps")] int pageSize = 3)
		{
			var postQuery = new PostQuery()
			{
				PublishedOnly = true,

				Keyword = keyword

			};

			var postsList = await _blogRepository
				.GetPagedPostsAsync(postQuery, pageNumber, pageSize);


			ViewBag.PostQuery = postQuery;

			return View(postsList);
		}


		// Bài 2
		public async Task<IActionResult> Tag([FromRoute(Name = "slug")] string slug)
		{
			var postQuery = new PostQuery()
			{
				TagSlug = slug,
			};
			ViewBag.PostQuery = postQuery;
			var postList = await _blogRepository.GetPagedPostsAsync(postQuery);
			return View("Index", postList);
		}

		public async Task<IActionResult> Category([FromRoute(Name = "slug")] string slug)
		{
			var postQuery = new PostQuery()
			{
				CategorySlug = slug,
			};
			ViewBag.PostQuery = postQuery; ;
			var postList = await _blogRepository.GetPagedPostsAsync(postQuery);
			return View("Index", postList);
		}

		public async Task<IActionResult> Author([FromRoute(Name = "slug")] string slug)
		{
			var postQuery = new PostQuery()
			{
				AuthorSlug = slug,
			};
			ViewBag.PostQuery = postQuery; ;
			var postList = await _blogRepository.GetPagedPostsAsync(postQuery);
			return View("Index", postList);
		}





		/*public IActionResult Index() 
        {
            ViewBag.CurrentTime = DateTime.Now.ToString("HH:mm:ss");

            return View();
        }*/

		/*public async Task<IActionResult> Post([FromRoute(Name = "year")] int year = 2023,
											[FromRoute(Name = "month")] int month = 3,
											[FromRoute(Name = "day")] int day = 18,
											[FromRoute(Name = "slug")] string slug = null)
		{
			var postQuery = new PostQuery()
			{
				Month = month,
				Year = year,
				Day = day,
				UrlSlug = slug,
			};

			ViewBag.PostQuery = postQuery;
			var posts = await _blogRepository.GetPostAsync(postQuery);
			try
			{
				if (posts != null && !posts.Published)
				{
					await _blogRepository.IncreaseViewCountAsync(posts.Id);
				}
			}
			catch (NullReferenceException)
			{
				return View("Error");
			}
			return View("DetailPost", posts);
		}

		public async Task<IActionResult> Archives([FromRoute(Name = "year")] int year = 2021,
                                                    [FromRoute(Name = "month")] int month = 9)
        {
            var postQuery = new PostQuey()
            {
                Year = year,
                Month = month
            };
            var posts = _blogRepository.GetPagedPostsAsync(postQuery);
            return View();
        }*/

        public IActionResult About()
            => View();

        public IActionResult Contact()
            => View();

        public IActionResult Rss()
            => Content("Nội dung sẽ được cập nhật");
    }
}


