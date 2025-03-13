using C3.Blocks.Domain;

namespace EfOwnsManyBug.Models;

public class Post(PostId id) : EntityBase<PostId>(id)
{
    public Post()
        : this(new PostId())
    {
    }

    public string Body { get; set; } = string.Empty;

    public List<PostTag> Tags { get; init; } = new();
}
