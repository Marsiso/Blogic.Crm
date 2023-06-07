namespace Blogic.Crm.Infrastructure.TypeExtensions;

public static class SpanExtensions
{
    public static ReadOnlySpan<char> SplitNext(this ref ReadOnlySpan<char> span, char delimiter) {
        int position = span.IndexOf(delimiter);
        if (position > -1) {
            var part = span.Slice(0, position);
            span = span.Slice(position + 1);
            return part;
        } else {
            var part = span;
            span = span.Slice(span.Length);
            return part;
        }
    }
    
    public static List<long> ParseNumbersToInt64(ReadOnlySpan<char> span)
    {
        var identifiers = new List<long>();
        while (span.Length > 0)
        {
            span.TrimStart();
            var entry = span.SplitNext(',').TrimEnd(' ');
            if (long.TryParse(entry, out var identifier)) identifiers.Add(identifier);
        }

        return identifiers;
    }
}