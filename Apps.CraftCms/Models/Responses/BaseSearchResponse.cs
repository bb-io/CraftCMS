using Blackbird.Applications.Sdk.Common;

namespace Apps.CraftCms.Models.Responses;

public class BaseSearchResponse<T>(List<T> items)
{
    public virtual List<T> Items { get; set; } = items;

    [Display("Total count")]
    public virtual double TotalCount { get; set; } = items.Count;
}