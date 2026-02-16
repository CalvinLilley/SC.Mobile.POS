using Dapper;
using SC.Mobile.POS.Data.Infrastructure;

namespace SC.Mobile.POS.Data;

public sealed class DbInitializer(SqliteConnectionFactory connectionFactory)
{
    public async Task EnsureCreated(CancellationToken cancellationToken = default)
    {
        using var connection = connectionFactory.CreateConnection();

        var sql = """
                  CREATE TABLE IF NOT EXISTS Branch(
                    BranchCode TEXT PRIMARY KEY,
                    RetailPriceCode TEXT NOT NULL,
                    WholesalePriceCode TEXT NOT NULL,
                    PromoPriceCode TEXT NULL,
                    TaxCode_Taxable TEXT NOT NULL,
                    TaxCode_NonTaxable TEXT NOT NULL,
                    TaxRate REAL NOT NULL,
                    CashCustomerCode TEXT NOT NULL,
                    DateChangedUtc TEXT NOT NULL
                  );

                  CREATE TABLE IF NOT EXISTS PriceGroup(
                    PriceGroup TEXT PRIMARY KEY,
                    CustomerType TEXT NOT NULL,
                    DateChangedUtc TEXT NOT NULL,
                    IsDeleted INTEGER NOT NULL DEFAULT 0
                  );

                  CREATE TABLE IF NOT EXISTS Customer(
                    CustomerCode TEXT PRIMARY KEY,
                    Name TEXT NOT NULL,
                    PriceGroup TEXT NOT NULL,
                    TaxStatus TEXT NOT NULL,
                    DateChangedUtc TEXT NOT NULL,
                    IsDeleted INTEGER NOT NULL DEFAULT 0
                  );

                  CREATE TABLE IF NOT EXISTS Product(
                    ProductCode TEXT PRIMARY KEY,
                    Description TEXT NOT NULL,
                    TaxStatus TEXT NOT NULL,
                    DateChangedUtc TEXT NOT NULL,
                    IsDeleted INTEGER NOT NULL DEFAULT 0
                  );

                  CREATE TABLE IF NOT EXISTS PriceList(
                    PriceCode TEXT NOT NULL,
                    ProductCode TEXT NOT NULL,
                    Price REAL NOT NULL,
                    DateChangedUtc TEXT NOT NULL,
                    IsDeleted INTEGER NOT NULL DEFAULT 0,
                    PRIMARY KEY (PriceCode, ProductCode)
                  );

                  CREATE INDEX IF NOT EXISTS IX_PriceList_Product ON PriceList(ProductCode);
                  """;

        await connection.ExecuteAsync(new CommandDefinition(sql, cancellationToken: cancellationToken));
    }
}
