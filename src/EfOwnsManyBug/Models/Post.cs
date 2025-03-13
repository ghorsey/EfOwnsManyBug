using C3.Blocks.Domain;

namespace EfOwnsManyBug.Models;

public class Post(PostId id) : EntityBase<PostId>(id)
{
    public Post()
        : this(new PostId())
    {
    }

    public PostBody Body { get; set; } = string.Empty;

    public IReadOnlyList<PostTag> Tags
    {
        get => this.Body.Slices.Select(
            s => new PostTag
            {
                PostId = this.Id,
                Tag = s.Text
            }).Distinct().ToList();
        set => _ = value;
    }
}
