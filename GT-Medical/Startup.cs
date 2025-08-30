using GT_Medical.Helper.Extensions;
using GT_Medical.UI;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;

namespace GT_Medical;
public sealed class Startup
{
    public static void ConfigureServices(IServiceCollection services)
    {
        // Logging (uses Host defaults; you can add Serilog here if you want)
        services.AddLogging(builder =>
        {
            builder.Services.AddLogging();
        });

        Assembly[] assemblies = [Assembly.GetExecutingAssembly()];
        services.AddByConvention(assemblies);

        // Auto-register all forms (MainForm as singleton)
        services.AddFormsByConvention(Assembly.GetExecutingAssembly());
    }
    public static void RunApp()
    {
        using var host = Host
             .CreateDefaultBuilder()
             .ConfigureServices((ctx, services) =>
             {
                 ConfigureServices(services);
             }).Build();
        
        // Register global exception handler
        host.Services.GetRequiredService<GlobalExceptionHandler>().Register();
        // Run your main form from DI
        var mainForm = host.Services.GetRequiredService<FrmVideoPlayer>();
        Application.Run(mainForm);
    }
}

