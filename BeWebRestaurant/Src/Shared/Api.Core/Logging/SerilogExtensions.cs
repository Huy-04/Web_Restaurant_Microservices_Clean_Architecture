using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;

namespace Api.Core.Logging
{
    public static class SerilogExtensions
    {
        private const string DefaultTextTemplate =
            "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz}] [{Level:u3}] [{TraceId}] [{SourceContextShort}] [{Message:lj}]{NewLine}{Exception}";

        public static IHostBuilder UseSharedSerilog(
            this IHostBuilder host,
            IConfiguration config,
            Action<LoggerConfiguration>? extra = null)
        {
            var opt = config.GetSection("SerilogOptions").Get<SerilogOptions>() ?? new();

            return host.UseSerilog((ctx, lc) =>
            {
                lc.MinimumLevel.Debug()

                  // Microsoft logs
                  .MinimumLevel.Override("Microsoft.AspNetCore.Hosting.Diagnostics", LogEventLevel.Information)
                  .MinimumLevel.Override("Microsoft.AspNetCore.Server.Kestrel", LogEventLevel.Information)
                  .MinimumLevel.Override("Microsoft.EntityFrameworkCore.Database.Command", LogEventLevel.Warning)
                  .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)

                  // Hide license & middleware noise
                  .MinimumLevel.Override("MediatR", LogEventLevel.Error)
                  .MinimumLevel.Override("LuckyPennySoftware.MediatR.License", LogEventLevel.Error)
                  .MinimumLevel.Override("Common.Middleware.ExceptionMiddleware", LogEventLevel.Error)
                  .MinimumLevel.Override("Common.Behaviors.ValidationBehaviors", LogEventLevel.Information)

                  .Enrich.FromLogContext()
                  .Enrich.With(new ShortSourceContextEnricher())
                  .Enrich.WithActivityId()

                  .WriteTo.Console(outputTemplate: opt.TextTemplate ?? DefaultTextTemplate)

                  // Be
                  .WriteTo.File(
                      path: opt.BeTextPath,
                      rollingInterval: RollingInterval.Day,
                      retainedFileCountLimit: opt.RetainedFileCountLimit,
                      buffered: false,
                      flushToDiskInterval: TimeSpan.FromSeconds(1),
                      shared: true,
                      outputTemplate: opt.TextTemplate ?? DefaultTextTemplate)

                  // FE
                  .WriteTo.File(
                      formatter: new CompactJsonFormatter(),
                      path: opt.FeJsonPath,
                      rollingInterval: RollingInterval.Day,
                      retainedFileCountLimit: opt.RetainedFileCountLimit,
                      buffered: false,
                      flushToDiskInterval: TimeSpan.FromSeconds(1),
                      shared: true);

                extra?.Invoke(lc);
            });
        }
    }
}

