using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Readers;

namespace ApiTestGenerator.OpenApi.Readers;

public class SwaggerDocumentReader
{
    public OpenApiDocument Read(
        string swaggerFilePath)
    {
        using var stream =
            File.OpenRead(swaggerFilePath);

        var reader =
            new OpenApiStreamReader();

        var document =
            reader.Read(
                stream,
                out var diagnostic);

        if (diagnostic.Errors.Count > 0)
        {
            var errors =
                string.Join(
                    Environment.NewLine,
                    diagnostic.Errors);

            throw new Exception(
                $"Swagger parsing failed:{Environment.NewLine}{errors}");
        }

        return document;
    }
}