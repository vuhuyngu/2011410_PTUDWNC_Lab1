
using TatBlog.WebApp.Extensions;

var builder = WebApplication.CreateBuilder(args);
{
    // Add services to the container.
    builder
        .ConfigureCors()
        .ConfigureNLog()
        .ConfigureServices()
        .ConfigureSwaggerOpenApi();
}

var app = builder.Build();
{
    // Configure the HTTP request pipeline.
    app.SetupRequestPipeLine();
    app.Run();
}


