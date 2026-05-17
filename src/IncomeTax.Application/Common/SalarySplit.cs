using System.Globalization;

namespace IncomeTax.Application.Common;

public static class SalarySplit
{
    public static (double? Amount, string? Period) Split(string? salary)
    {
        if (salary is null) return (null, null);
        
        ReadOnlySpan<char> span = salary.AsSpan();
        return (
            double.Parse(span[..span.IndexOf(' ')], NumberStyles.Currency, CultureInfo.CreateSpecificCulture("en-GB")),
            span[(span.IndexOf(' ') + 1)..].ToString());
    }
}