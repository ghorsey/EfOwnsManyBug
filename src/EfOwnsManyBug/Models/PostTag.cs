namespace EfOwnsManyBug.Models;

public class PostTag
{
    public PostId PostId { get; set; } = new PostId(Guid.NewGuid());
    public string Tag { get; set; } = string.Empty;
}
