using ApiTestGenerator.Core.Enums;

namespace ApiTestGenerator.Core.Models;

public class ApiEndpointDefinition
{
    public string Route { get; set; }
        = string.Empty;

    public HttpVerb Verb { get; set; }

    public List<ApiParameterDefinition> Parameters
        = new();
}