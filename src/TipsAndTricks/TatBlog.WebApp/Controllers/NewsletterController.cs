using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using TatBlog.Services.Blogs;

namespace TatBlog.WebApp.Controllers
{
    public class NewsletterController : Controller
    {
        private readonly IBlogRepository _blogRepository;

        public string Email { get; set; }

        public NewsletterController(IBlogRepository blogRepository)
        {
            _blogRepository = blogRepository;
        }

       /* public async Task<IActionResult> Subscribe(string email)
        {
            return NotImplementedException();
        }
        public async Task<IActionResult> Unsubscribe(string email)
        {
            return NotImplementedException();
        }*/
    }
}
