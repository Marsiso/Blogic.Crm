using Blogic.Crm.Domain.Data.Dtos;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Blogic.Crm.Web.Views.Client;

public sealed class ClientDeleteViewModel : PageModel
{
	public ClientDeleteViewModel(ClientRepresentation? client, string originAction)
	{
		Client = client;
		OriginAction = originAction;
	}
	
	public ClientRepresentation? Client { get; set; }
	public string OriginAction { get; set; }
}