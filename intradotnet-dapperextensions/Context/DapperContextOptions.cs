using System;

namespace IntraDotNet.DapperExtensions.Context
{
    public class DapperContextOptions
    {
        public string ConnectionString { get; set; } = string.Empty;

        public DapperContextOptions()
        {
        }

        public DapperContextOptions(string connectionString)
        {
            ConnectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }
    }
}