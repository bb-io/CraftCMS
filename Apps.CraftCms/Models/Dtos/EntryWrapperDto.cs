using Apps.CraftCms.Models.Responses.Entries;

namespace Apps.CraftCms.Models.Dtos;

public class EntryWrapperDto
{
    public EntryResponse Entry { get; set; } = new();
}