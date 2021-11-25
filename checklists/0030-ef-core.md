# Programming with Entity Framework

[Documentation...](https://docs.microsoft.com/en-us/ef/core/get-started/)

## Install EF

* [Install EF Tools globally](https://docs.microsoft.com/en-us/ef/core/cli/dotnet)
  * `dotnet tool install --global dotnet-ef`
  * Update with `dotnet tool update --global dotnet-ef` (add `--version ...` to install a prerelease)
* `dotnet add package Microsoft.EntityFrameworkCore.SqlServer`
  * [Choose your DB provider...](https://docs.microsoft.com/en-us/ef/core/providers/)
* `dotnet add package Microsoft.EntityFrameworkCore.Design`
* Optional: Add NuGet package with helpers to diagnose migrations-related errors
  * `dotnet add package Microsoft.AspNetCore.Diagnostics.EntityFrameworkCore`

## Connection String in `appsettings.json`

* [Documentation...](https://docs.microsoft.com/en-us/ef/core/miscellaneous/connection-strings#aspnet-core)
* Create *appsettings.json* file:

    ```json
    "ConnectionStrings": {
        "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=AddressBook;Trusted_Connection=True"
    }
    ```

* Read connection string in ASP.NET Core's startup class:

    ```csharp
    ...
    builder.Services.AddDbContext<MyDbContext>(
        options => options.UseSqlServer(builder.Configuration["ConnectionStrings:DefaultConnection"]))
        .AddDatabaseDeveloperPageExceptionFilter();
    ...
    ```
