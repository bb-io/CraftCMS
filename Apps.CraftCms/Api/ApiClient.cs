using Apps.CraftCms.Models.Dtos;
using Apps.CraftCms.Utils;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Exceptions;
using RestSharp;
using Blackbird.Applications.Sdk.Utils.RestSharp;
using Newtonsoft.Json;

namespace Apps.CraftCms.Api;

public class ApiClient(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders)
    : BlackBirdRestClient(new()
    {
        BaseUrl = authenticationCredentialsProviders.GetUri(),
        ThrowOnAnyError = false
    })
{
    protected override Exception ConfigureErrorException(RestResponse response)
    {
        if (string.IsNullOrEmpty(response.Content))
        {
            if (!string.IsNullOrEmpty(response.ErrorMessage))
            {
                return new PluginApplicationException(response.ErrorMessage);
            }

            throw new PluginApplicationException($"Unknown error occured with {response.StatusCode} status code");
        }
        
        var error = JsonConvert.DeserializeObject<ErrorDto>(response.Content!)!;
        return new PluginApplicationException(error.ToString());
    }
}