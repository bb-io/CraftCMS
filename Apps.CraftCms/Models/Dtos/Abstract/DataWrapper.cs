namespace Apps.CraftCms.Models.Dtos.Abstract;

public class DataWrapper<T>
{
    public T Data { get; set; } = default!;

    public List<ErrorDto> Errors { get; } = new();
}