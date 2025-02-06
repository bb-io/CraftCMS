namespace Apps.CraftCms.Constants;

public class GraphQlQueries
{
    public const string Ping = "{ ping }";

    private const string SearchEntries = @"{
        entries {parameters} {
            id
            title
            language
            isDraft
            isUnpublishedDraft
            isRevision
            dateCreated
            dateUpdated
            enabled
            sectionHandle
        }
        entryCount
    }";
    
    
    public const string GetEntryById = @"query getEntry($id: [QueryArgument]) {
        entry(id: $id) {
            id
            title
            language
            isDraft
            isUnpublishedDraft
            isRevision
            dateCreated
            dateUpdated
            enabled
            sectionHandle
        }
    }";

    public static string GetSearchEntriesQuery(string? parameters = null)
    {
        parameters ??= string.Empty;
        return SearchEntries.Replace("{parameters}", parameters);
    }
}