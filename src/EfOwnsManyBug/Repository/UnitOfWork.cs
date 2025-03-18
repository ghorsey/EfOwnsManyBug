
using C3.Blocks.Repository.MsSql;
using Microsoft.Extensions.Logging;

namespace EfOwnsManyBug.Repository;

public class UnitOfWork(BloggingContext context, ILoggerFactory factory) : UnitOfWork<BloggingContext>(context, factory)
{
    public PostRepository PostRepository { get; } = new PostRepository(context, factory.CreateLogger<PostRepository>());

    public PostSummaryRepository PostSummaryRepository { get; } = new PostSummaryRepository(context, factory.CreateLogger<PostSummaryRepository>());
}
