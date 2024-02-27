using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BrokerUnitTest;

/// <summary>
/// Base API test template class
/// </summary>
public abstract class BaseApiTest
{
    private IServiceProvider ServiceProvider { get; set; }

    protected IMediator Mediator { get; private set; }

    /// <summary>
    /// Initialize a new instance of <see cref="BaseApiTest"/>
    /// </summary>
    protected BaseApiTest()
    {
        IServiceCollection serviceCollection = new ServiceCollection().AddLogging();
        IConfigurationRoot config = this.GetConfigurationBuilder().Build();
        ServiceProvider = DiSetup(serviceCollection, config);

        Mediator = ServiceProvider.GetRequiredService<IMediator>();
    }

    protected virtual ConfigurationBuilder GetConfigurationBuilder()
    {
        return new ConfigurationBuilder();
    }
    protected abstract IServiceProvider DiSetup(IServiceCollection serviceCollection, IConfigurationRoot config);
}
