using System.Globalization;
using Blogic.Crm.Infrastructure.Sorting;
using Microsoft.AspNetCore.Mvc;

namespace Blogic.Crm.Infrastructure.Pagination;

/// <summary>
///     Query string parameters used by the searching, filtering sorting related to the <see cref="Consultant" /> data set.
/// </summary>
[BindProperties(SupportsGet = true)]
public sealed class ConsultantQueryString : QueryStringBase
{
	public const ConsultantSortOrder DefaultSortOrder = ConsultantSortOrder.FamilyName;
	private long _maxId = long.MaxValue;
	private long _minId = 1;

	/// <summary>
	///     Used to sort the <see cref="Client" /> data set.
	/// </summary>
	public ConsultantSortOrder SortOrder { get; set; } = DefaultSortOrder;

	/// <summary>
	///     Used for the <see cref="Client" /> data set filtering.
	/// </summary>
	public DateTime MinDateBorn { get; set; } = DateTime.MinValue;

	/// <summary>
	///     Used for the <see cref="Client" /> data set filtering.
	/// </summary>
	public DateTime MaxDateBorn { get; set; } = DateTime.MaxValue;

	/// <summary>
	///     Validation for the minimal and maximal date born.
	/// </summary>
	public bool IsValidDateBorn => MaxDateBorn >= MinDateBorn;

	public string Email { get; set; } = string.Empty;
	public bool IsEmailConfirmed { get; set; } = true;
	
	public string Phone { get; set; } = string.Empty;
	public string FamilyName { get; set; } = string.Empty;
	public string GivenName { get; set; } = string.Empty;
	public bool IsPhoneConfirmed { get; set; } = true;
	
	public string BirthNumber { get; set; } = string.Empty;

	public long MinId
	{
		get => _minId;
		set
		{
			if (value > 1)
			{
				_minId = value;
				return;
			}

			_minId = 1;
		}
	}

	public long MaxId
	{
		get => _maxId;
		set
		{
			if (value > 1)
			{
				_maxId = value;
				return;
			}

			_maxId = 1;
		}
	}

	public override Dictionary<string, string?> ToDictionary()
	{
		return new Dictionary<string, string?>(base.ToDictionary())
		{
			{ nameof(SortOrder), SortOrder.ToString() },
			{ nameof(MinDateBorn), MinDateBorn.ToString(CultureInfo.InvariantCulture) },
			{ nameof(MaxDateBorn), MaxDateBorn.ToString(CultureInfo.InvariantCulture) },
			{ nameof(Email), Email },
			{ nameof(Phone), Phone },
			{ nameof(BirthNumber), BirthNumber },
			{ nameof(IsEmailConfirmed), IsEmailConfirmed.ToString() },
			{ nameof(IsPhoneConfirmed), IsPhoneConfirmed.ToString() },
			{ nameof(MinId), MinId.ToString() },
			{ nameof(MaxId), MaxId.ToString() },
		};
	}
}