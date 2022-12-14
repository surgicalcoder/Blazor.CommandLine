using Blazor.CommandLine.Console;

namespace Blazor.CommandLine.Command;

public class OSCommand : BaseCommand
{
    public OSCommand() : base("os", "Displays the current opearting system.")
    {

    }
    public override bool Execute(DefaultStreamWriter console,string optionArgument1, string optionArgument2, string optionArgument3, string optionArgument4, List<string> arguments)
    {
        try
        {
            console.Write(System.Runtime.InteropServices.RuntimeInformation.OSDescription);
            return true;
        }
        catch (System.Exception)
        {
            return false;
        }
    }
}