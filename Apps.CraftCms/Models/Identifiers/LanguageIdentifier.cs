using Apps.CraftCms.DataHandlers.Static;
using Blackbird.Applications.Sdk.Common.Dictionaries;

namespace Apps.CraftCms.Models.Identifiers;

public class LanguageIdentifier
{
    [StaticDataSource(typeof(LanguageDataHandler))]
    public string? Language { get; set; }
}