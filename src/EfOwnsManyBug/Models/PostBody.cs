using EfOwnsManyBug.Models.BodySliceParsers;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace EfOwnsManyBug.Models;

public record class PostBody
{
    private static readonly IList<BaseBodySliceParser> BodySliceParsers = [
        new UrlBodySliceParser(),
        new TagBodySliceParser(),
    ];

    [Required]
    [MaxLength(8000)]
    public string Value { get; }

    [Required]
    public IReadOnlyList<PostSlice> Slices { get; }

    public PostBody(string value, IList<PostSlice> slices)
    {
        this.Value = value;
        this.Slices = new ReadOnlyCollection<PostSlice>(slices);
    }
    public PostBody()
        : this(string.Empty, new List<PostSlice>())
    {
    }

    public PostBody(string value)
    {
        this.Value = value;
        this.Slices = BodySliceParsers
            .SelectMany(parser => parser.ParseSlices(this.Value))
            .Aggregate(new List<PostSlice>(), (acc, slice) =>
            {
                if (!acc.Any(s => s.DoesSliceOverlap(slice)))
                {
                    acc.Add(slice);
                }
                return acc;
            })
            .OrderBy(s => s.Offset)
            .ToList();
    }

    public override string ToString() => this.Value ?? string.Empty;

    public static PostBody FromString(string value) => new PostBody(value ?? string.Empty);

    public static implicit operator string(PostBody body) => body?.ToString() ?? string.Empty;

    public static implicit operator PostBody(string s) => FromString(s);
}

