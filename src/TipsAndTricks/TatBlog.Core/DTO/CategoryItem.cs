using System;
namespace TatBlog.Core.DTO;

public class CategoryItem
{
    public string Name { get; set; }

    public string Description { get; set; }

    public bool ShowOnMenu { get; set; }

    public int Id { get; set; }

    public int PostCount { get; set; }

    public string UrlSlug { get; set; }
}
