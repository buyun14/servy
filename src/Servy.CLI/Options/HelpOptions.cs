using CommandLine;

namespace Servy.CLI.Options
{
    /// <summary>
    /// Represents the 'help' verb to display help information for the CLI.
    /// </summary>
    [Verb("help", HelpText = "显示帮助信息。")]
    public class HelpOptions : GlobalOptionsBase
    {
    }
}
