namespace Api.Core.Logging
{
    public sealed class SerilogOptions
    {
        public string BeTextPath { get; set; } = "logs/be/menulog-be-.txt";
        public string FeJsonPath { get; set; } = "logs/fe/menulog-fe-.clef";
        public int? RetainedFileCountLimit { get; set; } = 30;
        public string? TextTemplate { get; set; }
    }
}


