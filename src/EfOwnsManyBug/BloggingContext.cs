using C3.Blocks.Repository.MsSql;
using EfOwnsManyBug.Models;
using EfOwnsManyBug.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace EfOwnsManyBug;

public class BloggingContext(ILoggerFactory loggerFactory, IConfiguration config) : DbContext
{
    private readonly ILoggerFactory loggerFactory = loggerFactory;
    public static readonly PostId PostId = new PostId(Guid.Parse("7EE68A49-843E-4274-86B3-B80CFFE8D407"));

    internal DbSet<Post> Posts { get; set; }

    public BloggingContext()
        : this(LoggerFactory.Create(builder => builder.AddConsole().SetMinimumLevel(LogLevel.Trace)),
              new ConfigurationBuilder().AddUserSecrets<BloggingContext>().Build())
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder
            .UseLoggerFactory(this.loggerFactory)
            .EnableSensitiveDataLogging()
            .UseSqlServer(config.GetValue<string>("ConnectionString"))
            //.UseSqlServer(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\geoff\code\ghorsey\EfOwnsManyBug\src\EfOwnsManyBug\Data\SampleDatabase.mdf;Integrated Security=True")
            .UseSeeding((ctx, _) =>
            {
                var post = ctx.Set<Post>().Find(PostId);
                if (post == null)
                {
                    Post newPost = new(PostId)
                    {
                        Body = "my #test #post",
                    };
                    ctx.Set<Post>().Add(newPost);

                    ctx.SaveChanges();

                    ctx.Set<PostSummary>().Add(new(newPost.Id) { Body = newPost.Body });

                    ctx.SaveChanges();
                }
            });

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(BloggingContext).Assembly);
        modelBuilder.Ignore<PostBody>();

        modelBuilder.FixDateTimeOffsetForSqlite(this.Database.ProviderName);
    }
}



