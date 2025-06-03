using System.Data.Common;
using IntraDotNet.DapperExtensions.Context;
using Microsoft.Data.SqlClient;

namespace IntraDotNet.DapperExtensions.Tests.Context;

public class TestDbDapperContext : DapperContext, ITestDbDapperContext
{
    public override DbConnection GetDbConnection()
    {
        SqlConnection connection = new(Options.ConnectionString);
        connection.Open();

        return connection;
    }
}