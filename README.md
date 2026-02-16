# SC.Mobile.POS

Offline-first POS foundation built with .NET MAUI Blazor Hybrid, MudBlazor, and SQLite (Dapper).

## Run

1. Restore dependencies:
   - `dotnet restore SC.Mobile.POS.slnx`
2. Build solution:
   - `dotnet build SC.Mobile.POS.slnx`
3. Run the MAUI app (Windows example):
   - `dotnet build SC.Mobile.POS.App/SC.Mobile.POS.App.csproj -f net10.0-windows10.0.19041.0`
   - Launch from Visual Studio or `dotnet run` for the chosen target framework.
4. Open `/new-sale`.

## Debug seed data behavior

In `DEBUG`, startup does:
- DB ensure-create
- idempotent seed (branch, price groups, customers, products, and prices)

## Windows DB file location

`%LocalAppData%\SC.POS\pos.db`

(Resolved via `Environment.SpecialFolder.LocalApplicationData` + `SC.POS`.)
