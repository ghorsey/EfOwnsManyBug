using EfOwnsManyBug;
using EfOwnsManyBug.Models;
using System.Text.Json;

var ctx = new BloggingContext();

var post = ctx.Set<PostSummary>().Find(BloggingContext.PostId);

if (post == null)
{
    Console.WriteLine("The post does not exist, run the command 'dotnet ef -p .\\src\\EfOwnsManyBug database update' to seed the database");
    return;
}

JsonSerializerOptions seralizerSettings = new()
{
    WriteIndented = true
};

Console.WriteLine("The found POST with the owned collection:");
Console.WriteLine(JsonSerializer.Serialize(post, seralizerSettings));

Console.WriteLine("");
Console.WriteLine("Attempting to delete the record");
ctx.Set<PostSummary>().Remove(post);
ctx.SaveChanges();
