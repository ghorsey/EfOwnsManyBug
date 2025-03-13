using EfOwnsManyBug;
using EfOwnsManyBug.Repository;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Text.Json;

var ctx = new BloggingContext();
var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
var logger = loggerFactory.CreateLogger("Program");
var uow = new UnitOfWork(ctx, loggerFactory);

var ct = new CancellationToken();
await uow.ExecuteInResilientTransactionAsync(
    async () =>
    {
        var post = await uow.PostRepository.FindAsync(BloggingContext.PostId, ct);

        if (post == null)
        {
            logger.LogError("The post does not exist");
            return false;
        }

        var seralizerSettings = new JsonSerializerOptions
        {
            IncludeFields = true,
        };

        Console.WriteLine("The found POST with the owned collection:");
        Console.WriteLine(JsonSerializer.Serialize(post, seralizerSettings));

        Console.WriteLine("");
        Console.WriteLine("Attempting to delete the record");
        await uow.PostRepository.RemoveAsync(post);
        await uow.CommitAsync();
        return true;
    },
    IsolationLevel.ReadUncommitted,
    ct);
