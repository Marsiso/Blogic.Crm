﻿#nullable disable
namespace Blogic.Crm.Web.Views.Consultant;

public sealed class ConsultantCreateViewModel
{
	public ConsultantCreateViewModel()
	{
	}

	public ConsultantCreateViewModel(ConsultantInput consultant, ValidationException? validationException)
	{
		Consultant = consultant;
		ValidationException = validationException;
	}

	public ConsultantCreateViewModel(ConsultantInput consultant) : this()
	{
		Consultant = consultant;
	}

	public ConsultantInput Consultant { get; set; }
	public ValidationException ValidationException { get; set; }
}