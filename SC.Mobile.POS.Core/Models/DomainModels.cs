namespace SC.Mobile.POS.Core.Models;

public enum CustomerType
{
    Retail,
    Wholesale
}

public enum TaxStatus
{
    Taxable,
    NonTaxable
}

public enum PriceSource
{
    Base,
    Promo
}

public sealed class Branch
{
    public string BranchCode { get; set; } = string.Empty;
    public string RetailPriceCode { get; set; } = string.Empty;
    public string WholesalePriceCode { get; set; } = string.Empty;
    public string? PromoPriceCode { get; set; }
    public string TaxCode_Taxable { get; set; } = string.Empty;
    public string TaxCode_NonTaxable { get; set; } = string.Empty;
    public decimal TaxRate { get; set; }
    public string CashCustomerCode { get; set; } = string.Empty;
    public string DateChangedUtc { get; set; } = string.Empty;
}

public sealed class PriceGroup
{
    public string PriceGroupCode { get; set; } = string.Empty;
    public CustomerType CustomerType { get; set; }
    public string DateChangedUtc { get; set; } = string.Empty;
    public int IsDeleted { get; set; }
}

public sealed class Customer
{
    public string CustomerCode { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string PriceGroup { get; set; } = string.Empty;
    public TaxStatus TaxStatus { get; set; }
    public string DateChangedUtc { get; set; } = string.Empty;
    public int IsDeleted { get; set; }
}

public sealed class Product
{
    public string ProductCode { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public TaxStatus TaxStatus { get; set; }
    public string DateChangedUtc { get; set; } = string.Empty;
    public int IsDeleted { get; set; }
}

public sealed class PriceResult
{
    public decimal BasePrice { get; set; }
    public string BasePriceCodeUsed { get; set; } = string.Empty;
    public decimal? PromoPrice { get; set; }
    public string? PromoPriceCodeUsed { get; set; }
    public decimal FinalPrice { get; set; }
    public PriceSource Source { get; set; }
}

public sealed class TaxResult
{
    public decimal TaxRateUsed { get; set; }
    public string TaxCodeUsed { get; set; } = string.Empty;
    public decimal TaxAmount { get; set; }
}
