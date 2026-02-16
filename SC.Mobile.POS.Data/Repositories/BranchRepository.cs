using Dapper;
using SC.Mobile.POS.Core.Abstractions;
using SC.Mobile.POS.Core.Models;
using SC.Mobile.POS.Data.Infrastructure;

namespace SC.Mobile.POS.Data.Repositories;

public sealed class BranchRepository(SqliteConnectionFactory connectionFactory) : IBranchRepository
{
    public async Task<Branch?> GetAsync(string branchCode, CancellationToken cancellationToken = default)
    {
        using var connection = connectionFactory.CreateConnection();
        const string sql = """
                           SELECT BranchCode, RetailPriceCode, WholesalePriceCode, PromoPriceCode,
                                  TaxCode_Taxable, TaxCode_NonTaxable, TaxRate, CashCustomerCode, DateChangedUtc
                           FROM Branch
                           WHERE BranchCode = @BranchCode
                           """;

        return await connection.QuerySingleOrDefaultAsync<Branch>(new CommandDefinition(sql, new { BranchCode = branchCode }, cancellationToken: cancellationToken));
    }
}
