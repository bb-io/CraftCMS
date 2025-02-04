namespace Apps.CraftCms.Models.Dtos;

public class ErrorDto
{
    public string Name { get; set; } = string.Empty;
    
    public string Message { get; set; } = string.Empty;
    
    public int Code { get; set; } 
    
    public int Status { get; set; } 
    
    public string Type { get; set; } = string.Empty;

    public override string ToString()
    {
        return $"Name: {Name}; Message: {Message}; Status: {Status}; Type: {Type}";
    }
}