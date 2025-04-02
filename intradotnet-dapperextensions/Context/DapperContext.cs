using System;
using System.Data.Common;

namespace IntraDotNet.DapperExtensions.Context
{
    public abstract class DapperContext: IDapperContext
    {
        private string _connectionString;

        public DapperContext(DapperContextOptions options)
        {
            Configure(options);
        }

        public IDapperContext Configure(DapperContextOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            _connectionString = options.ConnectionString;

            return this;
        }

        public abstract DbConnection GetDbConnection();
    }
}