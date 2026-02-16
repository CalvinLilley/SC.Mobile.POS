using SC.Mobile.POS.Core.Models;

namespace SC.Mobile.POS.Core.Abstractions;

public interface IDatabasePathProvider
{
    string GetDatabasePath();
}

public interface IBranchRepository
{
    Task<Branch?> GetAsync(string branchCode, CancellationToken cancellationToken = default);
}

public interface ICustomerRepository
{
    Task<Customer?> GetAsync(string code, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Customer>> SearchAsync(string search, int take, CancellationToken cancellationToken = default);
}

public interface IProductRepository
{
    Task<Product?> GetAsync(string code, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Product>> SearchAsync(string search, int take, CancellationToken cancellationToken = default);
}

public interface IPriceGroupRepository
{
    Task<PriceGroup?> GetAsync(string priceGroup, CancellationToken cancellationToken = default);
}

public interface IPriceRepository
{
    Task<decimal?> GetPriceAsync(string priceCode, string productCode, CancellationToken cancellationToken = default);
}

public interface IPricingEngine
{
    Task<PriceResult> PriceAsync(Branch branch, CustomerType customerType, string productCode, CancellationToken cancellationToken = default);
}

public interface ITaxEngine
{
    TaxResult Calculate(Branch branch, TaxStatus customerTaxStatus, TaxStatus productTaxStatus, decimal lineSubtotal);
}
