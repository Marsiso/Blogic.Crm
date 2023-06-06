namespace Blogic.Crm.Web.Views.Consultant;

public sealed class ConsultantUpdateViewModel
{
	public ConsultantUpdateViewModel(long id, ConsultantInput consultant)
	{
		Id = id;
		Consultant = consultant;
	}

	public ConsultantUpdateViewModel(long id, ConsultantInput consultant, ValidationException? validationException)
	{
		Id = id;
		Consultant = consultant;
		ValidationException = validationException;
	}

	public long Id { get; set; }
	public ConsultantInput Consultant { get; set; }
	public ValidationException? ValidationException { get; set; }
}