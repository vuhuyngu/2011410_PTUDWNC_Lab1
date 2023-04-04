using TatBlog.Core.Collections;
using TatBlog.Core.DTO;
using TatBlog.Services.Blogs;
using TatBlog.Services.Media;
using TatBlog.WebApi.Filters;
using TatBlog.Core.Entities;
using TatBlog.WebApi.Extensions;
using TatBlog.WebApi.Models;
using FluentValidation;
using Mapster;
using MapsterMapper;
using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace TatBlog.WebApi.Endpoints;

public static class CategoryEndpoints
{
    public static WebApplication MapCategoryEndpoints(this WebApplication app) 
    {
        var routeGroupBuilder = app.MapGroup("/api/categories");

        routeGroupBuilder.MapGet("/", GetCategories)
            .WithName("GetCategories")
            .Produces<ApiResponse<PaginationResult<CategoryItem>>>();

        routeGroupBuilder.MapGet("/{id:int}", GetCategoryDetails)
                     .WithName("GetCategoryById")
                     .Produces<ApiResponse<CategoryItem>>()
                     .Produces(404);

        routeGroupBuilder.MapGet("/{slug::regex(^[a-z0-9_-]+$)}/posts", GetPostByCategorySlug)
                         .WithName("GetPostByCategorySlug")
                         .Produces<ApiResponse<PaginationResult<CategoryDto>>>();

        routeGroupBuilder.MapPost("/", AddCategory)
                         .AddEndpointFilter<ValidatorFilter<CategoryEditModel>>()
                         .WithName("AddNewCategory")
                         .RequireAuthorization()
                         .Produces(401)
                         .Produces<ApiResponse<CategoryItem>>();

        routeGroupBuilder.MapDelete("/{id:int}", DeleteCategory)
            .WithName("DeleteAnCategory")
            .Produces<ApiResponse<string>>()
            .Produces(401);

        routeGroupBuilder.MapPut("/{id:int}", UpdateCategory)
            .WithName("UpdateAnCategory")
            .Produces<ApiResponse<string>>()
            .Produces(401);

        return app;
    }

    public static async Task<IResult> GetCategories(
        [AsParameters] CategoryFilterModel model, ICategoryRepository categoryRepository)
    {
        var categoriesList = await categoryRepository.GetPagedCategoriesAsync(model, model.Name);
        var paginationResult = new PaginationResult<CategoryItem>(categoriesList);

        return Results.Ok(ApiResponse.Success(paginationResult));
    }

    private static async Task<IResult> GetCategoryDetails(int id, ICategoryRepository categoryRepository, IMapper mapper)
    {
        var category = await categoryRepository.GetCachedCategoryByIdAsync(id);

        return category == null ? Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound,
            $"Không tìm thấy chuyên mục có mã số {id}"))
            :
            Results.Ok(ApiResponse.Success(mapper.Map<CategoryItem>(category)));
    }
    
    private static async Task<IResult> GetPostByCategorySlug([FromRoute] string slug, [AsParameters] PagingModel pagingModel, IBlogRepository blogRepository)
    {
        var postQuery = new PostQuery
        {
            CategorySlug = slug,
            PublishedOnly = true
        };

        var categoryList = await blogRepository.GetPagedPostsAsync(postQuery, pagingModel, posts => posts.ProjectToType<CategoryDto>());

        var paginationResult = new PaginationResult<CategoryDto>(categoryList);

        return Results.Ok(ApiResponse.Success(paginationResult));
    }
    
    private static async Task<IResult> AddCategory(CategoryEditModel model, IValidator<CategoryEditModel> validator, ICategoryRepository categoryRepository, IMapper mapper)
    {
        if (await categoryRepository.IsCategorySlugExistedAsync(0, model.UrlSlug))
        {
            return Results.Ok(ApiResponse.Fail(HttpStatusCode.Conflict, $"Slug '{model.UrlSlug}' đã được sử dụng"));
        }

        var category = mapper.Map<Category>(model);
        await categoryRepository.AddOrUpdateAsync(category);

        return Results.Ok(ApiResponse.Success(mapper.Map<CategoryItem>(category), HttpStatusCode.Created));
    }

    private static async Task<IResult> DeleteCategory(
        int id, ICategoryRepository categoryRepository)
    {
        return await categoryRepository.DeleteCategoryAsync(id)
            ? Results.Ok(ApiResponse.Success("Category is deleted",
            HttpStatusCode.NoContent))
            : Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, "Could not find category"));
    }

    private static async Task<IResult> UpdateCategory(
        int id, CategoryEditModel model,
        IValidator<CategoryEditModel> validator,
        ICategoryRepository categoryRepository,
        IMapper mapper)
    {
        var validationResult = await validator.ValidateAsync(model);

        if (!validationResult.IsValid)
        {
            return Results.Ok(ApiResponse.Fail(
                HttpStatusCode.BadRequest, validationResult));
        }

        if (await categoryRepository
                .IsCategorySlugExistedAsync(id, model.UrlSlug))
        {
            return Results.Conflict(
                $"Slug '{model.UrlSlug}' đã được sử dụng");
        }

        var category = mapper.Map<Category>(model);
        category.Id = id;

        return await categoryRepository.AddOrUpdateAsync(category)
            ? Results.Ok(ApiResponse.Success("Category is updated",
            HttpStatusCode.NoContent))
            : Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound,
            "Could not find category"));
    }

}
