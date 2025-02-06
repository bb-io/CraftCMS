using Blackbird.Applications.Sdk.Common;

namespace Apps.CraftCms.Models.Responses.Entries;

public class EntryResponse
{
    [Display("Entry ID")]
    public string Id { get; set; } = string.Empty;

    public string Title { get; set; } = string.Empty;

    [Display("Is draft")]
    public bool IsDraft { get; set; }

    [Display("Is unpublished draft")]
    public bool IsUnpublishedDraft { get; set; }

    public bool Enabled { get; set; }

    [Display("Created at")]
    public DateTime DateCreated { get; set; }

    [Display("Updated at")]
    public DateTime DateUpdated { get; set; }

    public string Language { get; set; } = string.Empty;

    [DefinitionIgnore]
    public string TypeHandle { get; set; } = string.Empty;
    
    [DefinitionIgnore]
    public string SectionHandle { get; set; } = string.Empty;
}