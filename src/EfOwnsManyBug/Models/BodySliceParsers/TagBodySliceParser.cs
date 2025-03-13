namespace EfOwnsManyBug.Models.BodySliceParsers;

public class TagBodySliceParser
    : BaseBodySliceParser
{
    public TagBodySliceParser() :
        base(
            pattern: "#(?!\\d+[,.\\s])\\S+\\b",
            sliceType: PostSliceType.Tag)
    {
    }
}
