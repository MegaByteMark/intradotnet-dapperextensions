using System.Data.Common;
using System.Data.SqlClient;
using IntraDotNet.DapperExtensions.Context;

namespace intradotnet_dapperextensions_testapp;

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
        SqlConnection connection = new SqlConnection(Options.ConnectionString);

        return connection;
    }
}