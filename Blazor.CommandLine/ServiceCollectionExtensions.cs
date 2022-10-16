using System.CommandLine;
using System.Reflection;
using Blazor.CommandLine.Command;
using HarmonyLib;
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

        PerformHarmonyPatch();
        
        return serviceCollection;
    }

    private static void PerformHarmonyPatch()
    {
        var harmony = new Harmony("com.blazor.console.resetcolorpatch");

        var mOriginal = typeof(ConsoleExtensions).Assembly.GetType("System.CommandLine.IO.ConsoleExtensions").GetMethod("ResetTerminalForegroundColor", BindingFlags.Static | BindingFlags.NonPublic);
        var mPrefix = typeof(UnholyPatch).GetMethod("ResetTerminalForegroundColor", BindingFlags.Static | BindingFlags.NonPublic);
        
        // add null checks here

        harmony.Patch(mOriginal, new HarmonyMethod(mPrefix));
    }

    private static class UnholyPatch
    {
        private static void ResetTerminalForegroundColor(IConsole console, ref bool __result)
        {
            __result = false;
        }
    }

    public class CommandLineOptions
    {
        public bool UseDefaultServices { get; set; } = true;
    }
}