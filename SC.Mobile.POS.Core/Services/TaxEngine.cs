using SC.Mobile.POS.Core.Abstractions;
using SC.Mobile.POS.Core.Models;

namespace SC.Mobile.POS.Core.Services;

public sealed class TaxEngine : ITaxEngine
{
    public TaxResult Calculate(Branch branch, TaxStatus customerTaxStatus, TaxStatus productTaxStatus, decimal lineSubtotal)
    {
        if (customerTaxStatus == TaxStatus.Taxable && productTaxStatus == TaxStatus.Taxable)
        {
            var taxAmount = Math.Round(lineSubtotal * branch.TaxRate, 2, MidpointRounding.AwayFromZero);
            return new TaxResult
            {
                TaxRateUsed = branch.TaxRate,
                TaxCodeUsed = branch.TaxCode_Taxable,
                TaxAmount = taxAmount
            };
        }

        return new TaxResult
        {
            TaxRateUsed = 0,
            TaxCodeUsed = branch.TaxCode_NonTaxable,
            TaxAmount = 0
        };
    }
}
