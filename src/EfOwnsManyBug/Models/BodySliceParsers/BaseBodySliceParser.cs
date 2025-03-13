using System.Text.RegularExpressions;

namespace EfOwnsManyBug.Models.BodySliceParsers;

public abstract class BaseBodySliceParser
{
    private readonly Regex regEx;
    private readonly PostSliceType sliceType;

    protected BaseBodySliceParser(string pattern, PostSliceType sliceType)
    {
        this.regEx = new(pattern, RegexOptions.IgnoreCase);
        this.sliceType = sliceType;
    }

    public virtual IList<PostSlice> ParseSlices(string body)
    {
        var matches = this.regEx.Matches(body);

        return matches.Select(match => new PostSlice
        (
            Offset: match.Index,
            Length: match.Length,
            SliceType: this.sliceType,
            Text: match.Value
        )).ToList();
    }
}
