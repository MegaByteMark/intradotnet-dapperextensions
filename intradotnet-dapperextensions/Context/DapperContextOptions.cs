using System;

namespace IntraDotNet.DapperExtensions.Context
{
    /// <summary>
    /// This class represents the options for configuring a Dapper context.
    /// It contains properties for the connection string and other options.
    /// </summary>
    public class DapperContextOptions
    {
        /// <summary>
        /// Gets or sets the connection string for the Dapper context.
        /// </summary>
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