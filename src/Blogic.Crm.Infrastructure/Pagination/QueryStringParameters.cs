namespace Blogic.Crm.Infrastructure.Pagination;

public record QueryStringParameters
{
	public QueryStringParameters()
	{
		PageSize = MinimumPageSize;
		PageNumber = MinimumPageNumber;
		SearchString = string.Empty;
	}
	
	public QueryStringParameters(int pageSize, int pageNumber, string searchString)
	{
		PageSize = pageSize;
		PageNumber = pageNumber;
		SearchString = searchString;
	}

	public const int MinimumPageSize = 10;
	public const int MaximumPageSize = 50;
	public const int MinimumPageNumber = 1;
	private readonly int _pageSize;
	private readonly int _pageNumber;

	public int PageNumber
	{
		get => _pageNumber;
		private init => _pageNumber = value > MinimumPageNumber
			? value
			: MinimumPageNumber;
	}

	public int PageSize
	{
		get => _pageSize;
		private init => _pageSize = value > MaximumPageSize
			? MaximumPageSize
			: value > MinimumPageSize
				? value
				: MinimumPageSize;
	}

	public string SearchString { get; init; }
}