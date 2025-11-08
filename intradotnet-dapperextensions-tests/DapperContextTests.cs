using Microsoft.Extensions.DependencyInjection;
using IntraDotNet.DapperExtensions.Context;
using IntraDotNet.DapperExtensions.Tests.Context;
using Dapper;
using System.Data.Common;
using IntraDotNet.DapperExtensions.Tests.Fixtures;
using IntraDotNet.DapperExtensions.Tests.Models;
using Microsoft.Data.SqlClient;

namespace IntraDotNet.DapperExtensions.Tests;

[Collection("SqlServerTestCollection")]
public class DapperContextTests
{
    private readonly SqlServerTestContainerFixture _fixture;

    public DapperContextTests(SqlServerTestContainerFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public void ConnectToDbWithDapper_ConfigureAction_RunSelect()
    {
        // Arrange
        IServiceCollection services = new ServiceCollection();

        services.AddDapperContext<ITestDbDapperContext, TestDbDapperContext>(options =>
        {
            options.ConnectionString = _fixture.ConnectionString;
        });

        // Act
        using ServiceProvider serviceProvider = services.BuildServiceProvider();
        ITestDbDapperContext context = serviceProvider.GetRequiredService<ITestDbDapperContext>();

        Assert.NotNull(context);

        using (DbConnection connection = context.GetDbConnection())
        {
            Assert.NotNull(connection);

            IEnumerable<Category> result = connection.Query<Category>(Category.GetQuery);

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
        }
    }

    [Fact]
    public void ConnectToDbWithDapper_IOptions_RunSelect()
    {
        // Arrange
        IServiceCollection services = new ServiceCollection();

        services.Configure<DapperContextOptions>(options =>
        {
            options.ConnectionString = _fixture.ConnectionString;
        });

        services.AddDapperContext<ITestDbDapperContext, TestDbDapperContext>();

        // Act
        using ServiceProvider serviceProvider = services.BuildServiceProvider();
        ITestDbDapperContext context = serviceProvider.GetRequiredService<ITestDbDapperContext>();

        Assert.NotNull(context);

        using (DbConnection connection = context.GetDbConnection())
        {
            Assert.NotNull(connection);

            IEnumerable<Category> result = connection.Query<Category>(Category.GetQuery);

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
        }
    }

    [Fact]
    public void MultipleDapperContexts_DifferentConnectionStrings_CheckConnections()
    {
        // Arrange
        IServiceCollection services = new ServiceCollection();

        services.AddDapperContext<ITestDbDapperContext, TestDbDapperContext>(options =>
        {
            options.ConnectionString = _fixture.ConnectionString;
        });

        services.AddDapperContext<ISecondTestDbDapperContext, SecondTestDbDapperContext>(options =>
        {
            options.ConnectionString = _fixture.SecondConnectionString;
        });

        // Act
        using ServiceProvider serviceProvider = services.BuildServiceProvider();

        var testDbContext = serviceProvider.GetRequiredService<ITestDbDapperContext>();
        var secondDbContext = serviceProvider.GetRequiredService<ISecondTestDbDapperContext>();

        Assert.NotNull(testDbContext);
        Assert.NotNull(secondDbContext);

        using DbConnection testDbConnection = testDbContext.GetDbConnection();
        using DbConnection otherDbConnection = secondDbContext.GetDbConnection();

        var testConnStringBuilder = new SqlConnectionStringBuilder(testDbConnection.ConnectionString);
        var otherConnStringBuilder = new SqlConnectionStringBuilder(otherDbConnection.ConnectionString);
        var fixtureDbConnStringBuilder = new SqlConnectionStringBuilder(_fixture.ConnectionString);
        var secondFixtureDbConnStringBuilder = new SqlConnectionStringBuilder(_fixture.SecondConnectionString);

        // Assert
        Assert.Equal(fixtureDbConnStringBuilder.DataSource, testConnStringBuilder.DataSource);
        Assert.Equal(secondFixtureDbConnStringBuilder.DataSource, otherConnStringBuilder.DataSource);
    }
}
