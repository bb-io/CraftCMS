using Apps.CraftCms.Constants;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Utils.Extensions.Sdk;

namespace Apps.CraftCms.Utils;

public static class CredentialsProviderExtensions
{
    public static Uri GetUri(this IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders)
    {
        var credentialsProvider = authenticationCredentialsProviders.Get(CredNames.BaseUrl);
        if (string.IsNullOrEmpty(credentialsProvider.Value))
        {
            throw new Exception("Can not find base url in AuthenticationCredentialsProvider collection");
        }

        var url = credentialsProvider.Value;
        if (url.EndsWith("/"))
        {
            url += "api";
        }
        else
        {
            url += "/api";
        }
        
        return new Uri(url);
    }
}