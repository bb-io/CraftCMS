using Blackbird.Applications.Sdk.Common;
using Newtonsoft.Json;

namespace Apps.CraftCms.Models.Responses.Entries;

public class SearchEntriesResponse(List<EntryResponse> items) : BaseSearchResponse<EntryResponse>(items)
{
    [Display("Entries"), JsonProperty("entries")]
    public override List<EntryResponse> Items { get; set; } = items;

    [Display("Entry count"), JsonProperty("entryCount")]
    public override double TotalCount { get; set; }
}