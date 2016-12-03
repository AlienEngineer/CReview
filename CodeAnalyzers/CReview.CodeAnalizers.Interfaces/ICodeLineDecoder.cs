using CReview.Decoders;

namespace CReview.CodeAnalyzers
{
    public interface ICodeLineDecoder
    {
        CodeLine DecodeLine(string lineOfCode);
    }
}