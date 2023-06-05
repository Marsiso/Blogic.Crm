namespace Blogic.Crm.Web.Views.Consultant;

public sealed class GetConsultantsViewModel
{
	public GetConsultantsViewModel(PaginatedList<ConsultantRepresentation> consultants,
	                               ConsultantQueryString queryString)
	{
		Consultants = consultants;
		QueryString = queryString;
	}

	public PaginatedList<ConsultantRepresentation> Consultants { get; set; }

	public ConsultantQueryString QueryString { get; set; }
}