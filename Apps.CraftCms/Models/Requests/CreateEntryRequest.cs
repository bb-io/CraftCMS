using Blackbird.Applications.Sdk.Common;

namespace Apps.CraftCms.Models.Requests;

public class CreateEntryRequest
{
    [Display("Author ID")]
    public string AuthorId { get; set; } = string.Empty;
    
    public string Title { get; set; } = string.Empty;

    public string? Slug { get; set; }

    [Display("Entry type")]
    public string? TypeHandle { get; set; }
    
    [Display("Section")]
    public string? SectionHandle { get; set; }
}