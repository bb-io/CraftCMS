using Apps.CraftCms.Actions;
using Apps.CraftCms.Models.Identifiers;
using Apps.CraftCms.Models.Requests;
using Blackbird.Applications.Sdk.Common.Exceptions;
using Newtonsoft.Json;
using Tests.CraftCms.Base;

namespace Tests.CraftCms;

[TestClass]
public class EntryActionsTests : TestBase
{
    [TestMethod]
    public async Task SearchEntries_WithoutParameters_ShouldReturnNonEmptyCollection()
    {
        // Arrange
        var actions = new EntryActions(InvocationContext);
        var request = new SearchEntriesRequest();

        // Act
        var result = await actions.SearchEntriesAsync(request);

        // Assert
        Assert.IsNotNull(result, "Response should not be null");
        Assert.IsNotNull(result.Items, "Entries should not be null");
        Assert.IsTrue(result.Items.Count > 0, "Entries collection should not be empty");

        Console.WriteLine(JsonConvert.SerializeObject(result));
    }
    
    [TestMethod]
    public async Task SearchEntries_SearchForDeLanguageEntries_ShouldReturnCollectionWithDeLanguage()
    {
        // Arrange
        var actions = new EntryActions(InvocationContext);        
        var request = new SearchEntriesRequest { Language = "de" };

        // Act
        var result = await actions.SearchEntriesAsync(request);

        // Assert
        Assert.IsNotNull(result, "Response should not be null");
        Assert.IsNotNull(result.Items, "Entries should not be null");
        Assert.IsTrue(result.Items.Count > 0, "Entries collection should not be empty");
        Assert.IsTrue(result.Items.TrueForAll(e => e.Language == "de"), "All entries should have language 'de'");

        Console.WriteLine(JsonConvert.SerializeObject(result));
    }
    
    [TestMethod]
    public async Task SearchEntries_FilterByCreatedAfter_ShouldReturnOnlyRecentEntries()
    {
        // Arrange
        var actions = new EntryActions(InvocationContext);
        var filterDate = new DateTime(2025, 2, 6);
        var request = new SearchEntriesRequest { CreatedAfter = filterDate };

        // Act
        var result = await actions.SearchEntriesAsync(request);

        // Assert
        Assert.IsNotNull(result, "Response should not be null");
        Assert.IsNotNull(result.Items, "Entries should not be null");
        Assert.IsTrue(result.Items.Count > 0, "Entries collection should not be empty");
        Assert.IsTrue(result.Items.TrueForAll(e => e.DateCreated > filterDate), "All entries should be created after the specified date");

        Console.WriteLine(JsonConvert.SerializeObject(result));
    }
    
    [TestMethod]
    public async Task GetEntry_ValidId_ShouldReturnEntry()
    {
        // Arrange
        var entryId = "2";
        var actions = new EntryActions(InvocationContext);
        var request = new EntryIdentifier { EntryId = entryId };

        // Act
        var result = await actions.GetEntryAsync(request);

        // Assert
        Assert.IsNotNull(result, "Response should not be null");
        Assert.IsNotNull(result.Id, "Entry ID should not be null");
        Assert.AreEqual(entryId, result.Id, "Returned entry ID should match the requested ID");

        Console.WriteLine(JsonConvert.SerializeObject(result));
    }
    
    [TestMethod]
    public async Task GetEntry_InvalidId_ShouldReturnEntry()
    {
        // Arrange
        var entryId = "999";
        var actions = new EntryActions(InvocationContext);
        var request = new EntryIdentifier { EntryId = entryId };

        // Act
        var getEntryTask = actions.GetEntryAsync(request);

        // Assert
        await Assert.ThrowsExceptionAsync<PluginApplicationException>(() => getEntryTask);
    }
}