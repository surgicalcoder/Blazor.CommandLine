namespace Blazor.CommandLine.Command;

public class Input
{
    public string Text { get; set; }
    public DateTime Time { get; } = DateTime.Now;
}