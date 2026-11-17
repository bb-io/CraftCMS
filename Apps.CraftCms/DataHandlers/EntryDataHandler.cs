using Apps.CraftCms.Api;
using Apps.CraftCms.Constants;
using Apps.CraftCms.Invocables;
using Apps.CraftCms.Models.Identifiers;
using Apps.CraftCms.Models.Requests;
using Apps.CraftCms.Models.Responses.Entries;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Invocation;

namespace Apps.CraftCms.DataHandlers;

public class EntryDataHandler(InvocationContext invocationContext, [ActionParameter] LanguageIdentifier languageIdentifier)
    : AppInvocable(invocationContext), IAsyncDataSourceItemHandler
{
    public async Task<IEnumerable<DataSourceItem>> GetDataAsync(DataSourceContext context, CancellationToken cancellationToken)
    {
        var searchEntriesRequest = new SearchEntriesRequest();

        if (!string.IsNullOrEmpty(languageIdentifier.Language))
        {
            searchEntriesRequest.Language = languageIdentifier.Language;
        }
        
        var query = GraphQlQueries.GetSearchEntriesQuery(searchEntriesRequest.BuildGraphQlParameters());
        var graphQlRequest = new GraphQlRequest(query, Credentials);
        
        var entries = await Client.ExecuteWithErrorHandling<SearchEntriesResponse>(graphQlRequest);
        return entries.Items.Select(x => new DataSourceItem(x.Id, x.Title));
    }
}

