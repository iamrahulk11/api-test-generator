using ApiTestGenerator.Core.CodeGenerators;
using ApiTestGenerator.Core.Generators;
using ApiTestGenerator.OpenApi.Mappers;
using ApiTestGenerator.OpenApi.Readers;

var solutionRoot =
    Path.GetFullPath(
        Path.Combine(
            AppContext.BaseDirectory,
            "..",
            "..",
            "..",
            "..",
            ".."));
var swaggerPath =
    Path.Combine(
        solutionRoot,
        "samples",
        "swagger.json");

var reader =
    new SwaggerDocumentReader();

var document =
    reader.Read(swaggerPath);

var mapper =
    new OpenApiEndpointMapper();

var endpoints =
    mapper.Map(document);

//foreach (var endpoint in endpoints)
//{
//    Console.WriteLine(
//        $"{endpoint.Verb} {endpoint.Route}");

//    foreach (var parameter in endpoint.Parameters)
//    {
//        Console.WriteLine(
//            $"  Param: {parameter.Name}");

//        Console.WriteLine(
//            $"    Type: {parameter.Type}");

//        Console.WriteLine(
//            $"    Source: {parameter.Source}");

//        Console.WriteLine(
//            $"    Required: {parameter.Required}");
//    }

//    Console.WriteLine();
//}

//var generator =
//    new TestScenarioGenerator();

//foreach (var endpoint in endpoints)
//{
//    Console.WriteLine(
//        $"{endpoint.Verb} {endpoint.Route}");

//    var scenarios =
//        generator.Generate(endpoint);

//    foreach (var scenario in scenarios)
//    {
//        Console.WriteLine(
//            $"  - {scenario.Name}");

//        Console.WriteLine(
//            $"    {scenario.Description}");
//    }

//    Console.WriteLine();
//}


var scenarioGenerator =
    new TestScenarioGenerator();

var xunitGenerator =
    new XUnitTestGenerator();

var outputDirectory =
    Path.Combine(
        solutionRoot,
        "GeneratedTests");

Directory.CreateDirectory(
    outputDirectory);

foreach (var endpoint in endpoints)
{
    var scenarios =
        scenarioGenerator.Generate(
            endpoint);

    var code =
        xunitGenerator.Generate(
            endpoint,
            scenarios);

    //Console.WriteLine(code);

    //Console.WriteLine(
    //    "====================================");

    var fileName =
    $"{endpoint.Verb}_{endpoint.Route
        .Replace("/", "_")
        .Replace("{", "")
        .Replace("}", "")}.cs";

    var filePath =
        Path.Combine(
            outputDirectory,
            fileName);

    File.WriteAllText(
        filePath,
        code);

    Console.WriteLine(
        $"Generated: {fileName}");
}