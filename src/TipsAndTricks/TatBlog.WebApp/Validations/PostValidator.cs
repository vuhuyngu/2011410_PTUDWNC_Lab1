﻿using FluentValidation;
using TatBlog.Services.Blogs;
using TatBlog.WebApp.Areas.Admin.Models;

namespace TatBlog.WebApp.Validations;

public class PostValidator : AbstractValidator<PostEditModel>
{
    private readonly IBlogRepository _blogRepository;

    public PostValidator(IBlogRepository blogRepository)
    {
        _blogRepository = blogRepository;

        RuleFor(x => x.Title)
            .NotEmpty()
            .MaximumLength(500)
            .WithMessage("Không được để trống phần tiêu đề");

        RuleFor(x => x.ShortDescription)
            .NotEmpty()
            .WithMessage("Không được để trống phần giới thiệu");

        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage("Không được để trống phần nội dung");

        RuleFor(x => x.Meta)
            .NotEmpty()
            .MaximumLength(1000)
            .WithMessage("Không được để trống Metadata");

        RuleFor(x => x.UrlSlug)
            .NotEmpty()
            .MaximumLength(1000)
            .WithMessage("Không được để trống Slug");

        RuleFor(x => x.UrlSlug)
            .MustAsync(async (postModel, slug, cancellationToken) =>
            !await blogRepository.IsPostSlugExistedAsync(
                postModel.Id, slug, cancellationToken))
                .WithMessage("Slug '{PropertyValue}' đã được sử dụng");

        RuleFor(x => x.CategoryId)
            .NotEmpty()
            .WithMessage("Bạn phải chọn chủ đề cho bài viết");

        RuleFor(x => x.AuthorId)
            .NotEmpty()
            .WithMessage("Bạn phải chọn tác giả của bài viết");

        RuleFor(x => x.SelectedTags)
            .Must(HasAtLeastOneTag)
            .WithMessage("Bạn phải nhập ít nhất một thẻ");

        When(x => x.Id <= 0, () =>
        {
            RuleFor(x => x.ImageFile)
                .Must(x => x is { Length: > 0 })
                .WithMessage("Bạn phải chọn hình ảnh cho bài viết");
        })
            .Otherwise(() =>
            {
                RuleFor(x => x.ImageFile)
                .MustAsync(SetImageIfNotExist)
                .WithMessage("Bạn phải chọn hình ảnh cho bài viết");
            });
    }

    // Kiểm tra xem người dùng đã nhập ít nhất 1 thẻ (Tag)
    private bool HasAtLeastOneTag(PostEditModel postModel, string selectedTags)
    {
        return postModel.GetSelectedTags().Any();
    }

    //Kiểm tra xem bài viết đã có hình ảnh chưa.
    //Nếu chưa có, bắt buộc người dùng phải chọn file.
    private async Task<bool> SetImageIfNotExist(
        PostEditModel postModel,
        IFormFile imageFile,
        CancellationToken cancellationToken)
    {
        //Lấy thông tin bài viết từ CSDL
        var post = await _blogRepository.GetPostByIdAsync(
            postModel.Id, false, cancellationToken);

        //Nếu bài viết đã có hình ảnh => không bắt buộc chọn file
        if (!string.IsNullOrWhiteSpace(post?.ImageUrl))
            return true;


        //Ngược lại (bài viết chưa có hình ảnh), kiểm tra xem
        //Người dùng đã chọn file hay chưa. Nêu chưa thì bào lỗi.
        return imageFile is { Length: > 0 };
    }
}
