using Dapper;
using SC.Mobile.POS.Core.Abstractions;
using SC.Mobile.POS.Core.Models;
using SC.Mobile.POS.Data.Infrastructure;
using SC.Mobile.POS.Data.Mappers;

namespace SC.Mobile.POS.Data.Repositories;

public sealed class ProductRepository(SqliteConnectionFactory connectionFactory) : IProductRepository
{
    public async Task<Product?> GetAsync(string code, CancellationToken cancellationToken = default)
    {
        using var connection = connectionFactory.CreateConnection();
        const string sql = """
                           SELECT ProductCode, Description, TaxStatus, DateChangedUtc, IsDeleted
                           FROM Product
                           WHERE ProductCode = @Code AND IsDeleted = 0
                           """;

        var row = await connection.QuerySingleOrDefaultAsync<ProductRow>(new CommandDefinition(sql, new { Code = code }, cancellationToken: cancellationToken));
        return row?.ToDomain();
    }

    public async Task<IReadOnlyList<Product>> SearchAsync(string search, int take, CancellationToken cancellationToken = default)
    {
        using var connection = connectionFactory.CreateConnection();
        const string sql = """
                           SELECT ProductCode, Description, TaxStatus, DateChangedUtc, IsDeleted
                           FROM Product
                           WHERE IsDeleted = 0
                             AND (@Search = '' OR ProductCode LIKE @Pattern OR Description LIKE @Pattern)
                           ORDER BY ProductCode
                           LIMIT @Take
                           """;

        var rows = await connection.QueryAsync<ProductRow>(new CommandDefinition(sql, new
        {
            Search = search ?? string.Empty,
            Pattern = $"%{search}%",
            Take = take
        }, cancellationToken: cancellationToken));

        return rows.Select(x => x.ToDomain()).ToList();
    }

    private sealed class ProductRow
    {
        public string ProductCode { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string TaxStatus { get; set; } = string.Empty;
        public string DateChangedUtc { get; set; } = string.Empty;
        public int IsDeleted { get; set; }

        public Product ToDomain() => new()
        {
            ProductCode = ProductCode,
            Description = Description,
            TaxStatus = TypeParsers.ParseTaxStatus(TaxStatus),
            DateChangedUtc = DateChangedUtc,
            IsDeleted = IsDeleted
        };
    }
}
