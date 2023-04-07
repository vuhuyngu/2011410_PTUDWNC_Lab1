﻿using Microsoft.EntityFrameworkCore;
using NLog.Web;
using System.Runtime.CompilerServices;
using TatBlog.Data.Contexts;
using TatBlog.Data.Seeders;
using TatBlog.Services.Blogs;
using TatBlog.Services.Media;
using TatBlog.Services.Timing;

namespace TatBlog.WebApp.Extensions;

public static class WebApplicationExtensions
{
    public static WebApplicationBuilder ConfigureServices(
        this WebApplicationBuilder builder)
    {
        builder.Services.AddMemoryCache();

        builder.Services.AddDbContext<BlogDbContext>(options =>
            options.UseSqlServer(
                 builder.Configuration
                 .GetConnectionString("DefaultConnection")));

        builder.Services
            .AddScoped<ITimeProvider, LocalTimeProvider>();
        builder.Services
            .AddScoped<IMediaManager, LocalFileSystemMediaManager>();
        builder.Services
            .AddScoped<IBlogRepository, BlogRepository>();
        builder.Services
            .AddScoped<IAuthorRepository, AuthorRepository>();
        builder.Services
            .AddScoped<ICategoryRepository, CategoryRepository>();
        builder.Services
            .AddScoped<ISubscriberRepository, SubscriberRepository>();

        return builder;
    }

    public static WebApplicationBuilder ConfigureCors(
        this WebApplicationBuilder builder)
    {
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("TatBlogApp", policyBuilder =>
                policyBuilder
                     .AllowAnyOrigin()
                     .AllowAnyHeader()
                     .AllowAnyMethod());
        });

        return builder;
    }

    // Cấu hình việc sử dụng Nlog
    public static WebApplicationBuilder ConfigureNLog(
        this WebApplicationBuilder builder)
    {
        builder.Logging.ClearProviders();
        builder.Host.UseNLog();

        return builder;
    }

    public static WebApplicationBuilder ConfigureSwaggerOpenApi(
        this WebApplicationBuilder builder)
    {
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        return builder;
    }

    public static WebApplication SetupRequestPipeLine(
        this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseStaticFiles();
        app.UseHttpsRedirection();

        app.UseCors("TatBlogApp");

        return app;
    }
}
