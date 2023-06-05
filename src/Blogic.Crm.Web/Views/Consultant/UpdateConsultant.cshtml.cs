#nullable disable

namespace Blogic.Crm.Web.Views.Consultant;

public sealed class ConsultantUpdateViewModel
{
	public ConsultantUpdateViewModel() { }

	public ConsultantUpdateViewModel(long id, string originAction, ConsultantInput consultant)
	{
		Id = id;
		Consultant = consultant;
		OriginAction = originAction;
	}

	public ConsultantUpdateViewModel(long id, string originAction, ConsultantInput consultant,
	                                 ValidationException validationException)
	{
		Id = id;
		Consultant = consultant;
		ValidationException = validationException;
		OriginAction = originAction;
	}

	public long Id { get; set; }
	public string OriginAction { get; set; }
	public ConsultantInput Consultant { get; set; }
	public ValidationException ValidationException { get; set; }
}