using System.Globalization;
using Blogic.Crm.Infrastructure.Sorting;
using Microsoft.AspNetCore.Mvc;

namespace Blogic.Crm.Infrastructure.Pagination;

[BindProperties(SupportsGet = true)]
public sealed class ContractQueryString : QueryStringBase
{
	public const ContractSortOrder DefaultSortOrder = ContractSortOrder.Id;
	private long _maxClientId = long.MaxValue;
	private long _maxManagerId = long.MaxValue;
	private long _minClientId = 1;
	private long _minManagerId = 1;

	/// <summary>
	///     Used to sort the <see cref="Consultant" /> data set.
	/// </summary>
	public ContractSortOrder SortOrder { get; set; } = DefaultSortOrder;

	public string RegistrationNumber { get; set; } = string.Empty;
	public string Institution { get; set; } = string.Empty;
	public DateTime MinDateConcluded { get; set; } = DateTime.MinValue;
	public DateTime MaxDateConcluded { get; set; } = DateTime.MaxValue;
	public DateTime MinDateExpired { get; set; } = DateTime.MinValue;
	public DateTime MaxDateExpired { get; set; } = DateTime.MaxValue;
	public DateTime MinDateValid { get; set; } = DateTime.MinValue;
	public DateTime MaxDateValid { get; set; } = DateTime.MaxValue;

	public long MinClientId
	{
		get => _minClientId;
		set
		{
			if (value > 0)
			{
				_minClientId = value;
				return;
			}

			_minClientId = 1;
		}
	}

	public long MaxClientId
	{
		get => _maxClientId;
		set
		{
			if (value > 0)
			{
				_maxClientId = value;
				return;
			}

			_maxClientId = 1;
		}
	}

	public long MinManagerId
	{
		get => _minManagerId;
		set
		{
			if (value > 0)
			{
				_minManagerId = value;
				return;
			}

			_minManagerId = 1;
		}
	}

	public long MaxManagerId
	{
		get => _maxManagerId;
		set
		{
			if (value > 0)
			{
				_maxManagerId = value;
				return;
			}

			_maxManagerId = 1;
		}
	}
	
	public override Dictionary<string, string?> ToDictionary()
	{
		return new Dictionary<string, string?>(base.ToDictionary())
		{
			{ nameof(SortOrder), SortOrder.ToString() },
			{ nameof(RegistrationNumber), RegistrationNumber },
			{ nameof(Institution), Institution },
			{ nameof(MinDateConcluded), MinDateConcluded.ToString(CultureInfo.InvariantCulture) },
			{ nameof(MaxDateConcluded), MaxDateConcluded.ToString(CultureInfo.InvariantCulture) },
			{ nameof(MinDateExpired), MinDateExpired.ToString(CultureInfo.InvariantCulture) },
			{ nameof(MaxDateExpired), MaxDateExpired.ToString(CultureInfo.InvariantCulture) },
			{ nameof(MinDateValid), MinDateValid.ToString(CultureInfo.InvariantCulture) },
			{ nameof(MaxDateValid), MaxDateValid.ToString(CultureInfo.InvariantCulture) }
		};
	}
}