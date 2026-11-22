using CommandLine;

namespace Servy.CLI.Options
{
    /// <summary>
    /// Options for the <c>status</c> command, which retrieves the current status of a Windows service.
    /// </summary>
    [Verb(
        "status",
        HelpText = "获取 Windows 服务的当前状态。可能的结果：NotInstalled、Stopped、StartPending、StopPending、Running、ContinuePending、PausePending、Paused、Unknown。"
    )]
    public class ServiceStatusOptions : GlobalOptionsBase
    {
        /// <summary>
        /// Gets or sets the name of the Windows service to check.
        /// This option is required.
        /// </summary>
        [Option('n', "name", Required = true, HelpText = "要检查的服务名称。")]
        public string? ServiceName { get; set; }
    }
}
