using System;
using Microsoft.Extensions.DependencyInjection;

namespace IntraDotNet.DapperExtensions.Context
{
    public static class DapperContextDependencyInjection
    {
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

            services.AddScoped<IDapperContext, TDapperContext>(provider =>
            {
                TDapperContext context = new TDapperContext();

                context.Configure(options);
                
                return context;
            });

            return services;
        }
    }
}