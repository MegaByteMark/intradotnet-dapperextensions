using Microsoft.Extensions.DependencyInjection;
using IntraDotNet.DapperExtensions.Context;
using IntraDotNet.DapperExtensions.Tests.Context;
using Dapper;

namespace IntraDotNet.DapperExtensions.Tests;

public class DapperContextTests
{
    [Fact]
    public void ConnectToDbWithDapper_ConfigureAction_RunSelect()
    {
        // Arrange
        var connectionString = "Server=localhost;Database=TestDb;User Id=sa;Password=reallyStrongPassword123;TrustServerCertificate=True;";

        var services = new ServiceCollection();
        services.AddDapperContext<ITestDbDapperContext, TestDbDapperContext>(options =>
        {
            options.ConnectionString = connectionString;
        });

        // Act
        var context = services.BuildServiceProvider().GetRequiredService<ITestDbDapperContext>();
        Assert.NotNull(context);
        using (var connection = context.GetDbConnection())
        {
            Assert.NotNull(connection);
            var result = connection.Query("SELECT * FROM dbo.Category");

            // Assert
            Assert.NotNull(result);
        }
    }

    [Fact]
    public void ConnectToDbWithDapper_IOptions_RunSelect()
    {
        // Arrange
        var connectionString = "Server=localhost;Database=TestDb;User Id=sa;Password=reallyStrongPassword123;TrustServerCertificate=True;";

        var services = new ServiceCollection();

        services.Configure<DapperContextOptions>(options =>
        {
            options.ConnectionString = connectionString;
        });

        services.AddDapperContext<ITestDbDapperContext, TestDbDapperContext>();

        // Act
        var context = services.BuildServiceProvider().GetRequiredService<ITestDbDapperContext>();
        Assert.NotNull(context);
        using (var connection = context.GetDbConnection())
        {
            Assert.NotNull(connection);
            var result = connection.Query("SELECT * FROM dbo.Category");

            // Assert
            Assert.NotNull(result);
        }
    }
}
