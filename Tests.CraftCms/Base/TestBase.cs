using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Invocation;
using Microsoft.Extensions.Configuration;

namespace Tests.CraftCms.Base;

public class TestBase
{
    protected TestBase()
    {
        var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
        Credentials = config.GetSection("ConnectionDefinition").GetChildren()
            .Select(x => new AuthenticationCredentialsProvider(x.Key, x.Value!)).ToList();
        var folderLocation = config.GetSection("TestFolder").Value!;

        InvocationContext = new InvocationContext
        {
            AuthenticationCredentialsProviders = Credentials,
        };

        FileManager = new FileManager(folderLocation);
    }

    protected IEnumerable<AuthenticationCredentialsProvider> Credentials { get; set; }

    public InvocationContext InvocationContext { get; set; }

    public FileManager FileManager { get; set; }
}