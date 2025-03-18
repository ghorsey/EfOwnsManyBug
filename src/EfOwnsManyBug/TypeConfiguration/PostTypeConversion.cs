using EfOwnsManyBug.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Text.Json;

namespace EfOwnsManyBug.TypeConfiguration;

internal class PostTypeConversion : IEntityTypeConfiguration<Post>
{
    public void Configure(EntityTypeBuilder<Post> builder)
    {
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
    }
}
