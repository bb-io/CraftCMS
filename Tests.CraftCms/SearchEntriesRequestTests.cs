using Apps.CraftCms.Models.Requests;

namespace Tests.CraftCms;

[TestClass]
public class SearchEntriesRequestTests
{
    [TestMethod]
    public void BuildGraphQlParameters_AllFieldsSet_ShouldReturnCorrectQuery()
    {
        var request = new SearchEntriesRequest
        {
            Language = "en",
            Limit = 10,
            CreatedAfter = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc)
        };

        var result = request.BuildGraphQlParameters();
        
        Assert.AreEqual("(language: \"en\", limit: 10, after: \"2023-01-01T00:00:00.0000000Z\")", result);
        Console.WriteLine(result);
    }

    [TestMethod]
    public void BuildGraphQlParameters_LanguageOnly_ShouldReturnOnlyLanguageFilter()
    {
        var request = new SearchEntriesRequest
        {
            Language = "fr"
        };

        var result = request.BuildGraphQlParameters();

        Assert.AreEqual("(language: \"fr\")", result);
        Console.WriteLine(result);
    }

    [TestMethod]
    public void BuildGraphQlParameters_NoFieldsSet_ShouldReturnEmptyString()
    {
        var request = new SearchEntriesRequest();

        var result = request.BuildGraphQlParameters();

        Assert.AreEqual(string.Empty, result);
        Console.WriteLine(result);
    }
}
