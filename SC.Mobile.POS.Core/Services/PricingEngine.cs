using SC.Mobile.POS.Core.Abstractions;
using SC.Mobile.POS.Core.Models;

namespace SC.Mobile.POS.Core.Services;

public sealed class PricingEngine(IPriceRepository priceRepository) : IPricingEngine
{
    public async Task<PriceResult> PriceAsync(Branch branch, CustomerType customerType, string productCode, CancellationToken cancellationToken = default)
    {
        var baseCode = customerType == CustomerType.Retail ? branch.RetailPriceCode : branch.WholesalePriceCode;
        var basePrice = await priceRepository.GetPriceAsync(baseCode, productCode, cancellationToken);
        if (!basePrice.HasValue)
        {
            throw new InvalidOperationException($"No base price found for product {productCode} in {baseCode}.");
        }

        decimal? promoPrice = null;
        if (!string.IsNullOrWhiteSpace(branch.PromoPriceCode))
        {
            promoPrice = await priceRepository.GetPriceAsync(branch.PromoPriceCode!, productCode, cancellationToken);
        }

        var result = new PriceResult
        {
            BasePrice = basePrice.Value,
            BasePriceCodeUsed = baseCode,
            PromoPrice = promoPrice,
            PromoPriceCodeUsed = promoPrice.HasValue ? branch.PromoPriceCode : null,
            FinalPrice = basePrice.Value,
            Source = PriceSource.Base
        };

        if (promoPrice.HasValue && promoPrice.Value < basePrice.Value)
        {
            result.FinalPrice = promoPrice.Value;
            result.Source = PriceSource.Promo;
        }

        return result;
    }
}
