using FitnessApp.ApiGateway.Extensions;
using Newtonsoft.Json;

namespace FitnessApp.ApiGateway.UnitTests;

public class UrlExtensionsTests
{
    internal class Params
    {
        public string? Param1 { get; set; }
        public string? Param2 { get; set; }
    }

    [Theory]
    [InlineData("fitness-app.com", "", "fitness-app.com")]
    [InlineData("fitness-app.com", null, "fitness-app.com")]
    [InlineData("fitness-app.com", "sv-test", "fitness-app.com/api/sv-test")]
    [InlineData("fitness-app.com/", "sv-test", "fitness-app.com/api/sv-test")]
    public void ApiExtension_AddsApiSegment(string url, string api, string expected)
    {
        // Act
        var result = url.Api(api);

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("fitness-app.com", "", "fitness-app.com")]
    [InlineData("fitness-app.com", null, "fitness-app.com")]
    [InlineData("fitness-app.com/api", "sv-test", "fitness-app.com/api/sv-test")]
    [InlineData("fitness-app.com/api/", "sv-test", "fitness-app.com/api/sv-test")]
    public void ApiExtension_AddsMethodSegment(string url, string method, string expected)
    {
        // Act
        var result = url.Method(method);

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("fitness-app.com", new string[0], "fitness-app.com")]
    [InlineData("fitness-app.com/api", new string[] { "sv-test1", "sv-test2" }, "fitness-app.com/api/sv-test1/sv-test2")]
    [InlineData("fitness-app.com/api/", new string[] { "sv-test1", "sv-test2" }, "fitness-app.com/api/sv-test1/sv-test2")]
    public void ApiExtension_AddsRoutesSegment(string url, string[] routes, string expected)
    {
        // Act
        var result = url.Routes(routes);

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(
        "fitness-app.com/api/sv-test",
        $$"""
        {}
        """,
        "fitness-app.com/api/sv-test")]
    [InlineData(
        "fitness-app.com/api/sv-test",
        $$"""
        {
            "Param1": "Value1"
        }
        """,
        "fitness-app.com/api/sv-test?Param1=Value1")]
    [InlineData(
        "fitness-app.com/api/sv-test",
        $$"""
        {
            "Param1": "Value1",
            "Param2": "Value2"
        }
        """,
        "fitness-app.com/api/sv-test?Param1=Value1&Param2=Value2")]
    public void ApiExtension_AddsQueryStringSegment(string url, string paramString, string expected)
    {
        // Arrange
        var svTest = JsonConvert.DeserializeObject<Params>(paramString);

        // Act
        var result = url.ToQueryString(svTest);

        // Assert
        Assert.Equal(expected, result);
    }
}
