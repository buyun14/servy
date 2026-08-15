using CommandLine;

namespace Servy.CLI.Options
{
    /// <summary>
    /// Defines command-line options for the <c>import</c> verb, enabling the ingestion
    /// of Windows service configurations from JSON or XML files into the Servy
    /// database and optional registration with the Windows Service Control Manager.
    /// </summary>
    /// <remarks>
    /// This command requires administrative privileges to perform service installation
    /// and ensures that imported configuration files are validated against
    /// path-security policies and schema constraints.
    /// </remarks>
    [Verb("import", HelpText = "将 Windows 服务配置导入 Servy 数据库，并可选择安装该服务。")]
    public class ImportServiceOptions : GlobalOptionsBase
    {
        /// <summary>
        /// Gets or sets the configuration file type.
        /// Possible values: xml, json.
        /// </summary>
        [Option('c', "config", Required = true, HelpText = "配置文件类型（xml 或 json）。")]
        public string? ConfigFileType { get; set; }

        /// <summary>
        /// Gets or sets the path of the configuration file to import.
        /// </summary>
        [Option('p', "path", Required = true, HelpText = "要导入的配置文件的路径。")]
        public string? Path { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to install the service after import.
        /// If the service is already installed, restarting it is required for changes to take effect.
        /// </summary>
        [Option('i', "install",
            Required = false,
            HelpText = "导入后安装服务。如果服务已安装，需要重启服务才能让更改生效。")]
        public bool InstallService { get; set; }
    }
}
