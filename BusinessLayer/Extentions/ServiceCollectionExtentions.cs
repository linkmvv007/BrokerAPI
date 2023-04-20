using BusinessLayer.Interfaces;
using BusinessLayer.Settings;
using BusinessLayer.Validators;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace BusinessLayer.Extentions;

/// <summary>
/// Adds additional libraries to the project with all necessary dependencies
/// </summary>
public static class ServiceCollectionExtentions
{
    /// <summary>
    /// Common dependencies
    /// </summary>
    /// <param name="serviceCollection"></param>
    /// <returns></returns>
    public static IServiceCollection AddCommonServiceDependencies(this IServiceCollection serviceCollection)
    {
        return serviceCollection
            .AddApiLayerHttpClientDependencies()
            .AddMediatorDependencies()
            .AddLogging(builder => builder.AddConsole())
            .AddFluentValidationDependencies()
            .AddRedisDependencies();
    }


    /// <summary>
    /// Api Layer dependencies
    /// </summary>
    /// <param name="serviceCollection"></param>
    /// <returns></returns>
    private static IServiceCollection AddApiLayerHttpClientDependencies(this IServiceCollection serviceCollection)
    {
        serviceCollection
            .AddSingleton<IApiLayerSettings, ApiLayerSettings>();

        serviceCollection
            .AddHttpClient<IApiLayerHttpClient, ApiLayerHttpClient>();

        return serviceCollection;
    }

    /// <summary>
    /// Mediator dependencies
    /// </summary>
    /// <param name="serviceCollection"></param>
    /// <returns></returns>
    public static IServiceCollection AddMediatorDependencies(this IServiceCollection serviceCollection) =>
        serviceCollection
        .AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

    /// <summary>
    /// Fluent validation dependencies
    /// </summary>
    /// <param name="serviceCollection"></param>
    /// <returns></returns>
    private static IServiceCollection AddFluentValidationDependencies(this IServiceCollection serviceCollection) =>
       serviceCollection
        .AddFluentValidationAutoValidation()
        .AddValidatorsFromAssemblyContaining<BestContextValidator>(lifetime: ServiceLifetime.Transient);


    /// <summary>
    /// Redis dependencies
    /// </summary>
    /// <param name="serviceCollection"></param>
    /// <returns></returns>
    private static IServiceCollection AddRedisDependencies(this IServiceCollection serviceCollection) =>
        serviceCollection
        .AddSingleton<IRedisSettings, RedisSettings>()
        .AddRedisCache();

    /// <summary>
    /// Add redis caching
    /// </summary>
    /// <param name="serviceCollections"></param>
    /// <returns></returns>
    private static IServiceCollection AddRedisCache(this IServiceCollection serviceCollections)
    {
        var serviceProvider = serviceCollections.BuildServiceProvider();
        var settings = serviceProvider.GetRequiredService<IRedisSettings>();

        serviceCollections.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = settings.Url;
            options.InstanceName = settings.Prefix;
        });

        return serviceCollections;
    }
}
