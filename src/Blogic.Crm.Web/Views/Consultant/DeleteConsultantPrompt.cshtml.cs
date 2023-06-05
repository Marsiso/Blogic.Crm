namespace Blogic.Crm.Web.Views.Consultant;

public sealed class ConsultantDeleteViewModel
{
	public ConsultantDeleteViewModel(ConsultantRepresentation? consultant, string originAction)
	{
		Consultant = consultant;
		OriginAction = originAction;
	}

	public ConsultantRepresentation? Consultant { get; set; }
	public string OriginAction { get; set; }
}