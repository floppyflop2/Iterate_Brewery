using Microsoft.VisualStudio.TestPlatform.TestHost;
using Brewery;

namespace IntegrationTests;

public partial class BreweryApiTest : IClassFixture<CustomWebApplicationFactory<Brewery.Program>>
{
    private readonly CustomWebApplicationFactory<Brewery.Program> _factory;
    private readonly HttpClient _httpClient;

    public BreweryApiTest(CustomWebApplicationFactory<Brewery.Program> factory)
    {
        _factory = factory;
        _httpClient = _factory.CreateClient();
    }

    [Theory]
    [InlineData("/Beers")]
    [InlineData("/Brewery")]
    public async Task Get_EndpointsReturnSuccessAndCorrectContentType(string url)
    {
        // Arrange


        // Act
        var response = await _httpClient.GetAsync(url);

        // Assert
        response.EnsureSuccessStatusCode(); // Status Code 200-299
        Assert.Equal("application/json; charset=utf-8",
            response.Content.Headers.ContentType.ToString());
    }
}
