using C3.Blocks.Repository.MsSql;
using EfOwnsManyBug.Models;
using Microsoft.Extensions.Logging;

namespace EfOwnsManyBug.Repository
{
    public class PostRepository(BloggingContext context, ILogger<PostRepository> logger) : RepositoryBase<Post, PostId, BloggingContext>(context, logger)
    {
    }
}
