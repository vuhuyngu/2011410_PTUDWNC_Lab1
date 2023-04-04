﻿using FluentValidation;
using TatBlog.WebApi.Models;

namespace TatBlog.WebApi.Validations;

public class CategoryValidator : AbstractValidator<CategoryEditModel>
{
    public CategoryValidator()
    {


        RuleFor(a => a.Name)
            .NotEmpty()
            .WithMessage("UrlSlug không được để trống")
            .MaximumLength(100)
            .WithMessage("UrlSlug tối đa 100 ký tự");

        RuleFor(a => a.Description)
            .NotEmpty()
            .WithMessage("Email không được để trống")
            .MaximumLength(100)
            .WithMessage("Email chứa tối đa 100 ký tự");

        RuleFor(a => a.UrlSlug)
            .NotEmpty()
            .WithMessage("Email không được để trống")
            .MaximumLength(100)
            .WithMessage("Email chứa tối đa 100 ký tự");

    }
}
