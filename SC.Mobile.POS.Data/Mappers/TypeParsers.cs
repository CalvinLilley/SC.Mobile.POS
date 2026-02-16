using SC.Mobile.POS.Core.Models;

namespace SC.Mobile.POS.Data.Mappers;

internal static class TypeParsers
{
    public static CustomerType ParseCustomerType(string value)
        => Enum.TryParse<CustomerType>(value, true, out var parsed)
            ? parsed
            : throw new InvalidOperationException($"Invalid customer type: {value}");

    public static TaxStatus ParseTaxStatus(string value)
        => Enum.TryParse<TaxStatus>(value, true, out var parsed)
            ? parsed
            : throw new InvalidOperationException($"Invalid tax status: {value}");
}
