namespace Blogic.Crm.Web.Views.Consultant;

public sealed class GetConsultantViewModel
{
	public GetConsultantViewModel(ConsultantRepresentation? consultant)
	{
		Consultant = consultant;
	}

	public ConsultantRepresentation? Consultant { get; set; }
}