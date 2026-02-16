using Dapper;
using SC.Mobile.POS.Core.Abstractions;
using SC.Mobile.POS.Data.Infrastructure;

namespace SC.Mobile.POS.Data.Repositories;

public sealed class PriceRepository(SqliteConnectionFactory connectionFactory) : IPriceRepository
{
    public async Task<decimal?> GetPriceAsync(string priceCode, string productCode, CancellationToken cancellationToken = default)
    {
        using var connection = connectionFactory.CreateConnection();
        const string sql = """
                           SELECT Price
                           FROM PriceList
                           WHERE PriceCode = @PriceCode AND ProductCode = @ProductCode AND IsDeleted = 0
                           """;

        return await connection.QuerySingleOrDefaultAsync<decimal?>(new CommandDefinition(sql, new { PriceCode = priceCode, ProductCode = productCode }, cancellationToken: cancellationToken));
    }
}
