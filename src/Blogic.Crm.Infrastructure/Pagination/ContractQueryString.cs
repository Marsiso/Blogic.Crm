using System.Globalization;
using Microsoft.AspNetCore.Mvc;

namespace Blogic.Crm.Infrastructure.Pagination;

/// <summary>
///     Parameters for filtering and sorting client records, searching for matching properties and routing.
/// </summary>
[BindProperties(SupportsGet = true)]
public sealed class ContractQueryString : QueryStringBase
{
    /// <summary>
    ///     Default contract sort type by property.
    /// </summary>
    public const ContractSortOrder DefaultSortOrder = ContractSortOrder.Id;

    private long _maxClientId = long.MaxValue;
    private long _maxManagerId = long.MaxValue;
    private long _minClientId = 1;
    private long _minManagerId = 1;

    /// <summary>
    ///     Contract sort type by property.
    /// </summary>
    public ContractSortOrder SortOrder { get; set; } = DefaultSortOrder;

    /// <summary>
    ///     Property to search contract records.
    /// </summary>
    public string RegistrationNumber { get; set; } = string.Empty;

    /// <summary>
    ///     Property to search contract records.
    /// </summary>
    public string Institution { get; set; } = string.Empty;

    /// <summary>
    ///     Parameter for filtering of of the contracts by minimum date of conclusion.
    /// </summary>
    public DateTime MinDateConcluded { get; set; } = DateTime.MinValue;

    /// <summary>
    ///     Parameter for filtering of the contracts by maximum date of conclusion.
    /// </summary>
    public DateTime MaxDateConcluded { get; set; } = DateTime.MaxValue;

    /// <summary>
    ///     Parameter for filtering of the contracts by minimum date of expiration.
    /// </summary>
    public DateTime MinDateExpired { get; set; } = DateTime.MinValue;

    /// <summary>
    ///     Parameter for filtering of the contracts by maximum date of expiration.
    /// </summary>
    public DateTime MaxDateExpired { get; set; } = DateTime.MaxValue;

    /// <summary>
    ///     Parameter for filtering of the contracts by minimum date of validity.
    /// </summary>
    public DateTime MinDateValid { get; set; } = DateTime.MinValue;

    /// <summary>
    ///     Parameter for filtering of the contracts by maximum date of validity.
    /// </summary>
    public DateTime MaxDateValid { get; set; } = DateTime.MaxValue;

    /// <summary>
    ///     Parameter for filtering of the contracts by minimum contract owner identifier.
    /// </summary>
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

    /// <summary>
    ///     Parameter for filtering of the contracts by maximum contract owner identifier.
    /// </summary>
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

    /// <summary>
    ///     Parameter for filtering of the contracts by minimum contract manager identifier.
    /// </summary>
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

    /// <summary>
    ///     Parameter for filtering of the contracts by maximum contract owner identifier.
    /// </summary>
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

    /// <summary>
    ///     Converts an instance to the parameter dictionary. Designed for routing using the Razor engine.
    /// </summary>
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