using Blazor.CommandLine.Command;
using Microsoft.Extensions.DependencyInjection;

namespace Blazor.CommandLine;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCommandLine(this IServiceCollection serviceCollection,Action<CommandLineOptions> options=null)
    {

        serviceCollection.AddTransient<IRunningCommand, RunningCommand>();
        var consoleOptions = new CommandLineOptions();

        if (options != null)
        {
            options.Invoke(consoleOptions);
        }

        return serviceCollection;
    }

    public class CommandLineOptions
    {
        public bool UseDefaultServices { get; set; } = true;
    }
}