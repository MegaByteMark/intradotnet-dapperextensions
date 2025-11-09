using System.Data.Common;
using IntraDotNet.DapperExtensions.Context;
using Microsoft.Data.SqlClient;

namespace IntraDotNet.DapperExtensions.Tests.Context;

public class SecondTestDbDapperContext : DapperContext, ISecondTestDbDapperContext
{
    public override DbConnection GetDbConnection()
    {
        SqlConnection connection = new(Options.ConnectionString);
        connection.Open();

        return connection;
    }
}