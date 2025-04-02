using System.Data;
using System.Data.Common;
using Microsoft.Extensions.Hosting;
using Dapper;

namespace intradotnet_dapperextensions_testapp;

public sealed class Worker : IHostedService, IHostedLifecycleService
{
    private readonly LocalSqlDapperContext _context;

    public Worker(LocalSqlDapperContext context)
    {
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

        // Simulate some work
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
