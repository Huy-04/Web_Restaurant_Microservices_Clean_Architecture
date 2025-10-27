namespace Api.Core.Middleware
{
    public class CustomProblemDetails
    {
        public string? Type { get; set; }
        public string? Title { get; set; }
        public int Status { get; set; }
        public string? Detail { get; set; }
        public string? Instance { get; set; }
        public string? TraceId { get; set; }
        public string? ErrorCategory { get; set; }
        public List<CustomErrorDetail>? Errors { get; set; }
    }
}
