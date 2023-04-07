﻿using MapsterMapper;
using System.Net;
using TatBlog.Core.Collections;
using TatBlog.Core.DTO;
using TatBlog.Services.Blogs;
using TatBlog.WebApi.Models;

namespace TatBlog.WebApi.Endpoints;

public static class TagEndpointscs
{
    public static WebApplication MapTagEndpoints(
      this WebApplication app)
    {
        var routeGroupBuilder = app.MapGroup("/api/tags");

        routeGroupBuilder.MapGet("/", GetTag)
           .WithName("GetTag")
           .Produces<ApiResponse<PaginationResult<TagItem>>>();

        routeGroupBuilder.MapGet("/{id:int}", GetTagById)
            .WithName("GetTagById")
            .Produces<ApiResponse<TagItem>>();

        routeGroupBuilder.MapGet("/random/{number:int}", GetRandomTags)
            .WithName("GetRandomTag")
            .Produces<ApiResponse<IList<TagItem>>>();

        routeGroupBuilder.MapDelete("/{id:int}", DeleteTag)
           .WithName("DeleteTag")
           .Produces(401)
           .Produces<ApiResponse<string>>();

        return app;
    }

    private static async Task<IResult> GetTag(
         [AsParameters] TagFilterModel model,
         IBlogRepository blogRepository)
    {
        var tagList = await blogRepository.GetPagedTagsAsync(model);
        var paginationResult = new PaginationResult<TagItem>(tagList);

        return Results.Ok(ApiResponse.Success(paginationResult));
    }

    private static async Task<IResult> GetTagById(int id, IBlogRepository blogRepository, IMapper mapper)
    {
        var tag = await blogRepository.GetTagByIdAsync(id);

        return tag == null

            ? Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, $"Không tìm thấy thẻ có mã số {id}"))
            : Results.Ok(ApiResponse.Success(mapper.Map<TagItem>(tag)));
    }

    private static async Task<IResult> GetRandomTags(int number, IBlogRepository blogRepository, IMapper mapper)
    {
        var tags = await blogRepository.GetRandomPostsAsync(number);

        return tags.Count != 0

            ? Results.Ok(ApiResponse.Success(
                mapper.Map<IList<PostDto>>(tags)))
            : Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, "Không tìm thấy thẻ"));
    }

    private static async Task<IResult> DeleteTag(int id, IBlogRepository blogRepository)
    {
        return await blogRepository.DeleteTagByIdAsync(id)
            ? Results.Ok(ApiResponse.Success("Xóa thành công", HttpStatusCode.NoContent))

            : Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, $"Không thể tìm thấy thẻ có mã số {id}")); 
    }

}
