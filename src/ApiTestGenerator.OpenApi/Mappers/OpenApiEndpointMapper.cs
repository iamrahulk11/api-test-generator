using ApiTestGenerator.Core.Enums;
using ApiTestGenerator.Core.Models;
using Microsoft.OpenApi.Models;

namespace ApiTestGenerator.OpenApi.Mappers;

public class OpenApiEndpointMapper
{
    public List<ApiEndpointDefinition> Map(
        OpenApiDocument document)
    {
        var endpoints =
            new List<ApiEndpointDefinition>();

        foreach (var path in document.Paths)
        {
            foreach (var operation in path.Value.Operations)
            {
                var endpoint =
                    new ApiEndpointDefinition
                    {
                        Route = path.Key,
                        Verb = MapVerb(operation.Key)
                    };

                foreach (var parameter in operation.Value.Parameters)
                {
                    endpoint.Parameters.Add(
                        new ApiParameterDefinition
                        {
                            Name = parameter.Name,

                            Type =
                                parameter.Schema?.Type
                                ?? "unknown",

                            Required =
                                parameter.Required,

                            Source =
                                MapParameterSource(
                                    parameter.In ?? ParameterLocation.Query)
                        });
                }

                MapRequestBody(operation.Value, endpoint);
                endpoints.Add(endpoint);
            }
        }

        return endpoints;
    }

    private HttpVerb MapVerb(
        OperationType operationType)
    {
        return operationType switch
        {
            OperationType.Get => HttpVerb.Get,

            OperationType.Post => HttpVerb.Post,

            OperationType.Put => HttpVerb.Put,

            OperationType.Delete => HttpVerb.Delete,

            OperationType.Patch => HttpVerb.Patch,

            _ => throw new NotSupportedException(
                $"Unsupported HTTP verb: {operationType}")
        };
    }

    private ParameterSource MapParameterSource(
    ParameterLocation? parameterLocation)
    {
        return parameterLocation switch
        {
            ParameterLocation.Query
                => ParameterSource.Query,

            ParameterLocation.Path
                => ParameterSource.Route,

            ParameterLocation.Header
                => ParameterSource.Header,

            ParameterLocation.Cookie
                => ParameterSource.Header,

            _ => ParameterSource.Query
        };
    }
    private void MapRequestBody(
    OpenApiOperation operation,
    ApiEndpointDefinition endpoint)
    {
        var requestBody =
            operation.RequestBody;

        if (requestBody == null)
        {
            return;
        }

        if (!requestBody.Content.TryGetValue(
            "application/json",
            out var mediaType))
        {
            return;
        }

        var schema =
            mediaType.Schema;

        if (schema?.Properties == null)
        {
            return;
        }

        foreach (var property in schema.Properties)
        {
            endpoint.Parameters.Add(
                new ApiParameterDefinition
                {
                    Name = property.Key,

                    Type =
                        property.Value.Type
                        ?? "unknown",

                    Source =
                        ParameterSource.Body,

                    Required =
                        schema.Required.Contains(
                            property.Key)
                });
        }
    }
}