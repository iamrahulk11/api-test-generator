using ApiTestGenerator.Core.Enums;

namespace ApiTestGenerator.Core.Models;

public class ApiParameterDefinition
{
    public string Name { get; set; }
        = string.Empty;

    public string Type { get; set; }
        = string.Empty;

    public bool Required { get; set; }

    public ParameterSource Source { get; set; }
}