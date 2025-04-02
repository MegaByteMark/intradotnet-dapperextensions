using System;
using System.Data.Common;

namespace IntraDotNet.DapperExtensions.Context
{
    /// <summary>
    /// This is an abstract class that represents a Dapper context.
    /// It provides a way to configure the context with options and get a database connection.
    /// The class implements the IDapperContext interface.
    /// </summary>
    public abstract class DapperContext: IDapperContext
    {
        /// <summary>
        /// The options used to configure the Dapper context.
        /// This includes the connection string and other settings.
        /// </summary>
        protected DapperContextOptions Options;

        /// <summary>
        /// The constructor initializes the Dapper context with default options.
        /// It creates a new instance of DapperContextOptions.
        /// This is used to set up the connection string and other settings.
        /// </summary>
        public DapperContext()
        {
            Options = new DapperContextOptions();
        }

        /// <summary>
        /// The constructor initializes the Dapper context with the provided options.
        /// It throws an ArgumentNullException if the options are null.
        /// This is used to set up the connection string and other settings.
        /// </summary>
        /// <param name="options">The options used to configure the Dapper context.</param>
        /// <exception cref="ArgumentNullException">Thrown when the options are null.</exception>
        /// <remarks>
        /// This constructor is used to create a Dapper context with specific options.
        /// The options include the connection string and other settings.
        /// </remarks>
        /// <example>
        /// <code>
        /// var options = new DapperContextOptions
        /// {
        ///     ConnectionString = "your_connection_string"
        /// };
        /// var dapperContext = new DapperContext(options);
        /// </code>
        /// </example>
        /// <remarks>
        public DapperContext(DapperContextOptions options)
        {
            Configure(options);
        }

        /// <summary>
        /// This method configures the Dapper context with the provided options.
        /// It throws an ArgumentNullException if the options are null.
        /// This is used to set up the connection string and other settings.
        /// </summary>
        /// <param name="options">The options used to configure the Dapper context.</param>
        /// <exception cref="ArgumentNullException">Thrown when the options are null.</exception>
        /// <remarks>
        /// This method is used to configure the Dapper context with specific options.
        /// The options include the connection string and other settings.
        /// </remarks>
        /// <example>
        /// <code>
        /// var options = new DapperContextOptions
        /// {
        ///     ConnectionString = "your_connection_string"
        /// };
        /// var dapperContext = new DapperContext();
        /// dapperContext.Configure(options);
        /// </code>
        /// </example>
        /// <remarks>
        public IDapperContext Configure(DapperContextOptions options)
        {
            Options = options ?? throw new ArgumentNullException(nameof(options));

            return this;
        }

        /// <summary>
        /// This method is used to get a database connection.
        /// It is an abstract method that must be implemented by derived classes.
        /// The derived class should provide the specific implementation for getting a database connection.
        /// </summary>
        /// <returns>A DbConnection object representing the database connection.</returns>
        /// <remarks>
        /// This method is used to get a database connection.
        /// The derived class should provide the specific implementation for getting a database connection.
        /// </remarks>
        /// <example>
        /// <code>
        /// public class MyDapperContext : DapperContext
        /// {
        ///     public override DbConnection GetDbConnection()
        ///     {
        ///         // Provide the specific implementation for getting a database connection.
        ///         return new SqlConnection(Options.ConnectionString);
        ///     }
        /// }
        /// </code>
        /// </example>
        public abstract DbConnection GetDbConnection();
    }
}