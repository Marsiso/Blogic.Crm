#nullable disable

namespace Blogic.Crm.Web.Views.Client;

public sealed class GetClientsViewModel
{

	public PaginatedList<ClientRepresentation> Clients { get; set; }

	public ClientQueryString QueryString { get; set; }
}