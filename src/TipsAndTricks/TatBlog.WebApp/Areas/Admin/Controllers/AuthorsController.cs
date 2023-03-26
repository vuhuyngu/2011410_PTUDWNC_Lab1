using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using TatBlog.Core.Collections;
using TatBlog.Core.Contracts;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;
using TatBlog.Services.Blogs;

using TatBlog.WebApp.Areas.Admin.Models;

namespace TatBlog.WebApp.Areas.Admin.Controllers
{
    public class AuthorsController : Controller
    {
        private readonly IBlogRepository _blogRepository;
        private readonly IMapper _mapper;
        private readonly IAuthorRepository _authorRepository;

        public AuthorsController(
            IMapper mapper,
            IBlogRepository blogRepository,
            IAuthorRepository authorRepository)
        {
            _blogRepository = blogRepository;
            _authorRepository = authorRepository;
            _mapper = mapper;
        }

        public async Task<IActionResult> Edit(int id = 0)
        {
            var author = id > 0
                ? await _authorRepository.GetAuthorByIdAsync(id)
                : null;

            var model = author == null
                ? new AuthorEditModel()
                : _mapper.Map<AuthorEditModel>(author);

            return View(model);
        }

        public async Task<IActionResult> Index(
            AuthorFilterModel model, 
            [FromQuery(Name = "p")] int pageNumber = 1, 
            [FromQuery(Name = "ps")] int pageSize = 5)
        {
            IPagingParams pagingParams = new PagingParams()
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
            };
            var authorQuery = _mapper.Map<AuthorQuery>(model);

            ViewBag.AuthorsList = await _authorRepository
                .GetPagedAuthorsAsync(pagingParams);
            return View(model);
        }

        public async Task<IActionResult> Edit(
            AuthorEditModel model)
        {

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var author = model.Id > 0
                ? await _authorRepository.GetAuthorByIdAsync(model.Id)
                : null;

            if (author == null)
            {
                author = _mapper.Map<Author>(model);
                author.Id = 0;
                author.JoinedDate = DateTime.Now;
            }
            else
            {
                _mapper.Map(model, author);
            }

            await _authorRepository.AddOrUpdateAsync(author);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> DeleteAuthor(int id)
        {
            await _authorRepository.DeleteAuthorAsync(id);
            return RedirectToAction("Index");
        }
    }
}
