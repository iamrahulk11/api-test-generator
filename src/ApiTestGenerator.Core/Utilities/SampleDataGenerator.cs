using ApiTestGenerator.Core.Models;

namespace ApiTestGenerator.Core.Utilities;

public class SampleDataGenerator
{
    public string GenerateValue(
        ApiParameterDefinition parameter)
    {
        return parameter.Type.ToLower() switch
        {
            "string" => "\"sample-value\"",

            "integer" => "123",

            "number" => "99.5",

            "boolean" => "true",

            _ => "\"unknown\""
        };
    }
}