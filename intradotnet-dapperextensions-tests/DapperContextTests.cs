using Microsoft.Extensions.DependencyInjection;
using IntraDotNet.DapperExtensions.Context;
using IntraDotNet.DapperExtensions.Tests.Context;
using Dapper;
using System.Data.Common;
using IntraDotNet.DapperExtensions.Tests.Fixtures;
using IntraDotNet.DapperExtensions.Tests.Models;

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
}
