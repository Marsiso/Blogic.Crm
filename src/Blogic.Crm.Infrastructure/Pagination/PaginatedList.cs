namespace Blogic.Crm.Infrastructure.Pagination;

/// <summary>
/// Database entity data set 'fragment' (page) used by the pagination.
/// </summary>
/// <typeparam name="T"></typeparam>
public sealed class PaginatedList<T> : List<T> where T : class
{
	public PaginatedList(List<T> paginatedItems, int totalItems, int pageNumber, int pageSize)
	{
		TotalItems = totalItems;
		PageSize = pageSize;
		CurrentPage = pageNumber;
		TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
		AddRange(paginatedItems);
	}

	/// <summary>
	/// Retrieved data set 'fragment' index.
	/// </summary>
	public int CurrentPage { get; set; }
	
	/// <summary>
	/// Total number of data set 'fragments'.
	/// </summary>
	public int TotalPages { get; set; }
	
	/// <summary>
	/// Total number entities contained within data set 'fragment'.
	/// </summary>
	public int PageSize { get; set; }
	
	/// <summary>
	/// Total number of items contained in the data set.
	/// </summary>
	public int TotalItems { get; set; }
	
	/// <summary>
	/// True when the data set 'fragment' has predecessor.
	/// </summary>
	public bool HasPrevious => CurrentPage > 1;
	
	/// <summary>
	/// True when the data set 'fragment' has ancestor.
	/// </summary>
	
	public bool HasNext => CurrentPage < TotalPages;
}