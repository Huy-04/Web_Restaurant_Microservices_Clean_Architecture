using Serilog.Core;
using Serilog.Events;

namespace Api.Core.Logging
{
    public sealed class ShortSourceContextEnricher : ILogEventEnricher
    {
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory pf)
        {
            if (logEvent.Properties.TryGetValue("SourceContext", out var p)
                && p is ScalarValue sv && sv.Value is string s)
            {
                var shortName = s.Split('.').Last();
                logEvent.AddOrUpdateProperty(pf.CreateProperty("SourceContextShort", shortName));
            }
        }
    }
}
