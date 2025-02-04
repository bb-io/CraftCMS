namespace Apps.CraftCms.Models.Dtos.Abstract;

public class DataWrapper<T> where T : class
{
    public T Data { get; set; } = default!;
}