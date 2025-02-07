using Apps.CraftCms.DataHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.CraftCms.Models.Requests;

public class UpdateEntryRequest
{
    [Display("Entry ID"), DataSource(typeof(EntryDataHandler))]
    public string EntryId { get; set; } = string.Empty;
    
    public string Title { get; set; } = string.Empty;

    public string? Slug { get; set; }
    
    [Display("Site ID")]
    public string? SiteId { get; set; } = string.Empty;
}