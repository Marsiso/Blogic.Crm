namespace Blogic.Crm.Infrastructure.Paging;

public sealed class PaginatedList<T> : List<T> where T : class
{
	public PaginatedList(List<T> paginatedItems, int totalItems, int pageNumber, int pageSize)
	{
		TotalTotalItems = totalItems;
		PageSize = pageSize;
		CurrentPage = pageNumber;
		TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
		AddRange(paginatedItems);
	}

	public int CurrentPage { get; set; }
	public int TotalPages { get; set; }
	public int PageSize { get; set; }
	public int TotalTotalItems { get; set; }
	
	public bool HasPrevious => CurrentPage > 1;
	
	public bool HasNext => CurrentPage < TotalPages;
}