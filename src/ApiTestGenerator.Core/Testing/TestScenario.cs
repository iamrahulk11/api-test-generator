using System.Net;

namespace ApiTestGenerator.Core.Testing;

public class TestScenario
{
    public string Name { get; set; }
        = string.Empty;

    public string Description { get; set; }
        = string.Empty;

    public Dictionary<string, string> Overrides
    { get; set; }
    = new(StringComparer.OrdinalIgnoreCase);

    public HttpStatusCode ExpectedStatusCode
    { get; set; }

    public List<string> ExpectedErrorKeywords
    { get; set; } = [];
}