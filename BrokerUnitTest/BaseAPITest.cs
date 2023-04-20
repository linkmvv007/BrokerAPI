using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BrokerUnitTest;

/// <summary>
/// Base API test template class
/// </summary>
public abstract class BaseAPITest
{
    public IServiceProvider ServiceProvider { get; private set; }

    public IMediator Mediator { get; private set; }

    /// <summary>
    /// Initialize a new instance of <see cref="BaseAPITest"/>
    /// </summary>
    public BaseAPITest()
    {
        IServiceCollection serviceCollection = new ServiceCollection().AddLogging();
        IConfigurationRoot config = this.GetConfigurationBuilder().Build();
        ServiceProvider = DISetup(serviceCollection, config);

        Mediator = ServiceProvider.GetRequiredService<IMediator>();
    }

    protected virtual ConfigurationBuilder GetConfigurationBuilder()
    {
        return new ConfigurationBuilder();
    }
    protected abstract IServiceProvider DISetup(IServiceCollection serviceCollection, IConfigurationRoot config);
}
