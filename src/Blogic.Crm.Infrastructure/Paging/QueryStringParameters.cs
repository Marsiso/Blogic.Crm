namespace Blogic.Crm.Infrastructure.Paging;

public class QueryStringParameters
{
	public const int MinimumPageSize = 10;
	public const int MaximumPageSize = 50;
	public const int MinimumPageNumber = 1;
	private int _pageSize = MinimumPageSize;
	private int _pageNumber = MinimumPageNumber;

	public int PageNumber
	{
		get => _pageNumber;
		set => _pageNumber = value > MinimumPageNumber
			? value
			: MinimumPageNumber;
	}

	public int PageSize
	{
		get => _pageSize;
		set => _pageSize = value > MaximumPageSize
			? MaximumPageSize
			: value > MinimumPageSize
				? value
				: MinimumPageSize;
	}
}