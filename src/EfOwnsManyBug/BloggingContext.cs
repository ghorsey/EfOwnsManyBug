using Microsoft.EntityFrameworkCore;

namespace EfOwnsManyBug;

public class BloggingContext : DbContext
{
    DbSet<Post> Posts { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder
            .UseSqlServer($"Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename={Directory.GetCurrentDirectory()}\\Data\\SampleDatabase.mdf;Integrated Security=True")
            .UseSeeding((ctx, _) =>
            {
                var postId = new PostId(Guid.Parse("7EE68A49-843E-4274-86B3-B80CFFE8D407"));
                var post = ctx.Set<Post>().FirstOrDefault(p => p.Id == postId);

                if (post == null)
                {
                    ctx.Set<Post>().Add(
                        new Post
                        {
                            Id = postId,
                            Body = "my test post",
                            Tags = new List<PostTag>
                            {
                                new PostTag
                                {
                                    PostId =postId,
                                    Tag = "#tag-one"
                                },
                                new PostTag
                                {
                                    PostId =postId,
                                    Tag = "#tag-two"
                                },
                            }
                        }
                    );
                    ctx.SaveChanges();
                }
            });

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        var postBuilder = modelBuilder.Entity<Post>();

        postBuilder.HasKey(d => d.Id);
        postBuilder.Property(d => d.Id)
            .IsRequired()
            .HasConversion(v => v.Value, v => new PostId(v))
            .ValueGeneratedNever();

        postBuilder.Property(d => d.Body)
            .IsRequired()
            .HasDefaultValue(string.Empty)
            .HasMaxLength(1000);

        postBuilder.OwnsMany(
            d => d.Tags,
            tags =>
            {
                tags.WithOwner()
                    .HasForeignKey(tag => tag.PostId);
                tags.Property<Guid>("Id");
                tags.HasKey("Id");

                tags.Property(tag => tag.Tag)
                    .IsRequired()
                    .HasDefaultValue(string.Empty)
                    .HasMaxLength(200);
            }
        );
            
    }
}

public record struct PostId(Guid Value);

public class Post
{
    public PostId Id { get; set; } = new PostId(Guid.NewGuid());

    public string Body { get; set; } = string.Empty;

    public List<PostTag> Tags { get; init; } = new();
}

public class PostTag
{
    public PostId PostId { get; set; } = new PostId(Guid.NewGuid());
    public string Tag { get; set; } = string.Empty;
}

