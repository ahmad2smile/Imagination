using System.Diagnostics;
using ImageMagick;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using OpenTelemetry.Context.Propagation;

namespace Imagination;

internal static class Program
{
    internal static readonly ActivitySource Telemetry = new ("Server");

    private static void Main(string[] args)
    {
        OpenTelemetry.Sdk.SetDefaultTextMapPropagator(new B3Propagator());
        CreateHostBuilder(args).Build().Run();

        // NOTE: Pre-Initializing as else it loads + initializes native MagickImage library on first req
        MagickNET.Initialize();
    }

    private static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
}