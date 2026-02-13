using SC.Mobile.POS.Core.Abstractions;

namespace SC.Mobile.POS.App.Infrastructure;

public sealed class LocalDatabasePathProvider : IDatabasePathProvider
{
    public string GetDatabasePath()
    {
        var basePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        var folder = Path.Combine(basePath, "SC.POS");
        Directory.CreateDirectory(folder);
        return Path.Combine(folder, "pos.db");
    }
}
