namespace Blogic.Crm.Infrastructure.Pagination;

/// <summary>
///     General query string parameters used by the searching and pagination for the database entity data sets.
/// </summary>
public class QueryStringBase
{
	/// <summary>
	///     <see cref="PageSize" /> lower bound.
	/// </summary>
	public const int MinimumPageSize = 10;

	/// <summary>
	///     <see cref="PageSize" /> upper bound.
	/// </summary>
	public const int MaximumPageSize = 50;

	/// <summary>
	///     <see cref="PageNumber" /> lower bound.
	/// </summary>
	public const int MinimumPageNumber = 1;

	private int _pageNumber;

	private int _pageSize;

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

	/// <summary>
	///     Used by the pagination for the index of the retrieved data set 'fragment' (page).
	/// </summary>
	public int PageNumber
	{
		get => _pageNumber;
		set => _pageNumber = value > MinimumPageNumber
			? value
			: MinimumPageNumber;
	}

	/// <summary>
	///     Used by the pagination for the size of the retrieved data set 'fragment' (page).
	/// </summary>
	public int PageSize
	{
		get => _pageSize;
		set => _pageSize = value > MaximumPageSize
			? MaximumPageSize
			: value > MinimumPageSize
				? value
				: MinimumPageSize;
	}

	/// <summary>
	///     Used for the searching through database entity data set.
	///     Consists of search terms separated by the delimiter.
	///     Example search string 'user@example.com Petr 19323455843'.
	/// </summary>
	public string SearchString { get; set; }
}