using CommandLine;

namespace Servy.CLI.Options
{
    /// <summary>
    /// Options for the <c>start</c> command to start a Windows service.
    /// </summary>
    [Verb("start", HelpText = "启动 Windows 服务。")]
    public class StartServiceOptions : GlobalOptionsBase
    {
        /// <summary>
        /// Gets or sets the name of the service to start.
        /// This option is required.
        /// </summary>
        [Option('n', "name", Required = true, HelpText = "要启动的服务名称。")]
        public string? ServiceName { get; set; }
    }
}
