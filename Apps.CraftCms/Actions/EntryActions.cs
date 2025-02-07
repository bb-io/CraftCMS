using Apps.CraftCms.Api;
using Apps.CraftCms.Constants;
using Apps.CraftCms.Invocables;
using Apps.CraftCms.Models.Dtos;
using Apps.CraftCms.Models.Identifiers;
using Apps.CraftCms.Models.Requests;
using Apps.CraftCms.Models.Responses.Entries;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Exceptions;
using Blackbird.Applications.Sdk.Common.Invocation;
using Newtonsoft.Json.Linq;

namespace Apps.CraftCms.Actions;

[ActionList]
public class EntryActions(InvocationContext invocationContext) : AppInvocable(invocationContext)
{
    [Action("Search entries", Description = "Returns list of entries that satisfied input criteria")]
    public async Task<SearchEntriesResponse> SearchEntriesAsync([ActionParameter] SearchEntriesRequest request)
    {
        var parameters = request.BuildGraphQlParameters();
        var query = GraphQlQueries.GetSearchEntriesQuery(parameters);
        var graphQlRequest = new GraphQlRequest(query, Credentials);

        var entries = await Client.ExecuteWithErrorHandling<SearchEntriesResponse>(graphQlRequest);
        return entries;
    }

    [Action("Get entry", Description = "Get entry by ID")]
    public async Task<EntryResponse> GetEntryAsync([ActionParameter] EntryIdentifier request)
    {
        var query = GraphQlQueries.GetEntryById;
        var graphQlRequest = new GraphQlRequest(query, request.GetGraphQlVariables(), Credentials);

        var entryWrapper = await Client.ExecuteWithErrorHandling<EntryWrapperDto>(graphQlRequest);
        if (entryWrapper.Entry == null)
        {
            throw new PluginApplicationException(
                $"Couldn't find entry with ID: {request.EntryId}. Please ensure that ID is valid");
        }

        return entryWrapper.Entry;
    }

    [Action("Create entry", Description = "Creates a new entry with specified parameters")]
    public async Task<EntryResponse> CreateEntryAsync([ActionParameter] CreateEntryRequest request)
    {
        var entries = await SearchEntriesAsync(new() { Limit = 1 });
        var firstEntry = entries.Items.FirstOrDefault();

        if (firstEntry == null && (request.TypeHandle == null || request.SectionHandle == null))
        {
            throw new PluginMisconfigurationException("Since you don't have any entries, please fill in 'Entry type' and 'Section' optional inputs");
        }
        
        var mutationName = $"save_{request.SectionHandle ?? firstEntry!.SectionHandle}_{request.TypeHandle ?? firstEntry!.TypeHandle}_Entry";
        var query = GraphQlMutations.GetCreateEntryMutation(mutationName);

        var variables = new Dictionary<string, object>
        {
            { "title", request.Title }
        };

        // Add the optional slug only if provided.
        if (!string.IsNullOrWhiteSpace(request.Slug))
        {
            variables.Add("slug", request.Slug);
        }

        // Try to convert AuthorId to a number; otherwise pass the string.
        if (int.TryParse(request.AuthorId, out var authorId))
        {
            variables.Add("authorId", authorId);
        }
        else
        {
            variables.Add("authorId", request.AuthorId);
        }

        var graphQlRequest = new GraphQlRequest(query, variables, Credentials);
        var response = await Client.ExecuteWithErrorHandling<JObject>(graphQlRequest);
        var entryResponse = response[mutationName]?.ToObject<EntryResponse>()
                            ?? throw new PluginApplicationException("Entry creation failed. Please check input parameters.");

        return entryResponse;
    }

    [Action("Update entry", Description = "Updates an existing entry with specified parameters")]
    public async Task<EntryResponse> UpdateEntryAsync([ActionParameter] UpdateEntryRequest request)
    {
        var entry = await GetEntryAsync(new EntryIdentifier { EntryId = request.EntryId });
        var mutationName = $"save_{entry.SectionHandle}_{entry.TypeHandle}_Entry";

        var variables = new Dictionary<string, object>();
        var mutationVariables = new List<string>();
        var mutationParameters = new List<string>();

        if (int.TryParse(request.EntryId, out var entryId))
        {
            variables.Add("id", entryId);
        }
        else
        {
            variables.Add("id", request.EntryId);
        }

        mutationVariables.Add("$id: ID");
        mutationParameters.Add("id: $id");

        variables.Add("title", request.Title);
        mutationVariables.Add("$title: String");
        mutationParameters.Add("title: $title");

        if (!string.IsNullOrWhiteSpace(request.Slug))
        {
            variables.Add("slug", request.Slug);
            mutationVariables.Add("$slug: String");
            mutationParameters.Add("slug: $slug");
        }

        if (!string.IsNullOrWhiteSpace(request.SiteId))
        {
            if (int.TryParse(request.SiteId, out var siteId))
            {
                variables.Add("siteId", siteId);
                mutationVariables.Add("$siteId: Int");
                mutationParameters.Add("siteId: $siteId");
            }
            else
            {
                throw new PluginApplicationException("Invalid Site ID provided. It should be a numeric value.");
            }
        }

        var mutationVariablesString =
            mutationVariables.Any() ? $"({string.Join(", ", mutationVariables)})" : string.Empty;
        var mutationParametersString =
            mutationParameters.Any() ? $"({string.Join(", ", mutationParameters)})" : string.Empty;

        var mutation =
            GraphQlMutations.GetUpdateEntryMutation(mutationName, mutationVariablesString, mutationParametersString);

        var graphQlRequest = new GraphQlRequest(mutation, variables, Credentials);
        var response = await Client.ExecuteWithErrorHandling<JObject>(graphQlRequest);

        var entryResponse = response[mutationName]?.ToObject<EntryResponse>()
                            ?? throw new PluginApplicationException(
                                $"Couldn't update entry with ID: {request.EntryId}. Please ensure the provided data is valid.");

        return entryResponse;
    }

    [Action("Delete entry", Description = "Delete entry by ID")]
    public async Task DeleteEntryAsync([ActionParameter] EntryIdentifier request)
    {
        var query = GraphQlQueries.DeleteEntryById;
        var graphQlRequest = new GraphQlRequest(query, request.GetGraphQlVariables(), Credentials);

        var deleteEntryDto = await Client.ExecuteWithErrorHandling<DeleteEntryDto>(graphQlRequest);
        if (deleteEntryDto.DeleteEntry == false)
        {
            throw new PluginApplicationException(
                $"Couldn't delete entry with ID: {request.EntryId}. Please ensure that ID is valid");
        }
    }
}