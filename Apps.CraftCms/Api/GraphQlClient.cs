using System.Text;
using Apps.CraftCms.Models.Dtos;
using Apps.CraftCms.Models.Dtos.Abstract;
using Apps.CraftCms.Utils;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Exceptions;
using RestSharp;
using Blackbird.Applications.Sdk.Utils.RestSharp;
using Newtonsoft.Json;

namespace Apps.CraftCms.Api;

public class GraphQlClient(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders)
    : BlackBirdRestClient(new()
    {
        BaseUrl = authenticationCredentialsProviders.GetUri(),
        ThrowOnAnyError = false
    })
{
    public override async Task<T> ExecuteWithErrorHandling<T>(RestRequest request)
    {
        var dataResponse = await base.ExecuteWithErrorHandling<DataWrapper<T>>(request);
        if (dataResponse.Errors.Count > 0)
        {
            var stringBuilder = new StringBuilder();
            for (int i = 0; i < dataResponse.Errors.Count; i++)
            {
                stringBuilder.Append($"Error #{i}: {dataResponse.Errors[i].ToString()}");
            }

            if (dataResponse.Errors.Any(x => x.Extensions?.Category == "graphql"))
            {
                throw new(stringBuilder.ToString());
            }
            
            throw new PluginApplicationException(stringBuilder.ToString());
        }
        
        return dataResponse.Data;
    }

    protected override Exception ConfigureErrorException(RestResponse response)
    {
        try
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
        catch (Exception e)
        {
            throw new(response.Content, e);
        }
    }
}