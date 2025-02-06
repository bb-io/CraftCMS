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
            throw new PluginApplicationException($"Couldn't find entry with ID: {request.EntryId}. Please ensure that ID is valid");
        }
        
        return entryWrapper.Entry;
    }
    
    [Action("Delete entry", Description = "Delete entry by ID")]
    public async Task DeleteEntryAsync([ActionParameter] EntryIdentifier request)
    {
        var query = GraphQlQueries.DeleteEntryById;
        var graphQlRequest = new GraphQlRequest(query, request.GetGraphQlVariables(), Credentials);
        
        var deleteEntryDto = await Client.ExecuteWithErrorHandling<DeleteEntryDto>(graphQlRequest);
        if (deleteEntryDto.DeleteEntry == false)
        {
            throw new PluginApplicationException($"Couldn't delete entry with ID: {request.EntryId}. Please ensure that ID is valid");
        }
    }
}