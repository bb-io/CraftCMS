using Apps.CraftCms.Constants;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Utils.Extensions.Sdk;

namespace Apps.CraftCms.Utils;

public static class CredentialsProviderExtensions
{
    public static Uri GetUri(this IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders)
    {
        var url = authenticationCredentialsProviders.Get(CredNames.BaseUrl);
        if (string.IsNullOrEmpty(url.Value))
        {
            throw new Exception("Can not find base url in AuthenticationCredentialsProvider collection");
        }

        return new Uri(url.Value);
    }
}