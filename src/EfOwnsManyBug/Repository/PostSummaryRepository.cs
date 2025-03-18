using C3.Blocks.Repository.MsSql;
using EfOwnsManyBug.Models;
using Microsoft.Extensions.Logging;

namespace EfOwnsManyBug.Repository;

public class PostSummaryRepository(BloggingContext context, ILogger<PostSummaryRepository> logger)
    : RepositoryBase<PostSummary, PostId, BloggingContext>(context, logger)
{
}
