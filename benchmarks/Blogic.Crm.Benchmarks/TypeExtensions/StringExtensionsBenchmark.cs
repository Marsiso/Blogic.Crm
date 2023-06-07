using System.Text;
using BenchmarkDotNet.Attributes;
using static System.Int64;

namespace Blogic.Crm.Benchmarks.TypeExtensions;

[MemoryDiagnoser]
public class StringExtensionsBenchmark
{
    private const string Identifier = "   1239865243     ";

    public static string IdentifiersGenerator(int repeat)
    {
        return string.Join(',', Enumerable.Repeat(Identifier, repeat));
    }

    public IEnumerable<string> Identifiers()
    {
        yield return IdentifiersGenerator(1);
        yield return IdentifiersGenerator(100);
        yield return IdentifiersGenerator(10_000);
    }

    [Benchmark]
    [ArgumentsSource(nameof(Identifiers))]
    public List<long> ParseIdentifiersToInt64_Loop(string identifiersAsString)
    {
        var identifiers = new List<long>();
        var digits = new[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
        var others = new[] { ',', ' ' };
        var startIndex = identifiersAsString.IndexOfAny(digits);
        if (startIndex == -1) return identifiers;

        while (startIndex < identifiersAsString.Length)
        {
            var endIndex = identifiersAsString.IndexOfAny(others, startIndex);
            if (endIndex == -1)
            {
                if (TryParse(identifiersAsString[startIndex..], out var lastIdentifier))
                    identifiers.Add(lastIdentifier);
                return identifiers;
            }

            if (TryParse(identifiersAsString[startIndex..endIndex], out var identifier))
                identifiers.Add(identifier);

            startIndex = identifiersAsString.IndexOfAny(digits, endIndex);
            if (startIndex == -1) return identifiers;
        }

        return identifiers;
    }

    [Benchmark]
    [ArgumentsSource(nameof(Identifiers))]
    public List<long> ParseIdentifiersToInt64_Loop_Span(ReadOnlySpan<char> span)
    {
        ReadOnlySpan<char> digits = stackalloc[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
        ReadOnlySpan<char> others = stackalloc[] { ' ', ',' };

        var identifiers = new List<long>();

        var index = span.IndexOfAny(digits);
        if (index == -1) return identifiers;

        span = span.Slice(index);
        while (span.IsEmpty is false)
        {
            index = span.IndexOfAny(others);
            if (index == -1)
            {
                if (TryParse(span.Slice(0, index), out var lastIdentifier)) identifiers.Add(lastIdentifier);
                return identifiers;
            }

            if (TryParse(span.Slice(0, index), out var identifier)) identifiers.Add(identifier);
            span = span.Slice(index);

            index = span.IndexOfAny(digits);
            if (index == -1) return identifiers;

            span = span.Slice(index);
        }

        return identifiers;
    }

    [Benchmark]
    [ArgumentsSource(nameof(Identifiers))]
    public List<long> ParseIdentifiersToInt64_Loop_Span_Optimized(ReadOnlySpan<char> span)
    {
        var identifiers = new List<long>();
        while (span.Length > 0)
        {
            span.TrimStart();
            var entry = span.SplitNext(',').TrimEnd(' ');
            if (TryParse(entry, out var identifier)) identifiers.Add(identifier);
        }

        return identifiers;
    }

    [Benchmark]
    [ArgumentsSource(nameof(Identifiers))]
    public List<long> ParseIdentifiersToInt64_Loop_StringBuilder(string identifiersAsString)
    {
        var identifiers = new List<long>();
        StringBuilder stringBuilder = new();
        foreach (var c in identifiersAsString)
        {
            if (char.IsDigit(c))
            {
                stringBuilder.Append(c);
                continue;
            }

            if (stringBuilder.Length > 0 && TryParse(stringBuilder.ToString(), out var identifier))
                identifiers.Add(identifier);

            stringBuilder.Clear();
        }

        return identifiers;
    }

    [Benchmark]
    [ArgumentsSource(nameof(Identifiers))]
    public List<long> ParseIdentifiersToInt64_Loop_StringBuilder_Span(string identifiersAsString)
    {
        var identifiers = new List<long>();
        StringBuilder stringBuilder = new();
        foreach (var c in identifiersAsString.AsSpan())
        {
            if (char.IsDigit(c))
            {
                stringBuilder.Append(c);
                continue;
            }

            if (stringBuilder.Length > 0 && TryParse(stringBuilder.ToString(), out var identifier))
                identifiers.Add(identifier);

            stringBuilder.Clear();
        }


        return identifiers;
    }

    [Benchmark]
    [ArgumentsSource(nameof(Identifiers))]
    public long[] ParseIdentifiersToInt64_Linq(string identifiersAsString)
    {
        var valuesAsString = string.Concat(identifiersAsString.Where(c => !char.IsWhiteSpace(c)))
            .Split(',', StringSplitOptions.RemoveEmptyEntries);
        var identifiers = new long[valuesAsString.Length];
        for (var index = 0; index < valuesAsString.Length; index++)
        {
            var valueAsString = valuesAsString[index];
            identifiers[index] = Parse(valueAsString);
        }

        return identifiers;
    }
}

public static class Extensions
{
    public static ReadOnlySpan<char> SplitNext(this ref ReadOnlySpan<char> span, char seperator)
    {
        var pos = span.IndexOf(seperator);
        if (pos > -1)
        {
            var part = span.Slice(0, pos);
            span = span.Slice(pos + 1);
            return part;
        }
        else
        {
            var part = span;
            span = span.Slice(span.Length);
            return part;
        }
    }
}