using Xunit;
using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;

public class usersTests
    : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public usersTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Valid_Request()
    {
        // Arrange
        var request = new
        {
            name = "sample-value",
            age = 123,
        };

        // Act
        var response =

        var responseContent =
            await response.Content
                .ReadAsStringAsync();

            await _client.PostAsJsonAsync("/users", request);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(
            HttpStatusCode.Created,
            response.StatusCode);
    }

    [Fact]
    public async Task Missing_name()
    {
        // Arrange
        var request = new
        {
            name = null,
            age = 123,
        };

        // Act
        var response =

        var responseContent =
            await response.Content
                .ReadAsStringAsync();

            await _client.PostAsJsonAsync("/users", request);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(
        Assert.Contains(
            "name",
            responseContent,
            StringComparison.OrdinalIgnoreCase);

        Assert.Contains(
            "required",
            responseContent,
            StringComparison.OrdinalIgnoreCase);

            HttpStatusCode.BadRequest,
            response.StatusCode);
    }

    [Fact]
    public async Task Invalid_age()
    {
        // Arrange
        var request = new
        {
            name = "sample-value",
            age = 123,
        };

        // Act
        var response =

        var responseContent =
            await response.Content
                .ReadAsStringAsync();

            await _client.PostAsJsonAsync("/users", request);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(
            HttpStatusCode.BadRequest,
            response.StatusCode);
    }

    [Fact]
    public async Task Empty_name()
    {
        // Arrange
        var request = new
        {
            name = "",
            age = 123,
        };

        // Act
        var response =

        var responseContent =
            await response.Content
                .ReadAsStringAsync();

            await _client.PostAsJsonAsync("/users", request);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(
        Assert.Contains(
            "name",
            responseContent,
            StringComparison.OrdinalIgnoreCase);

        Assert.Contains(
            "empty",
            responseContent,
            StringComparison.OrdinalIgnoreCase);

            HttpStatusCode.BadRequest,
            response.StatusCode);
    }

    [Fact]
    public async Task Large_name()
    {
        // Arrange
        var request = new
        {
            name = "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA",
            age = 123,
        };

        // Act
        var response =

        var responseContent =
            await response.Content
                .ReadAsStringAsync();

            await _client.PostAsJsonAsync("/users", request);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(
        Assert.Contains(
            "name",
            responseContent,
            StringComparison.OrdinalIgnoreCase);

        Assert.Contains(
            "length",
            responseContent,
            StringComparison.OrdinalIgnoreCase);

            HttpStatusCode.BadRequest,
            response.StatusCode);
    }

    [Fact]
    public async Task Null_name()
    {
        // Arrange
        var request = new
        {
            name = null,
            age = 123,
        };

        // Act
        var response =

        var responseContent =
            await response.Content
                .ReadAsStringAsync();

            await _client.PostAsJsonAsync("/users", request);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(
            HttpStatusCode.BadRequest,
            response.StatusCode);
    }

}
