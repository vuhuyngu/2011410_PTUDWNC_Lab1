using System.Diagnostics.Eventing.Reader;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;
using TatBlog.WebApp.Areas.Admin.Models;

using TatBlog.Services.Blogs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace TatBlog.WebApp.Areas.Admin.Controllers;

public class PostsController : Controller
{
    private readonly IBlogRepository _blogRepository;

    public PostsController(IBlogRepository blogRepository)
    {
        _blogRepository = blogRepository;
    }

    public string Keyword { get; private set; }
    public int? CategoryId { get; private set; }
    public int? AuthorId { get; private set; }
    public int? Year { get; private set; }
    public int? Month { get; private set; }

    public async Task<IActionResult> Index(PostFilterModel model)
    {
        var postQuery = new PostQuery();
        {
            Keyword = model.Keyword;
            CategoryId = model.CategoryId;
            AuthorId = model.AuthorId;
            Year = model.Year;
            Month = model.Month;
        };

        ViewBag.PostsList = await _blogRepository
            .GetPagedPostsAsync(postQuery, 1, 10);

        await PopulatePostFilterModelAsync(model);

        return View(model);
    }

    private async Task PopulatePostFilterModelAsync(PostFilterModel model)
    {
        var authors = await _blogRepository.GetAuthorsAsync();
        var categories = await _blogRepository.GetCategoriesAsync();

        model.AuthorList = authors.Select(a => new SelectListItem()
        {
            Text = a.FullName,
            Value = a.Id.ToString()
        });

        model.CategoryList = categories.Select(c => new SelectListItem()
        {
            Text = c.Name,
            Value = c.Id.ToString()
        });
    }
}
