namespace Blogic.Crm.Infrastructure.Logging;

public sealed class LoggerOptions
{
	public ConsoleSinkOptions Console { get; set; } = default!;

	public SeqSinkOptions Seq { get; set; } = default!;
}

public sealed class ConsoleSinkOptions
{
	public string OutputTemplate { get; set; } = default!;
}

public sealed class SeqSinkOptions
{
	public string ServerUrl { get; set; } = default!;
}