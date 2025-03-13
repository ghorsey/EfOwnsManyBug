using System.ComponentModel.DataAnnotations;

namespace EfOwnsManyBug.Models;

public record class PostSlice(
    [Required]
    int Offset,
    [Required]
    int Length,
    [Required]
    string Text,
    [Required]
    PostSliceType SliceType
)
{
    public bool DoesSliceOverlap(PostSlice other)
    {
        ArgumentNullException.ThrowIfNull(other, nameof(other));

        return (Offset >= other.Offset && this.Offset <= other.Offset + other.Length) ||
                (Offset + Length >= other.Offset && Offset + Length <= other.Offset + other.Length);
    }
}

