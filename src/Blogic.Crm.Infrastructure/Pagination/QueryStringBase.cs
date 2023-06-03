namespace Blogic.Crm.Infrastructure.Pagination;

public class QueryStringBase
{
	public QueryStringBase()
	{
		PageSize = MinimumPageSize;
		PageNumber = MinimumPageNumber;
		SearchString = string.Empty;
	}
	
	public QueryStringBase(int pageSize, int pageNumber, string searchString)
	{
		PageSize = pageSize;
		PageNumber = pageNumber;
		SearchString = searchString;
	}

	public const int MinimumPageSize = 10;
	public const int MaximumPageSize = 50;
	public const int MinimumPageNumber = 1;
	private int _pageSize;
	private int _pageNumber;

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

	public string SearchString { get; set; }
}