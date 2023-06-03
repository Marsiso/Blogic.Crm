using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Blogic.Crm.Web.Views.Client;

public sealed class ClientDetailViewModel : PageModel
{
	public ClientDetailViewModel(Domain.Data.Entities.Client? client)
	{
		Client = client;
	}
	
	public Domain.Data.Entities.Client? Client { get; set; }
}