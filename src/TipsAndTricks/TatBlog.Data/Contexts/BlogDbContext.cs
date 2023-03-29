﻿using Microsoft.EntityFrameworkCore;
using TatBlog.Core.Entities;
using TatBlog.Data.Mappings;

namespace TatBlog.Data.Contexts;

public class BlogDbContext : DbContext
{
    public DbSet<Author> Authors { get; set; }

    public DbSet<Subscriber> Subscribers { get; set; }

    public DbSet<Category> Categories { get; set; }

    public DbSet<Post> Posts { get; set; }

    public DbSet<Tag> Tags { get; set; }

    /*protected override void OnConfiguring(
        DbContextOptionsBuilder optionsBuilder)
    {
<<<<<<< Updated upstream
        optionsBuilder.UseSqlServer(@"Server=DESKTOP-GJ77F65;Database=TatBlog;
=======
        public DbSet<Author> Authors { get; set; }

        public DbSet<Subscriber> Subscribers { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Post> Posts { get; set; }

        public DbSet<Tag> Tags { get; set; }*/

    public BlogDbContext(DbContextOptions<BlogDbContext> options)
            : base(options)
        {

        }

        public BlogDbContext()
        {
        }

        /*protected override void OnConfiguring(
            DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(@"Server=PC336;Database=TatBlog;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True;");
    }*/
    /*>>>>>>> Stashed changes
                Trusted_Connection=True;MultipleActiveResultSets=true");
        }*/

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(CategoryMap).Assembly);
    }

    /*public static implicit operator BlogDbContext(TatBlog.Services.Blogs.BlogRepository v) => throw new NotImplementedException();*/

}
