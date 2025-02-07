namespace Apps.CraftCms.Constants;

public class GraphQlMutations
{
    private const string CreateEntry = @"
    mutation saveEntry($title: String, $slug: String, $authorId: ID) {
        {mutation_name} (title: $title, slug: $slug, authorId: $authorId) {
            id
            title
            isDraft
            isUnpublishedDraft
            isRevision
            dateCreated
            dateUpdated
            enabled
            sectionHandle
            language
            typeHandle
        }
    }";

    private const string UpdateEntry = @"
    mutation saveEntry {mutation_variables} {
        {mutation_name} {mutation_parameters} {
            id
            title
            isDraft
            isUnpublishedDraft
            isRevision
            dateCreated
            dateUpdated
            enabled
            sectionHandle
            language
            typeHandle
        }
    }";

    public static string GetUpdateEntryMutation(string mutationName, string mutationVariables,
        string mutationParameters)
    {
        return UpdateEntry.Replace("{mutation_variables}", mutationVariables)
            .Replace("{mutation_name}", mutationName)
            .Replace("{mutation_parameters}", mutationParameters);
    }
    
    public static string GetCreateEntryMutation(string mutationName)
    {
        return CreateEntry.Replace("{mutation_name}", mutationName);
    }
}
