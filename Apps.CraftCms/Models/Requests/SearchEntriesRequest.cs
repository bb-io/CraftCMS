using Apps.CraftCms.DataHandlers.Static;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dictionaries;

namespace Apps.CraftCms.Models.Requests;

public class SearchEntriesRequest
{
    [StaticDataSource(typeof(LanguageDataHandler))]
    public string? Language { get; set; }

    public int? Limit { get; set; }

    [Display("Created from")]
    public DateTime? CreatedAfter { get; set; }

    public string BuildGraphQlParameters()
    {
        var filters = new List<string>();

        if (!string.IsNullOrEmpty(Language))
        {
            filters.Add($"language: \"{Language}\"");
        }

        if (Limit.HasValue)
        {
            filters.Add($"limit: {Limit.Value}");
        }

        if (CreatedAfter.HasValue)
        {
            filters.Add($"after: \"{CreatedAfter.Value.ToUniversalTime():o}\"");
        }

        return filters.Count > 0
            ? $"({string.Join(", ", filters)})"
            : string.Empty;
    }
}