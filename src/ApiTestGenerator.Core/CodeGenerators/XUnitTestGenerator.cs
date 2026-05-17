using ApiTestGenerator.Core.Enums;
using ApiTestGenerator.Core.Models;
using ApiTestGenerator.Core.Testing;
using ApiTestGenerator.Core.Utilities;
using System.Text;

namespace ApiTestGenerator.Core.CodeGenerators;

public class XUnitTestGenerator
{
    private readonly SampleDataGenerator _sampleDataGenerator = new();
    private readonly EndpointUrlBuilder _urlBuilder = new();
    public string Generate(
        ApiEndpointDefinition endpoint,
        List<TestScenario> scenarios)
    {
        var builder =
            new StringBuilder();

        var className =
            GenerateClassName(endpoint);

        builder.AppendLine(
            "using Xunit;");

        builder.AppendLine(
            "using System.Net;");

        builder.AppendLine(
            "using System.Net.Http.Json;");

        builder.AppendLine(
            "using Microsoft.AspNetCore.Mvc.Testing;");

        builder.AppendLine();

        builder.AppendLine(
            $"public class {className}");

        builder.AppendLine(
            "    : IClassFixture<WebApplicationFactory<Program>>");

        builder.AppendLine("{");

        builder.AppendLine(
            "    private readonly HttpClient _client;");

        builder.AppendLine();

        builder.AppendLine(
            $"    public {className}(WebApplicationFactory<Program> factory)");

        builder.AppendLine(
            "    {");

        builder.AppendLine(
            "        _client = factory.CreateClient();");

        builder.AppendLine(
            "    }");

        builder.AppendLine();

        foreach (var scenario in scenarios)
        {
            GenerateTestMethod(
                builder,
                scenario,
                endpoint);
        }

        builder.AppendLine("}");

        return builder.ToString();
    }

    private string GenerateClassName(
    ApiEndpointDefinition endpoint)
    {
        var route =
            endpoint.Route
                .Replace("/", "_")
                .Replace("{", "")
                .Replace("}", "")
                .Replace("-", "_");

        route =
            route.Trim('_');

        return $"{route}Tests";
    }

    private void GenerateTestMethod(
    StringBuilder builder,
    TestScenario scenario,
    ApiEndpointDefinition endpoint)
    {
        var methodName =
            scenario.Name
                .Replace(" ", "_")
                .Replace("-", "_");

        builder.AppendLine(
            "    [Fact]");

        builder.AppendLine(
            $"    public async Task {methodName}()");

        builder.AppendLine(
            "    {");

        builder.AppendLine(
            "        // Arrange");

        var bodyParameters =
                endpoint.Parameters
                    .Where(x =>
                        x.Source ==
                        ParameterSource.Body)
                    .ToList();

        if (bodyParameters.Any())
        {
            builder.AppendLine(
                "        var request = new");

            builder.AppendLine(
                "        {");

            foreach (var parameter in bodyParameters)
            {
                var sampleValue =
                    scenario.Overrides.TryGetValue(
                        parameter.Name,
                        out var overrideValue)
                    ? overrideValue
                    : _sampleDataGenerator.GenerateValue(
                        parameter);

                builder.AppendLine(
                    $"            {parameter.Name} = {sampleValue},");
            }

            builder.AppendLine(
                "        };");

            builder.AppendLine();
        }

        builder.AppendLine(
            "        // Act");

        var route =
    endpoint.Route;

        builder.AppendLine(
            "        var response =");

        builder.AppendLine();

        builder.AppendLine(
            "        var responseContent =");

        builder.AppendLine(
            "            await response.Content");

        builder.AppendLine(
            "                .ReadAsStringAsync();");

        builder.AppendLine();

        if (endpoint.Verb == Enums.HttpVerb.Post)
        {
            builder.AppendLine(
                $"            await _client.PostAsJsonAsync(\"{route}\", request);");
        }
        else if (endpoint.Verb == Enums.HttpVerb.Put)
        {
            builder.AppendLine(
                $"            await _client.PutAsJsonAsync(\"{route}\", request);");
        }
        else if (endpoint.Verb == Enums.HttpVerb.Get)
        {
            builder.AppendLine(
                $"            await _client.GetAsync(\"{route}\");");
        }
        else if (endpoint.Verb == Enums.HttpVerb.Delete)
        {
            builder.AppendLine(
                $"            await _client.DeleteAsync(\"{route}\");");
        }
        else
        {
            builder.AppendLine(
                "            null!;");
        }

        builder.AppendLine();

        builder.AppendLine(
            "        // Assert");

        builder.AppendLine(
            "        Assert.NotNull(response);");

        builder.AppendLine(
            $"        Assert.Equal(");

        foreach (var keyword in scenario.ExpectedErrorKeywords)
        {
            builder.AppendLine(
                $"        Assert.Contains(");

            builder.AppendLine(
                $"            \"{keyword}\",");

            builder.AppendLine(
                "            responseContent,");

            builder.AppendLine(
                "            StringComparison.OrdinalIgnoreCase);");

            builder.AppendLine();
        }

        builder.AppendLine(
            $"            HttpStatusCode.{scenario.ExpectedStatusCode},");

        builder.AppendLine(
            "            response.StatusCode);");

        builder.AppendLine(
            "    }");

        builder.AppendLine();
    }
}