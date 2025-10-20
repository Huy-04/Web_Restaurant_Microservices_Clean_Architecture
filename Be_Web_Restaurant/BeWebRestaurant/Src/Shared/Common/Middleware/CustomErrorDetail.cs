using System.Text.Json.Serialization;

public class CustomErrorDetail
{
    public string? Field { get; set; }
    public string? ErrorCode { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Dictionary<string, object>? Parameter { get; set; }
}