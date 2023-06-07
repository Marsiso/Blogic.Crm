using System.Globalization;
using Microsoft.AspNetCore.Mvc;

namespace Blogic.Crm.Infrastructure.Pagination;

/// <summary>
///     Parameters for filtering and sorting client records, searching for matching properties and routing.
/// </summary>
[BindProperties(SupportsGet = true)]
public sealed class ClientQueryString : QueryStringBase
{
    /// <summary>
    ///     Default client sort type by property.
    /// </summary>
    public const ClientsSortOrder DefaultSortOrder = ClientsSortOrder.Id;

    private long _maxId = long.MaxValue;
    private long _minId = 1;

    /// <summary>
    ///     Client sort type by property.
    /// </summary>
    public ClientsSortOrder SortOrder { get; set; } = DefaultSortOrder;

    /// <summary>
    ///     Parameter for sorting the client by minimum date of birth.
    /// </summary>
    public DateTime MinDateBorn { get; set; } = DateTime.MinValue;

    /// <summary>
    ///     Parameter for sorting the client by maximum date of birth.
    /// </summary>
    public DateTime MaxDateBorn { get; set; } = DateTime.MaxValue;

    /// <summary>
    ///     Validation for the minimal and maximal date born.
    /// </summary>
    public bool IsValidDateBorn => MaxDateBorn >= MinDateBorn;

    /// <summary>
    ///     Property to search client records.
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    ///     Parameter for filtering of the clients by confirmed email address.
    /// </summary>
    public bool IsEmailConfirmed { get; set; } = true;

    /// <summary>
    ///     Property to search client records.
    /// </summary>
    public string Phone { get; set; } = string.Empty;

    /// <summary>
    ///     Property to search client records.
    /// </summary>
    public string GivenName { get; set; } = string.Empty;

    /// <summary>
    ///     Property to search client records.
    /// </summary>
    public string FamilyName { get; set; } = string.Empty;

    /// <summary>
    ///     Parameter for filtering of the clients by confirmed telephone number.
    /// </summary>
    public bool IsPhoneConfirmed { get; set; } = true;

    /// <summary>
    ///     Property to search clients records.
    /// </summary>
    public string BirthNumber { get; set; } = string.Empty;

    /// <summary>
    ///     Parameter for filtering of the clients by minimum client identifier.
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
    ///     Parameter for filtering of the clients by maximum client identifier.
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