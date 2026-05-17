using ApiTestGenerator.Core.Models;
using ApiTestGenerator.Core.Testing;

namespace ApiTestGenerator.Core.Generators;

public class TestScenarioGenerator
{
    public List<TestScenario> Generate(
        ApiEndpointDefinition endpoint)
    {
        var scenarios =
            new List<TestScenario>();

        GenerateValidScenario(
            endpoint,
            scenarios);

        GenerateRequiredFieldScenarios(
            endpoint,
            scenarios);

        GenerateIntegerValidationScenarios(
            endpoint,
            scenarios);

        GenerateStringEdgeCases(
            endpoint,
            scenarios);

        GenerateNullScenarios(
            endpoint,
            scenarios);

        return scenarios;
    }

    private void GenerateValidScenario(
        ApiEndpointDefinition endpoint,
        List<TestScenario> scenarios)
    {
        scenarios.Add(
            new TestScenario
            {
                Name = "Valid Request",

                Description =
                $"Should successfully call {endpoint.Route}",

                ExpectedStatusCode =
                endpoint.Verb ==
                Enums.HttpVerb.Post
                    ? System.Net.HttpStatusCode.Created
                    : System.Net.HttpStatusCode.OK
            });
    }

    private void GenerateRequiredFieldScenarios(
    ApiEndpointDefinition endpoint,
    List<TestScenario> scenarios)
    {
        var requiredParameters =
            endpoint.Parameters
                .Where(x => x.Required);

        foreach (var parameter in requiredParameters)
        {
            scenarios.Add(
                new TestScenario
                {
                    Name =
                        $"Missing_{parameter.Name}",

                    Description =
                        $"Should fail when '{parameter.Name}' is missing",

                    ExpectedStatusCode =
                        System.Net.HttpStatusCode.BadRequest,

                    ExpectedErrorKeywords =
                    [
                        parameter.Name,
                        "required"
                    ],

                    Overrides =
                    {
                    [parameter.Name] = "null"
                    }
                });
        }
    }

    private void GenerateIntegerValidationScenarios(
        ApiEndpointDefinition endpoint,
        List<TestScenario> scenarios)
    {
        var integerParameters =
            endpoint.Parameters
                .Where(x =>
                    x.Type.Equals(
                        "integer",
                        StringComparison.OrdinalIgnoreCase));

        foreach (var parameter in integerParameters)
        {
            scenarios.Add(
                new TestScenario
                {
                    Name =
                        $"Invalid {parameter.Name}",

                    Description =
                        $"Should fail when '{parameter.Name}' is invalid",

                    ExpectedStatusCode = System.Net.HttpStatusCode.BadRequest
                });
        }
    }

    private void GenerateStringEdgeCases(
    ApiEndpointDefinition endpoint,
    List<TestScenario> scenarios)
    {
        var stringParameters =
            endpoint.Parameters
                .Where(x =>
                    x.Type.Equals(
                        "string",
                        StringComparison.OrdinalIgnoreCase));

        foreach (var parameter in stringParameters)
        {
            scenarios.Add(
                new TestScenario
                {
                    Name =
                        $"Empty {parameter.Name}",

                    Description =
                        $"Should fail when '{parameter.Name}' is empty",

                    ExpectedErrorKeywords =
                    [
                        parameter.Name,
                        "empty"
                    ],

                    Overrides =
                    {
                    [parameter.Name] = "\"\""
                    },

                    ExpectedStatusCode = System.Net.HttpStatusCode.BadRequest
                });

            scenarios.Add(
                new TestScenario
                {
                    Name =
                        $"Large {parameter.Name}",

                    Description =
                        $"Should handle large '{parameter.Name}' values",

                    ExpectedErrorKeywords =
                    [
                        parameter.Name,
                        "length"
                    ],

                    Overrides =
                    {
                    [parameter.Name] =
                        $"\"{new string('A', 500)}\""
                    },

                    ExpectedStatusCode = System.Net.HttpStatusCode.BadRequest
                });
        }
    }

    private void GenerateNullScenarios(
    ApiEndpointDefinition endpoint,
    List<TestScenario> scenarios)
    {
        var requiredParameters =
            endpoint.Parameters
                .Where(x => x.Required);

        foreach (var parameter in requiredParameters)
        {
            scenarios.Add(
                new TestScenario
                {
                    Name =
                        $"Null {parameter.Name}",

                    Description =
                        $"Should fail when '{parameter.Name}' is null",

                    Overrides =
                    {
                    [parameter.Name] = "null"
                    },

                    ExpectedStatusCode = System.Net.HttpStatusCode.BadRequest
                });
        }
    }
}