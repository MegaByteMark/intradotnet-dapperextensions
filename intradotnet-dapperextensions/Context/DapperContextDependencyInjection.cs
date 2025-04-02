using System;
using Microsoft.Extensions.DependencyInjection;

namespace IntraDotNet.DapperExtensions.Context
{
    /// <summary>
    /// This class provides extension methods for registering a Dapper context with dependency injection.
    /// It allows you to configure the Dapper context options and register it with the service collection.
    /// </summary>
    /// <remarks>
    /// This class is used to register a Dapper context with dependency injection.
    /// It provides a method to configure the Dapper context options and register it with the service collection.
    /// </remarks>
    /// <example>
    /// <code>
    /// var services = new ServiceCollection();
    /// services.AddDapperContext<LocalSqlDapperContext>(options =>
    /// {
    ///     options.ConnectionString = "your_connection_string";
    /// });
    /// </code>
    /// </example>
    public static class DapperContextDependencyInjection
    {
        /// <summary>
        /// This method registers a Dapper context with dependency injection.
        /// It allows you to configure the Dapper context options and register it with the service collection.
        /// </summary>
        /// <typeparam name="TDapperContext">The type of the Dapper context to register.</typeparam>
        /// <param name="services">The service collection to register the Dapper context with.</param>
        /// <param name="configureOptions">The action to configure the Dapper context options.</param>
        /// <returns>The service collection with the Dapper context registered.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the configureOptions action is null.</exception>
        /// <exception cref="ArgumentException">Thrown when the connection string is not configured.</exception>
        /// <remarks>
        /// This method is used to register a Dapper context with dependency injection.
        /// It provides a way to configure the Dapper context options and register it with the service collection.
        /// </remarks>
        /// <example>
        /// <code>
        /// var services = new ServiceCollection();
        /// services.AddDapperContext<LocalSqlDapperContext>(options =>
        /// {
        ///     options.ConnectionString = "your_connection_string";
        /// });
        /// </code>
        /// </example>
        public static IServiceCollection AddDapperContext<TDapperContext>(this IServiceCollection services, Action<DapperContextOptions> configureOptions)
        where TDapperContext : class, IDapperContext, new()
        {
            if (configureOptions == null)
            {
                throw new ArgumentNullException(nameof(configureOptions));
            }

            var options = new DapperContextOptions();
            configureOptions(options);

            if (string.IsNullOrEmpty(options.ConnectionString))
            {
                throw new ArgumentException("Connection string is not configured.");
            }

            services.AddScoped(provider =>
            {
                TDapperContext context = new TDapperContext();

                context.Configure(options);
                
                return context;
            });

            return services;
        }
    }
}