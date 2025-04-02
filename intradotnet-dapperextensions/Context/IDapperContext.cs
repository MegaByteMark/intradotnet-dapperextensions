using System.Data.Common;

namespace IntraDotNet.DapperExtensions.Context
{
    /// <summary>
    /// This interface defines the contract for a Dapper context.
    /// It provides methods to configure the context and retrieve a database connection.
    /// </summary>
    public interface IDapperContext
    {
        IDapperContext Configure(DapperContextOptions options);
        DbConnection GetDbConnection();
    }
}