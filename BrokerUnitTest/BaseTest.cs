using BrokerUnitTest.Mocks;
using BusinessLayer;
using BusinessLayer.Extensions;
using BusinessLayer.Interfaces;
using BusinessLayer.Mediator;
using BusinessLayer.Validators;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;

namespace BrokerUnitTest;

/// <summary>
/// Initialization services
/// </summary>
public abstract class BaseTest : BaseApiTest
{
    protected override IServiceProvider DiSetup(IServiceCollection serviceCollection, IConfigurationRoot config)
    {
        serviceCollection.AddSingleton<IApiLayerSettings>(provider => ApiLayerSettingMock.GetMock().Object);

        serviceCollection.AddSingleton<IApiLayerHttpClient>(provider => ApiLayerHttpClientMock.GetMock().Object);

        serviceCollection.AddSingleton(Mock.Of<ILogger<ApiLayerHttpClient>>());
        serviceCollection.AddSingleton(Mock.Of<ILogger<CalculateBestRevenueQueryHandler>>());
        serviceCollection.AddTransient<IConfiguration>(x => config);

        serviceCollection
            .AddMediatorDependencies();

        serviceCollection.
            AddLogging(builder => builder.AddConsole());

        serviceCollection
            .AddFluentValidationAutoValidation()
            .AddValidatorsFromAssemblyContaining<BestContextValidator>(lifetime: ServiceLifetime.Transient);


        return serviceCollection.BuildServiceProvider();
    }
}
