using CommandLine;

namespace Servy.CLI.Options
{
    /// <summary>
    /// Options for the <c>restart</c> command to restart a Windows service.
    /// </summary>
    [Verb("restart", HelpText = "重启 Windows 服务。")]
    public class RestartServiceOptions : GlobalOptionsBase
    {
        /// <summary>
        /// Gets or sets the name of the service to restart.
        /// This option is required.
        /// </summary>
        [Option('n', "name", Required = true, HelpText = "要重启的服务名称。")]
        public string? ServiceName { get; set; }
    }
}
