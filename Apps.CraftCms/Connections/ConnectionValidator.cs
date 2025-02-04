using Apps.CraftCms.Api;
using Apps.CraftCms.Constants;
using Apps.CraftCms.Models.Dtos;
using Apps.CraftCms.Models.Dtos.Abstract;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Connections;
using RestSharp;

namespace Apps.CraftCms.Connections;

public class ConnectionValidator : IConnectionValidator
{
    public async ValueTask<ConnectionValidationResponse> ValidateConnection(
        IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
        CancellationToken cancellationToken)
    {
        var credentials = authenticationCredentialsProviders as AuthenticationCredentialsProvider[] ?? authenticationCredentialsProviders.ToArray();
        
        var client = new ApiClient(credentials);
        var request = new ApiRequest("/api", Method.Post, credentials)
            .AddBody(GraphQlQueries.Ping);
        
        var response = await client.ExecuteAsync<DataWrapper<StringQueryDto>>(request, cancellationToken);
        return new()
        {
            IsValid = response.IsSuccessful,
            Message = response.Data?.Data?.Query ?? response.Content ?? response.ErrorMessage
        };
    }
}