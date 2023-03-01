﻿using System;

using TatBlog.Core.Contracts;

namespace TatBlog.Core.Entities;

public class Tag : IEntity
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string UrlSlug { get; set; }

    public string Description { get; set; }

    public IList<Post> Posts { get; set; }
}
