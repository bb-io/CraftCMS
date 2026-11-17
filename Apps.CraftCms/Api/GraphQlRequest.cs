using Apps.CraftCms.Constants;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Utils.Extensions.Sdk;
using Blackbird.Applications.Sdk.Utils.RestSharp;
using RestSharp;

namespace Apps.CraftCms.Api;

public class GraphQlRequest : BlackBirdRestRequest
{
    public GraphQlRequest(string query, IEnumerable<AuthenticationCredentialsProvider> creds) : base(string.Empty, Method.Post, creds)
    {
        this.AddJsonBody(new
        {
            query
        });
    }
    
    public GraphQlRequest(string query, object variables, IEnumerable<AuthenticationCredentialsProvider> creds) : base(string.Empty, Method.Post, creds)
    {
        this.AddJsonBody(new
        {
            query,
            variables
        });
    }
    
    protected override void AddAuth(IEnumerable<AuthenticationCredentialsProvider> creds)
    {
        var apiKey = creds.Get(CredNames.AccessToken).Value;
        this.AddHeader("Authorization", $"Bearer {apiKey}");
    }
}