using BusinessLayer.Interfaces;
using Moq;

namespace BrokerUnitTest.Mocks;

/// <summary>
/// API Layer Settings mock object
/// </summary>
public static class ApiLayerSettingMock
{
    public static Mock<IApiLayerSettings> GetMock()
    {
        Mock<IApiLayerSettings> mock = new();
        mock.SetupGet(x => x.Url).Returns("http://example.com");
        mock.SetupGet(x => x.ApiKey).Returns("12345");

        return mock;
    }

}