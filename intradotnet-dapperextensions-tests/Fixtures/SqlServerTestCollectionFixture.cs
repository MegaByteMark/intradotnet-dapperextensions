using Testcontainers.MsSql;
using Bogus;
using Microsoft.Data.SqlClient;
using Dapper;
using IntraDotNet.DapperExtensions.Tests.Models;
using DotNet.Testcontainers.Builders;

namespace IntraDotNet.DapperExtensions.Tests.Fixtures;

public class SqlServerTestContainerFixture : IAsyncLifetime
{
    public MsSqlContainer? DbContainer { get; private set; }
    public string ConnectionString { get; private set; } = string.Empty;

    public async Task InitializeAsync()
    {
        // Generate a random complex password using Bogus
        var password = new Faker().Internet.Password(16, false, "", "!1Aa");

        DbContainer = new MsSqlBuilder()
            .WithPassword(password)
            .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
            .WithCleanUp(true)
            .WithWaitStrategy(Wait.ForUnixContainer().UntilMessageIsLogged("SQL Server is now ready for client connections."))
            .Build();

        await DbContainer.StartAsync();
        ConnectionString = DbContainer.GetConnectionString();

        using SqlConnection connection = new SqlConnection(ConnectionString);
        await connection.OpenAsync();

        await connection.ExecuteAsync(@"
            CREATE TABLE dbo.Category (
                Id INT PRIMARY KEY IDENTITY,
                Name NVARCHAR(100) NOT NULL
            );
        ");

        var categories = new Faker<Category>()
            .RuleFor(c => c.Name, f => f.Commerce.Categories(1)[0])
            .Generate(10);

        foreach (Category category in categories)
        {
            await connection.ExecuteAsync("INSERT INTO dbo.Category (Name) VALUES (@Name)", category);
        }
    }

    public async Task DisposeAsync()
    {
        if (DbContainer != null)
        {
            await DbContainer.DisposeAsync();
        }
    }
}