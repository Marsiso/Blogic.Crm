#nullable disable

using Blogic.Crm.Infrastructure.Sorting;

namespace Blogic.Crm.Web.Views.Client;

public sealed class GetClientsViewModel
{
	private PaginatedList<ClientRepresentation> _clients;
	private ClientQueryString _queryString;

	public GetClientsViewModel()
	{
	}

	public GetClientsViewModel(PaginatedList<ClientRepresentation> clients,
	                           ClientQueryString queryString)
	{
		Clients = clients;
		QueryString = queryString;
	}

	public PaginatedList<ClientRepresentation> Clients
	{
		get => _clients;
		set
		{
			if (value is { Count: > 0 })
			{
				_clients = value;
				return;
			}

			_clients = new PaginatedList<ClientRepresentation>(new List<ClientRepresentation>(), 0,
			                                                   MinimumPageNumber, MinimumPageSize);
		}
	}

	public ClientQueryString QueryString
	{
		get => _queryString;
		set
		{
			if (value is not null)
			{
				_queryString = value;
				return;
			}

			_queryString = new ClientQueryString(MinimumPageSize, MinimumPageNumber, string.Empty,
			                                     ClientsSortOrder.Id, DateTime.MinValue,
			                                     DateTime.MaxValue);
		}
	}
}