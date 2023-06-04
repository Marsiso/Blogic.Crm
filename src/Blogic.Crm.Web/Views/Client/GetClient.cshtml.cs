using Blogic.Crm.Domain.Data.Dtos;

namespace Blogic.Crm.Web.Views.Client;

public sealed class GetClientViewModel
{
	public GetClientViewModel(ClientRepresentation? client)
	{
		Client = client;
	}
	
	public ClientRepresentation? Client { get; set; }
}