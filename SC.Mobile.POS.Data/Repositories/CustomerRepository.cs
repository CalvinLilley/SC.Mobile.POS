using Dapper;
using SC.Mobile.POS.Core.Abstractions;
using SC.Mobile.POS.Core.Models;
using SC.Mobile.POS.Data.Infrastructure;
using SC.Mobile.POS.Data.Mappers;

namespace SC.Mobile.POS.Data.Repositories;

public sealed class CustomerRepository(SqliteConnectionFactory connectionFactory) : ICustomerRepository
{
    public async Task<Customer?> GetAsync(string code, CancellationToken cancellationToken = default)
    {
        using var connection = connectionFactory.CreateConnection();
        const string sql = """
                           SELECT CustomerCode, Name, PriceGroup, TaxStatus, DateChangedUtc, IsDeleted
                           FROM Customer
                           WHERE CustomerCode = @Code AND IsDeleted = 0
                           """;

        var row = await connection.QuerySingleOrDefaultAsync<CustomerRow>(new CommandDefinition(sql, new { Code = code }, cancellationToken: cancellationToken));
        return row?.ToDomain();
    }

    public async Task<IReadOnlyList<Customer>> SearchAsync(string search, int take, CancellationToken cancellationToken = default)
    {
        using var connection = connectionFactory.CreateConnection();
        const string sql = """
                           SELECT CustomerCode, Name, PriceGroup, TaxStatus, DateChangedUtc, IsDeleted
                           FROM Customer
                           WHERE IsDeleted = 0
                             AND (@Search = '' OR CustomerCode LIKE @Pattern OR Name LIKE @Pattern)
                           ORDER BY CustomerCode
                           LIMIT @Take
                           """;

        var rows = await connection.QueryAsync<CustomerRow>(new CommandDefinition(sql, new
        {
            Search = search ?? string.Empty,
            Pattern = $"%{search}%",
            Take = take
        }, cancellationToken: cancellationToken));

        return rows.Select(x => x.ToDomain()).ToList();
    }

    private sealed class CustomerRow
    {
        public string CustomerCode { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string PriceGroup { get; set; } = string.Empty;
        public string TaxStatus { get; set; } = string.Empty;
        public string DateChangedUtc { get; set; } = string.Empty;
        public int IsDeleted { get; set; }

        public Customer ToDomain() => new()
        {
            CustomerCode = CustomerCode,
            Name = Name,
            PriceGroup = PriceGroup,
            TaxStatus = TypeParsers.ParseTaxStatus(TaxStatus),
            DateChangedUtc = DateChangedUtc,
            IsDeleted = IsDeleted
        };
    }
}
