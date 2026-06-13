using Serilog;

namespace Decoupler.WebApi.Common.Logging;

public static class LoggingExtensions
{
    extension(WebApplicationBuilder webApplicationBuilder)
    {
        public void ConfigureSerilog()
        {
            webApplicationBuilder.Host.UseSerilog((_, _, configuration) =>
            {
                configuration
                    .Enrich.FromLogContext()
                    .WriteTo.Console();
            });
        }
    }
}