using Apps.CraftCms.Connections;
using Apps.CraftCms.Constants;
using Blackbird.Applications.Sdk.Common.Authentication;
using Tests.CraftCms.Base;

namespace Tests.CraftCms;

[TestClass]
public class ConnectionValidatorTests : TestBase
{
    [TestMethod]
    public async Task ValidatesCorrectConnection()
    {
        var validator = new ConnectionValidator();

        var result = await validator.ValidateConnection(Credentials, CancellationToken.None);

        Assert.IsTrue(result.IsValid);
        Console.WriteLine(result.Message);
    }

    [TestMethod]
    public async Task DoesNotValidateIncorrectConnection()
    {
        var validator = new ConnectionValidator();

        var newCredentials = Credentials
            .Select(x => x.KeyName == CredNames.BaseUrl
                ? new AuthenticationCredentialsProvider(x.KeyName, x.Value)
                : new AuthenticationCredentialsProvider(x.KeyName, x.Value + "_incorrect"))
            .ToList();

        var result = await validator.ValidateConnection(newCredentials, CancellationToken.None);
        Assert.IsFalse(result.IsValid);
        Console.WriteLine(result.Message);
    }
}