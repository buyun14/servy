using CommandLine;

namespace Servy.CLI.Options
{
    /// <summary>
    /// Options for the <c>export</c> command to export a Servy Windows service configuration to a file.
    /// </summary>
    [Verb("export", HelpText = "将 Servy Windows 服务配置导出到配置文件。")]
    public class ExportServiceOptions : GlobalOptionsBase
    {
        /// <summary>
        /// Gets or sets the name of the service to export.
        /// </summary>
        [Option('n', "name", Required = true, HelpText = "要导出的服务名称。")]
        public string? ServiceName { get; set; }

        /// <summary>
        /// Gets or sets the configuration file type.
        /// Possible values: xml, json.
        /// </summary>
        [Option('c', "config", Required = true, HelpText = "配置导出文件类型（xml 或 json）。")]
        public string? ConfigFileType { get; set; }

        /// <summary>
        /// Gets or sets the path of the configuration file to export.
        /// </summary>
        [Option('p', "path", Required = true, HelpText = "要导出的配置文件的路径。")]
        public string? Path { get; set; }
    }
}
