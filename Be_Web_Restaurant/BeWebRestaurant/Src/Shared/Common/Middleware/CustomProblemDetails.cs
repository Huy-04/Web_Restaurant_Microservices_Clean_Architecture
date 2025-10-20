public class CustomProblemDetails
{
    public string? Title { get; set; }
    public int Status { get; set; }
    public string? Instance { get; set; }
    public string? ErrorCategory { get; set; }
    public List<CustomErrorDetail>? Errors { get; set; }
}