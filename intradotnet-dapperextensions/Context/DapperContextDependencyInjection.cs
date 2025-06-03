using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

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
        /// services.AddDapperContext<LocalSqlDapperContext>(options =>
        /// {
        ///     options.ConnectionString = "your_connection_string";
        /// });
        /// </code>
        /// </example>
        public static IServiceCollection AddDapperContext<TDapperContext>(
            this IServiceCollection services,
            Action<DapperContextOptions> configureOptions)
            where TDapperContext : class, IDapperContext, new()
        {
            return services.AddDapperContext<TDapperContext, TDapperContext>(configureOptions);
        }

        /// <summary>
        /// This method registers a Dapper context with dependency injection with a given interface implementation to support mocking.
        /// It allows you to configure the Dapper context options and register it with the service collection.
        /// </summary>
        /// <typeparam name="TIDapperContext">The type of the Dapper context interface to register.</typeparam>
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
        /// services.AddDapperContext<ILocalSqlDapperContext, LocalSqlDapperContext>(options =>
        /// {
        ///     options.ConnectionString = "your_connection_string";
        /// });
        /// </code>
        /// </example>
        public static IServiceCollection AddDapperContext<TIDapperContext, TDapperContext>(
            this IServiceCollection services,
            Action<DapperContextOptions> configureOptions)
            where TDapperContext : class, IDapperContext, TIDapperContext, new()
            where TIDapperContext : class, IDapperContext
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (configureOptions == null) throw new ArgumentNullException(nameof(configureOptions));

            services.Configure(configureOptions);
            return AddDapperContextInternal<TIDapperContext, TDapperContext>(services);
        }

        /// <summary>
        /// This method registers a Dapper context with dependency injection using configuration from appsettings.json.
        /// It allows you to register the Dapper context with the service collection without explicitly configuring options.
        /// </summary>
        /// <typeparam name="TIDapperContext">The type of the Dapper context interface to register.</typeparam>
        /// <typeparam name="TDapperContext">The type of the Dapper context to register.</typeparam>
        /// <param name="services">The service collection to register the Dapper context with.</param>
        /// <returns>The service collection with the Dapper context registered.</returns>
        /// <remarks>
        /// This method is used to register a Dapper context with dependency injection using configuration from appsettings.json.
        /// It provides a way to register the Dapper context with the service collection without explicitly configuring options.
        /// </remarks>
        /// <example>
        /// <code>
        /// // In appsettings.json:
        /// // "DapperContextOptions": {
        /// //   "ConnectionString": "your_connection_string"
        /// // }
        ///
        /// var services = new ServiceCollection();
        /// services.Configure<DapperContextOptions>(configuration.GetSection("DapperContextOptions"));
        /// services.AddDapperContext<ILocalSqlDapperContext, LocalSqlDapperContext>();
        /// </code>
        /// </example>
        public static IServiceCollection AddDapperContext<TIDapperContext, TDapperContext>(
            this IServiceCollection services)
            where TDapperContext : class, IDapperContext, TIDapperContext, new()
            where TIDapperContext : class, IDapperContext
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            return AddDapperContextInternal<TIDapperContext, TDapperContext>(services);
        }

        /// <summary>
        /// This method registers a Dapper context with dependency injection without any configuration.
        /// It allows you to register the Dapper context with the service collection without explicitly configuring options.
        /// </summary>
        /// <typeparam name="TIDapperContext">The type of the Dapper context interface to register.</typeparam>
        /// <typeparam name="TDapperContext">The type of the Dapper context to register.</typeparam>
        /// <param name="services">The service collection to register the Dapper context with.</param>
        /// <returns>The service collection with the Dapper context registered.</returns>
        /// <remarks>
        /// This method is used to register a Dapper context with dependency injection without any configuration.
        /// It provides a way to register the Dapper context with the service collection without explicitly configuring options.
        /// </remarks>
        private static IServiceCollection AddDapperContextInternal<TIDapperContext, TDapperContext>(
            IServiceCollection services)
            where TDapperContext : class, IDapperContext, TIDapperContext, new()
            where TIDapperContext : class, IDapperContext
        {
            services.AddScoped<TIDapperContext, TDapperContext>(provider =>
            {
                if (provider == null) throw new ArgumentNullException(nameof(provider));

                // Retrieve the Dapper context options from the service provider.
                // This assumes that the options have been configured previously.
                // If the options are not configured, an exception will be thrown.
                DapperContextOptions options = provider.GetRequiredService<IOptions<DapperContextOptions>>().Value;
                ValidateOptions(options);

                TDapperContext context = new TDapperContext();
                context.Configure(options);

                return context;
            });

            return services;
        }

        /// <summary>
        /// This method validates the Dapper context options to ensure that the connection string is configured.
        /// It throws an ArgumentException if the connection string is not configured.
        /// </summary>
        /// <param name="options">The Dapper context options to validate.</param>
        /// <exception cref="ArgumentException">Thrown when the connection string is not configured.</exception>
        /// <remarks>
        /// This method is used to validate the Dapper context options.
        /// It ensures that the connection string is configured before creating a Dapper context.
        /// </remarks>
        private static void ValidateOptions(DapperContextOptions options)
        {
            if (string.IsNullOrEmpty(options.ConnectionString))
                throw new ArgumentException("Connection string is not configured.");
        }
    }
}