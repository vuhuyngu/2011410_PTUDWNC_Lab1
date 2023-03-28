using System.Diagnostics.Eventing.Reader;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;
using TatBlog.WebApp.Areas.Admin.Models;

using TatBlog.Services.Blogs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MapsterMapper;
using TatBlog.Services.Media;
using FluentValidation;
using FluentValidation.AspNetCore;

namespace TatBlog.WebApp.Areas.Admin.Controllers;

public class PostsController : Controller
{
    private readonly ILogger<PostsController> _logger;
    private readonly IBlogRepository _blogRepository;
    private readonly IAuthorRepository _authorRepository;
    private readonly IMediaManager _mediaManager;
    private readonly IMapper _mapper;

    public PostsController(
        ILogger<PostsController> logger,
        IBlogRepository blogRepository,
        IAuthorRepository authorRepository,
        IMediaManager mediaManager,
        IMapper mapper)
    {
        _logger = logger;
        _blogRepository = blogRepository;
        _authorRepository = authorRepository;
        _mediaManager = mediaManager;
        _mapper = mapper;
    }


    public string? Keyword { get; private set; }
    public int? CategoryId { get; private set; }
    public int? AuthorId { get; private set; }
    public int? Year { get; private set; }
    public int? Month { get; private set; }

    public async Task<IActionResult> Index(PostFilterModel model)
    {
        _logger.LogInformation("Tạo điều kiện truy vấn");

        // Sử dụng Mapster để tạo đối tượng PostQuery
        // từ đối tượng PostFillterModel model
        var postQuery = _mapper.Map<PostQuery>(model);

        ViewBag.PostsList = await _blogRepository
            .GetPagedPostsAsync(postQuery, 1, 10);

        _logger.LogInformation("Chuẩn bị dữ liệu cho ViewModel");

        await PopulatePostFilterModelAsync(model);

        return View(model);
    }

    private async Task PopulatePostFilterModelAsync(PostFilterModel model)
    {
        var authors = await _authorRepository.GetAuthorsAsync();
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

    public async Task<IActionResult> Edit(int id = 0)
    {
        // ID = 0 <=> Thêm bài viết mới
        // ID > 0 : Đọc dữ liệu của bài viết từ CSDL
        var post = id > 0
            ? await _blogRepository.GetPostByIdAsync(id, true)
            : null;

        // Tạo view model từ dữ liệu của bài viết
        var model = post == null
            ? new PostEditModel()
            : _mapper.Map<PostEditModel>(post);

        // Gán các giá trị khác cho view model
        await PopulatePostEditModelAsync(model);

        return View(model);
    }

    private Task PopulatePostEditModelAsync(PostEditModel model)
    {
        throw new NotImplementedException();
    }

    public async Task<IActionResult> Edit(
        IValidator<PostEditModel> postValidator,
        PostEditModel model)
    {
        var validationResult = await postValidator.ValidateAsync(model);

        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState);
        }

        if (!ModelState.IsValid)
        {
            await PopulatePostEditModelAsync(model);
            return View(model);
        }

        var post = model.Id > 0
            ? await _blogRepository.GetPostByIdAsync(model.Id, true)
            : null;

        if (post == null)
        {
            post = _mapper.Map<Post>(model);

            post.Id = 0;
            post.PostedDate = DateTime.Now;
        }
        else
        {
            _mapper.Map(model, post);

            post.Category = null;
            post.ModifiedDate = DateTime.Now;
        }

        // Nếu người dùng có upload hình ảnh minh họa cho bài viết
        if (model.ImageFile?.Length > 0)
        {
            // Thì thực hiện việc lưu tập tin vào thư mục uploads
            var newImagePath = await _mediaManager.SaveFileAsync(
                model.ImageFile.OpenReadStream(),
                model.ImageFile.FileName,
                model.ImageFile.ContentType);

            // Nếu lưu thành công, xóa tập tin hình ảnh cũ (nếu có)
            if (!string.IsNullOrWhiteSpace(newImagePath))
            {
                await _mediaManager.DeleteFileAsync(post.ImageUrl);
                post.ImageUrl = newImagePath;
            }
        }

        await _blogRepository.CreateOrUpdatePostAsync(
            post, model.GetSelectedTags());

        return RedirectToAction(nameof(Index));

    }

    public async Task<IActionResult> VerifyPostSlug(
            int id, string urlSlug)
    {
        var slugExitsted = await _blogRepository
            .IsPostSlugExistedAsync(id, urlSlug);

        return slugExitsted
            ? Json($"Slug '{urlSlug}' đã được sử dụng")
            : Json(true);
    }
}
