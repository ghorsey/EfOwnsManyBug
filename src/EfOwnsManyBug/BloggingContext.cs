using C3.Blocks.Repository.MsSql;
using EfOwnsManyBug.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace EfOwnsManyBug;

public class BloggingContext(ILoggerFactory loggerFactory) : DbContext
{
    private readonly ILoggerFactory loggerFactory = loggerFactory;
    public static readonly PostId PostId = new PostId(Guid.Parse("7EE68A49-843E-4274-86B3-B80CFFE8D407"));

    internal DbSet<Post> Posts { get; set; }

    public BloggingContext()
        : this(LoggerFactory.Create(builder => builder.AddConsole().SetMinimumLevel(LogLevel.Trace)))
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder
            .UseLoggerFactory(this.loggerFactory)
            .EnableSensitiveDataLogging()
            .UseSqlServer(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\geoff\code\ghorsey\EfOwnsManyBug\src\EfOwnsManyBug\Data\SampleDatabase.mdf;Integrated Security=True")
            .UseSeeding((ctx, _) =>
            {
                var post = ctx.Set<Post>().FirstOrDefault(p => p.Id == PostId);

                if (post == null)
                {
                    ctx.Set<Post>().Add(
                        new Post(PostId)
                        {
                            Body = "my test #post"
                        }
                    );
                    ctx.SaveChanges();
                }
            });

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Ignore<PostSlice>();
        modelBuilder.FixDateTimeOffsetForSqlite(this.Database.ProviderName);

        var builder = modelBuilder.Entity<Post>();

        builder.HasKey(d => d.Id);
        builder.Property(d => d.Id)
            .IsRequired()
            .HasConversion(v => v.Value, v => new PostId(v))
            .ValueGeneratedNever();

        var serializerOptions = new JsonSerializerOptions();
        builder.OwnsOne(
            d => d.Body,
            body =>
            {
                body.Property(p => p.Value)
                    .IsRequired()
                    .HasDefaultValue(string.Empty)
                    .HasMaxLength(8000);

                body.Property(p => p.Slices)
                    .IsRequired()
                    .HasMaxLength(8000)
                    .HasConversion(
                        v => JsonSerializer.Serialize(v, serializerOptions),
                        v => JsonSerializer.Deserialize<List<PostSlice>>(v, serializerOptions) ?? new List<PostSlice>(),
                        new ValueComparer<IReadOnlyList<PostSlice>>(
                            (c1, c2) => (c1 ?? Enumerable.Empty<PostSlice>()).SequenceEqual(c2 ?? Enumerable.Empty<PostSlice>()),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToList()
                        )
                    );
            }
        );
        builder.OwnsMany(
            d => d.Tags,
            tags =>
            {
                tags.WithOwner()
                    .HasForeignKey(tag => tag.PostId);

                tags.Property<Guid>("Id");
                tags.HasKey("Id");

                tags.Property(tag => tag.PostId)
                    .HasConversion(v => v.Value, v => new PostId(v));

                tags.Property(tag => tag.Tag)
                    .IsRequired()
                    .HasDefaultValue(string.Empty)
                    .HasMaxLength(200);
                tags.HasIndex(tag => new { tag.PostId, tag.Tag });
            }
        );
            
    }
}



