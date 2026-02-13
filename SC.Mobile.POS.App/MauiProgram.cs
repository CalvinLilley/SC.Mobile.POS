using Microsoft.Extensions.Logging;
using MudBlazor.Services;
using SC.Mobile.POS.App.Infrastructure;
using SC.Mobile.POS.Core.Abstractions;
using SC.Mobile.POS.Core.Services;
using SC.Mobile.POS.Data;
using SC.Mobile.POS.Data.Infrastructure;
using SC.Mobile.POS.Data.Repositories;
using SC.Mobile.POS.Data.Seed;

namespace SC.Mobile.POS.App;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        SQLitePCL.Batteries_V2.Init();

        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts => { fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular"); });

        builder.Services.AddMauiBlazorWebView();
        builder.Services.AddMudServices();

        builder.Services.AddSingleton<IDatabasePathProvider, LocalDatabasePathProvider>();
        builder.Services.AddSingleton<SqliteConnectionFactory>();
        builder.Services.AddSingleton<DbInitializer>();
        builder.Services.AddSingleton<DbSeeder>();

        builder.Services.AddSingleton<IBranchRepository, BranchRepository>();
        builder.Services.AddSingleton<ICustomerRepository, CustomerRepository>();
        builder.Services.AddSingleton<IProductRepository, ProductRepository>();
        builder.Services.AddSingleton<IPriceGroupRepository, PriceGroupRepository>();
        builder.Services.AddSingleton<IPriceRepository, PriceRepository>();

        builder.Services.AddSingleton<IPricingEngine, PricingEngine>();
        builder.Services.AddSingleton<ITaxEngine, TaxEngine>();

#if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
        builder.Logging.AddDebug();
#endif

        var app = builder.Build();

#if DEBUG
        using var scope = app.Services.CreateScope();
        var initializer = scope.ServiceProvider.GetRequiredService<DbInitializer>();
        initializer.EnsureCreated().GetAwaiter().GetResult();
        var seeder = scope.ServiceProvider.GetRequiredService<DbSeeder>();
        seeder.SeedDebugData().GetAwaiter().GetResult();
#endif

        return app;
    }
}
