using Dapper;
using SC.Mobile.POS.Core.Abstractions;
using SC.Mobile.POS.Core.Models;
using SC.Mobile.POS.Data.Infrastructure;
using SC.Mobile.POS.Data.Mappers;

namespace SC.Mobile.POS.Data.Repositories;

public sealed class PriceGroupRepository(SqliteConnectionFactory connectionFactory) : IPriceGroupRepository
{
    public async Task<PriceGroup?> GetAsync(string priceGroup, CancellationToken cancellationToken = default)
    {
        using var connection = connectionFactory.CreateConnection();
        const string sql = """
                           SELECT PriceGroup, CustomerType, DateChangedUtc, IsDeleted
                           FROM PriceGroup
                           WHERE PriceGroup = @PriceGroup AND IsDeleted = 0
                           """;

        var row = await connection.QuerySingleOrDefaultAsync<PriceGroupRow>(new CommandDefinition(sql, new { PriceGroup = priceGroup }, cancellationToken: cancellationToken));
        return row?.ToDomain();
    }

    private sealed class PriceGroupRow
    {
        public string PriceGroup { get; set; } = string.Empty;
        public string CustomerType { get; set; } = string.Empty;
        public string DateChangedUtc { get; set; } = string.Empty;
        public int IsDeleted { get; set; }

        public PriceGroup ToDomain() => new()
        {
            PriceGroupCode = PriceGroup,
            CustomerType = TypeParsers.ParseCustomerType(CustomerType),
            DateChangedUtc = DateChangedUtc,
            IsDeleted = IsDeleted
        };
    }
}
