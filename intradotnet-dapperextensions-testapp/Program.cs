using intradotnet_dapperextensions_testapp;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using IntraDotNet.DapperExtensions.Context;
using Microsoft.Extensions.Configuration;

internal class Program
{
    private static void Main(string[] args)
    {
        HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);
        
        builder.Services.AddDapperContext<LocalSqlDapperContext>(options =>
        {
            options.ConnectionString = builder.Configuration.GetConnectionString("LocalSql");
        });

        builder.Services.AddHostedService<Worker>();

        IHost host = builder.Build();
        host.Run();
    }
}