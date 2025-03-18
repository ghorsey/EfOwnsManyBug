using C3.Blocks.Domain;

namespace EfOwnsManyBug.Models;

public class PostSummary(PostId id) : EntityBase<PostId>(id)
{
    public PostSummary()
        : this(new PostId())
    {
    }

    public PostBody Body { get; set; } = string.Empty;

    public IReadOnlyList<PostSummaryTag> Tags
    {
        get => this.Body.Slices.Select(
            s => new PostSummaryTag
            {
                PostId = this.Id,
                Tag = s.Text
            }).Distinct().ToList();
        set => _ = value;
    }
}
