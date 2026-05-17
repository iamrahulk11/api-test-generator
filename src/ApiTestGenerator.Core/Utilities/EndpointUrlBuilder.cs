using System.Text;
using ApiTestGenerator.Core.Enums;
using ApiTestGenerator.Core.Models;

namespace ApiTestGenerator.Core.Utilities;

public class EndpointUrlBuilder
{
    private readonly SampleDataGenerator _sampleDataGenerator
        = new();

    public string Build(
        ApiEndpointDefinition endpoint)
    {
        var route =
            endpoint.Route;

        route =
            ReplaceRouteParameters(
                route,
                endpoint);

        route =
            AppendQueryParameters(
                route,
                endpoint);

        return route;
    }

    private string ReplaceRouteParameters(
        string route,
        ApiEndpointDefinition endpoint)
    {
        var routeParameters =
            endpoint.Parameters
                .Where(x =>
                    x.Source ==
                    ParameterSource.Route);

        foreach (var parameter in routeParameters)
        {
            var value =
                _sampleDataGenerator.GenerateValue(
                    parameter);

            value =
                value.Replace("\"", "");

            route =
                route.Replace(
                    $"{{{parameter.Name}}}",
                    value);
        }

        return route;
    }

    private string AppendQueryParameters(
        string route,
        ApiEndpointDefinition endpoint)
    {
        var queryParameters =
            endpoint.Parameters
                .Where(x =>
                    x.Source ==
                    ParameterSource.Query)
                .ToList();

        if (!queryParameters.Any())
        {
            return route;
        }

        var builder =
            new StringBuilder();

        builder.Append(route);

        builder.Append("?");

        for (int i = 0; i < queryParameters.Count; i++)
        {
            var parameter =
                queryParameters[i];

            var value =
                _sampleDataGenerator.GenerateValue(
                    parameter);

            value =
                value.Replace("\"", "");

            builder.Append(
                $"{parameter.Name}={value}");

            if (i < queryParameters.Count - 1)
            {
                builder.Append("&");
            }
        }

        return builder.ToString();
    }
}