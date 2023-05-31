namespace Blogic.Crm.Infrastructure.Logging;

public class LoggerOptions
{
    public ConsoleSinkOptions Console { get; set; } = default!;

    public SeqSinkOptions Seq { get; set; } = default!;
}

public class ConsoleSinkOptions
{
    public string OutputTemplate { get; set; } = default!;
}

public class SeqSinkOptions
{
    public string ServerUrl { get; set; } = default!;
}