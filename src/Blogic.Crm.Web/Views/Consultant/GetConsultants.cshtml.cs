#nullable disable

namespace Blogic.Crm.Web.Views.Consultant;

public sealed class GetConsultantsViewModel
{
	public PaginatedList<ConsultantRepresentation> Consultants { get; set; }

	public ConsultantQueryString QueryString { get; set; }
}