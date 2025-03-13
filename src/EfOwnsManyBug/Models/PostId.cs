namespace EfOwnsManyBug.Models;

public record struct PostId(Guid Value)
{
    public PostId()
        : this(Guid.NewGuid())
    {
    }
}
