using System.Globalization;
using Microsoft.AspNetCore.Mvc;

namespace Blogic.Crm.Infrastructure.Pagination;

/// <summary>
///     Parameters for filtering and sorting client records, searching for matching properties and routing.
/// </summary>
[BindProperties(SupportsGet = true)]
public sealed class ConsultantQueryString : QueryStringBase
{
    /// <summary>
    ///     Default consultant sort type by property.
    /// </summary>
    public const ConsultantSortOrder DefaultSortOrder = ConsultantSortOrder.FamilyName;

    private long _maxId = long.MaxValue;
    private long _minId = 1;

    /// <summary>
    ///     Consultant sort type by property.
    /// </summary>
    public ConsultantSortOrder SortOrder { get; set; } = DefaultSortOrder;

    /// <summary>
    ///     Parameter for sorting the consultant by minimum date of birth.
    /// </summary>
    public DateTime MinDateBorn { get; set; } = DateTime.MinValue;

    /// <summary>
    ///     Parameter for sorting the consultant by maximum date of birth.
    /// </summary>
    public DateTime MaxDateBorn { get; set; } = DateTime.MaxValue;

    /// <summary>
    ///     Validation for the minimal and maximal date born.
    /// </summary>
    public bool IsValidDateBorn => MaxDateBorn >= MinDateBorn;

    /// <summary>
    ///     Property to search consultant records.
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    ///     Parameter for filtering of the contracts by confirmed email address.
    /// </summary>
    public bool IsEmailConfirmed { get; set; } = true;

    /// <summary>
    ///     Property to search consultant records.
    /// </summary>
    public string Phone { get; set; } = string.Empty;

    /// <summary>
    ///     Property to search consultant records.
    /// </summary>
    public string GivenName { get; set; } = string.Empty;

    /// <summary>
    ///     Property to search consultant records.
    /// </summary>
    public string FamilyName { get; set; } = string.Empty;

    /// <summary>
    ///     Parameter for filtering of the contracts by confirmed telephone number.
    /// </summary>
    public bool IsPhoneConfirmed { get; set; } = true;

    /// <summary>
    ///     Property to search consultant records.
    /// </summary>
    public string BirthNumber { get; set; } = string.Empty;

    /// <summary>
    ///     Parameter for filtering of the contracts by minimum consultant identifier.
    /// </summary>
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

    /// <summary>
    ///     Parameter for filtering of the consultants by maximum consultant identifier.
    /// </summary>
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

    /// <summary>
    ///     Converts an instance to the parameter dictionary. Designed for routing using the Razor engine.
    /// </summary>
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
            { nameof(MaxId), MaxId.ToString() }
        };
    }
}