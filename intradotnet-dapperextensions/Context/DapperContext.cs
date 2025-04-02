using System;
using System.Data.Common;

namespace IntraDotNet.DapperExtensions.Context
{
    public abstract class DapperContext: IDapperContext
    {
        protected DapperContextOptions Options;

        public DapperContext()
        {
            Options = new DapperContextOptions();
        }

        public DapperContext(DapperContextOptions options)
        {
            Configure(options);
        }

        public IDapperContext Configure(DapperContextOptions options)
        {
            Options = options ?? throw new ArgumentNullException(nameof(options));

            return this;
        }

        public abstract DbConnection GetDbConnection();
    }
}