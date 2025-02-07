using Apps.CraftCms.DataHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.CraftCms.Models.Identifiers;

public class EntryIdentifier : LanguageIdentifier
{
    [Display("Entry ID"), DataSource(typeof(EntryDataHandler))]
    public string EntryId { get; set; } = string.Empty;

    public Dictionary<string, object> GetGraphQlVariables()
    {
        var parameters = new Dictionary<string, object>
        {
            { "id", int.Parse(EntryId) }
        };

        if (!string.IsNullOrEmpty(Language))
        {
            parameters.Add("language", Language);
        }

        return parameters;
    }
}