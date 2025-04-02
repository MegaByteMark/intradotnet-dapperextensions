using System.Data.Common;

namespace IntraDotNet.DapperExtensions.Context
{
    public interface IDapperContext
    {
        IDapperContext Configure(DapperContextOptions options);
        DbConnection GetDbConnection();
    }
}