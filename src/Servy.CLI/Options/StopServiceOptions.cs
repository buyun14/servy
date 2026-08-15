using CommandLine;

namespace Servy.CLI.Options
{
    /// <summary>
    /// Options for the <c>stop</c> command to stop a Windows service.
    /// </summary>
    [Verb("stop", HelpText = "停止 Windows 服务。")]
    public class StopServiceOptions : GlobalOptionsBase
    {
        /// <summary>
        /// Gets or sets the name of the service to stop.
        /// This option is required.
        /// </summary>
        [Option('n', "name", Required = true, HelpText = "要停止的服务名称。")]
        public string? ServiceName { get; set; }
    }
}
