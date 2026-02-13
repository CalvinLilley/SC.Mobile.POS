using Dapper;
using SC.Mobile.POS.Data.Infrastructure;

namespace SC.Mobile.POS.Data.Seed;

public sealed class DbSeeder(SqliteConnectionFactory connectionFactory)
{
    public async Task SeedDebugData(CancellationToken cancellationToken = default)
    {
        using var connection = connectionFactory.CreateConnection();
        var now = DateTime.UtcNow.ToString("O");

        var sql = """
                  INSERT OR IGNORE INTO Branch
                  (BranchCode, RetailPriceCode, WholesalePriceCode, PromoPriceCode, TaxCode_Taxable, TaxCode_NonTaxable, TaxRate, CashCustomerCode, DateChangedUtc)
                  VALUES
                  ('B001', 'R001', 'W001', 'P001', 'TAX', 'NON', 0.15, 'CASH', @Now);

                  INSERT OR IGNORE INTO PriceGroup (PriceGroup, CustomerType, DateChangedUtc, IsDeleted) VALUES ('RETAIL', 'Retail', @Now, 0);
                  INSERT OR IGNORE INTO PriceGroup (PriceGroup, CustomerType, DateChangedUtc, IsDeleted) VALUES ('WHOLE', 'Wholesale', @Now, 0);

                  INSERT OR IGNORE INTO Customer (CustomerCode, Name, PriceGroup, TaxStatus, DateChangedUtc, IsDeleted)
                  VALUES ('CASH', 'Walk-in Customer', 'RETAIL', 'Taxable', @Now, 0);
                  INSERT OR IGNORE INTO Customer (CustomerCode, Name, PriceGroup, TaxStatus, DateChangedUtc, IsDeleted)
                  VALUES ('WH001', 'Wholesale Customer', 'WHOLE', 'Taxable', @Now, 0);

                  INSERT OR IGNORE INTO Product (ProductCode, Description, TaxStatus, DateChangedUtc, IsDeleted)
                  VALUES ('P100', 'Milk 2L', 'Taxable', @Now, 0);
                  INSERT OR IGNORE INTO Product (ProductCode, Description, TaxStatus, DateChangedUtc, IsDeleted)
                  VALUES ('P200', 'Bread', 'Taxable', @Now, 0);
                  INSERT OR IGNORE INTO Product (ProductCode, Description, TaxStatus, DateChangedUtc, IsDeleted)
                  VALUES ('P300', 'Book (Non-taxable example)', 'NonTaxable', @Now, 0);
                  """;

        await connection.ExecuteAsync(new CommandDefinition(sql, new { Now = now }, cancellationToken: cancellationToken));

        await UpsertPriceList(connection, now, "R001", "P100", 25m, cancellationToken);
        await UpsertPriceList(connection, now, "R001", "P200", 18m, cancellationToken);
        await UpsertPriceList(connection, now, "R001", "P300", 100m, cancellationToken);

        await UpsertPriceList(connection, now, "W001", "P100", 22m, cancellationToken);
        await UpsertPriceList(connection, now, "W001", "P200", 16m, cancellationToken);
        await UpsertPriceList(connection, now, "W001", "P300", 95m, cancellationToken);

        await UpsertPriceList(connection, now, "P001", "P100", 20m, cancellationToken);
        await UpsertPriceList(connection, now, "P001", "P200", 19m, cancellationToken);
    }

    private static Task UpsertPriceList(System.Data.IDbConnection connection, string now, string priceCode, string productCode, decimal price, CancellationToken cancellationToken)
    {
        var sql = """
                  INSERT INTO PriceList (PriceCode, ProductCode, Price, DateChangedUtc, IsDeleted)
                  VALUES (@PriceCode, @ProductCode, @Price, @Now, 0)
                  ON CONFLICT(PriceCode, ProductCode) DO UPDATE SET
                      Price = excluded.Price,
                      DateChangedUtc = excluded.DateChangedUtc,
                      IsDeleted = 0;
                  """;

        return connection.ExecuteAsync(new CommandDefinition(sql, new
        {
            PriceCode = priceCode,
            ProductCode = productCode,
            Price = price,
            Now = now
        }, cancellationToken: cancellationToken));
    }
}
