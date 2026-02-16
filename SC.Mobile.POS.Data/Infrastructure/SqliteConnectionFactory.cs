using Microsoft.Data.Sqlite;
using SC.Mobile.POS.Core.Abstractions;
using System.Data;

namespace SC.Mobile.POS.Data.Infrastructure;

public sealed class SqliteConnectionFactory(IDatabasePathProvider databasePathProvider)
{
    public IDbConnection CreateConnection()
    {
        var connectionString = $"Data Source={databasePathProvider.GetDatabasePath()}";
        return new SqliteConnection(connectionString);
    }
}
