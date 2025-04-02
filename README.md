# IntraDotNet.DapperExtensions
Utility library for Dapper to improve the development experience of common Dapper scenarios.

## Initializing a Dependency Injectable Context Wrapper for Dapper
To initialize a dependency injectable context wrapper for Dapper, first you need to create a context class that inherits froms `DapperContext`, then implement the `GetDbConnection` function.
The following example uses a connection to an Sql Server database which requires installing either `System.Data.SqlClient` or `Microsoft.Data.SqlClient` depending on your target runtime, however you can use any database provider by installing the package relevant for that provider if the provider implements the .NET Data Provider schema. 

```csharp
using System.Data.Common;
using System.Data.SqlClient;

//Include the DapperExtensions namespace
using IntraDotNet.DapperExtensions.Context;

namespace TestApp;

/// <summary>
/// This class is used to create a local SQL Server Dapper context.
/// It inherits from the DapperContext class and overrides the GetDbConnection method.
/// The GetDbConnection method creates a new SqlConnection object using the connection string
/// provided in the DapperContextOptions.
/// </summary>
public class LocalSqlDapperContext : DapperContext
{
    public LocalSqlDapperContext() : base()
    {
    }

    public LocalSqlDapperContext(DapperContextOptions options) : base(options)
    {
    }

    public override DbConnection GetDbConnection()
    {
#pragma warning disable CS0618 // Type or member is obsolete
        SqlConnection connection = new SqlConnection(Options.ConnectionString);
#pragma warning restore CS0618 // Type or member is obsolete

        return connection;
    }
}
```

Then in your application startup register the DapperContext for DI by adding it to the services container. In the following example the connection string `LocalSql` is loaded from `appsettings.json`:

```csharp
internal class Program
{
    private static void Main(string[] args)
    {
        HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);
        builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

        builder.Services.AddDapperContext<LocalSqlDapperContext>(options =>
        {
            options.ConnectionString = builder.Configuration.GetConnectionString("LocalSql");
        });

        builder.Services.AddHostedService<Worker>();

        IHost host = builder.Build();
        host.Run();
    }
}
```

Then dependency inject the DapperContext at the point it is required in your code:

```csharp
using System.Data.Common;
using Microsoft.Extensions.Hosting;
using Dapper;

namespace TestApp;

public sealed class Worker : IHostedService, IHostedLifecycleService
{
    private readonly LocalSqlDapperContext _context;

    public Worker(LocalSqlDapperContext context)
    {
        //Dependency injected here
        _context = context;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        Console.WriteLine("Worker started.");
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        Console.WriteLine("Worker stopped.");
        return Task.CompletedTask;
    }

    public void OnStarting()
    {
        Console.WriteLine("OnStarting called.");
    }

    public void OnStopping()
    {
        Console.WriteLine("OnStopping called.");
    }

    public Task StartingAsync(CancellationToken cancellationToken)
    {
        Console.WriteLine("Worker starting.");
        return Task.CompletedTask;
    }

    public Task StartedAsync(CancellationToken cancellationToken)
    {
        Console.WriteLine("Worker started.");

        // Query executed here
        using DbConnection connection = _context.GetDbConnection();
        var id = connection.Query("SELECT 1 AS Id").FirstOrDefault();

        return Task.CompletedTask;
    }

    public Task StoppingAsync(CancellationToken cancellationToken)
    {
        Console.WriteLine("Worker stopping.");
        return Task.CompletedTask;
    }

    public Task StoppedAsync(CancellationToken cancellationToken)
    {
        Console.WriteLine("Worker stopped.");
        return Task.CompletedTask;
    }
}
```

**IMPORTANT**: Remember the `DbConnection` is unmanaged resources and must be wrapped in a using to prevent memory leakage.

## Contributing

Contributions are welcome! Please open an issue or submit a pull request.

## License

This project is licensed under the MIT License.