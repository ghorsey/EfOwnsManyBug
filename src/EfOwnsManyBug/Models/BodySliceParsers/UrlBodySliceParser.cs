namespace EfOwnsManyBug.Models.BodySliceParsers;

public class UrlBodySliceParser
    : BaseBodySliceParser
{
    public UrlBodySliceParser() :
        base(
            pattern: "((http|https)://)(www.)?"
                   + "[a-zA-Z\\d@:%.\\-_+~#?&/=]{1,256}"
                   + "\\.[a-z]{2,6}\\b"
                   + "([-a-zA-Z\\d@:%._+~#?&/=]*)",
            sliceType: PostSliceType.Url)
    {
    }
}
