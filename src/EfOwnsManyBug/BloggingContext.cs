using EfOwnsManyBug.Models;
using Microsoft.EntityFrameworkCore;

namespace EfOwnsManyBug;

public class BloggingContext : DbContext
{
    public static readonly PostId PostId = new PostId(Guid.Parse("7EE68A49-843E-4274-86B3-B80CFFE8D407"));

    internal DbSet<Post> Posts { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder
            .UseSqlServer(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\geoff\code\ghorsey\EfOwnsManyBug\src\EfOwnsManyBug\Data\SampleDatabase.mdf;Integrated Security=True")
            .UseSeeding((ctx, _) =>
            {
                var post = ctx.Set<Post>().FirstOrDefault(p => p.Id == PostId);

                if (post == null)
                {
                    ctx.Set<Post>().Add(
                        new Post(PostId)
                        {
                            Body = "my test post",
                            Tags = new List<PostTag>
                            {
                                new PostTag
                                {
                                    PostId = PostId,
                                    Tag = "#tag-one"
                                },
                                new PostTag
                                {
                                    PostId =PostId,
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



