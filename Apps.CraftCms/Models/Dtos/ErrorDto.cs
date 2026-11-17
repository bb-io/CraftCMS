using Newtonsoft.Json;

namespace Apps.CraftCms.Models.Dtos;

public class ErrorDto
{
    public string? Name { get; set; } = string.Empty;

    public string Message { get; set; } = string.Empty;

    public int? Code { get; set; }

    public int? Status { get; set; }

    public string? Type { get; set; } = string.Empty;

    public ExtensionDto? Extensions { get; set; }

    public override string ToString()
    {
        var properties = new List<string>();

        if (!string.IsNullOrEmpty(Name))
        {
            properties.Add($"Name: {Name}");
        }

        if (!string.IsNullOrEmpty(Message))
        {
            properties.Add($"Message: {Message}");
        }

        if (Code.HasValue)
        {
            properties.Add($"Code: {Code.Value}");
        }

        if (Status.HasValue)
        {
            properties.Add($"Status: {Status.Value}");
        }

        if (!string.IsNullOrEmpty(Type))
        {
            properties.Add($"Type: {Type}");
        }

        if (Extensions != null)
        {
            properties.Add($"Category: {Extensions.Category}");
        }

        return string.Join("; ", properties);
    }
}