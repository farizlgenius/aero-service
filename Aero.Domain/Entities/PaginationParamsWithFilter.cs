using Aero.Domain.Helpers;
using System;

namespace Aero.Domain.Entities;

public sealed class PaginationParamsWithFilter
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string Search { get; set; } = string.Empty;
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }

    public PaginationParamsWithFilter() { }

    public PaginationParamsWithFilter(int pageNumber, int pageSize, string search, DateTime? startDate, DateTime? endDate)
    {
        PageNumber = pageNumber;
        PageSize = pageSize;
        Search = ValidateRequiredString(search, nameof(search));
        StartDate = startDate;
        EndDate = endDate;
    }

    private static string ValidateRequiredString(string value, string field)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value, field);
        var trimmed = value.Trim();
        if (!RegexHelper.IsValidName(trimmed) && !RegexHelper.IsValidOnlyCharAndDigit(trimmed.Replace("-", string.Empty).Replace("_", string.Empty)))
        {
            throw new ArgumentException($"{field} invalid.", field);
        }

        return value;
    }
}
