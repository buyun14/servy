using CommandLine;

namespace Servy.CLI.Options
{
    /// <summary>
    /// Options for the <c>show</c> command, which prints a human-readable view of the Servy
    /// service configuration stored in the database.
    /// </summary>
    /// <remarks>
    /// The command has two modes, selected by whether <see cref="ServiceName"/> is supplied:
    /// <list type="bullet">
    /// <item><description>with <c>--name</c>, the full configuration of a single service, grouped by category;</description></item>
    /// <item><description>without it, a one-line-per-service table of every service in the database,
    /// optionally narrowed by <see cref="SearchKeyword"/>.</description></item>
    /// </list>
    /// The two options are mutually exclusive: <c>--search</c> narrows the list, and there is nothing
    /// for it to narrow once a single service has been named.
    /// </remarks>
    [Verb("show", HelpText = "以人类可读的形式显示 Servy Windows 服务配置；未指定名称时列出所有服务。")]
    public class ShowServiceOptions : GlobalOptionsBase
    {
        /// <summary>
        /// Gets or sets the name of the service to show.
        /// When omitted, the command lists every service in the database instead.
        /// </summary>
        [Option('n', "name", Required = false, HelpText = "要显示的服务名称。省略时列出所有服务。")]
        public string? ServiceName { get; set; }

        /// <summary>
        /// Gets or sets the keyword used to narrow the service list by name or description.
        /// Only meaningful in list mode; it cannot be combined with <see cref="ServiceName"/>.
        /// </summary>
        [Option('s', "search", Required = false, HelpText = "按与服务名称或描述匹配的关键词筛选服务列表。不能与 --name 一起使用。")]
        public string? SearchKeyword { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the fields encrypted at rest are shown in clear text
        /// instead of masked. The stored password is masked either way and is never printed.
        /// </summary>
        /// <remarks>
        /// Only meaningful with <see cref="ServiceName"/>: the service list renders no encrypted column,
        /// so the flag has nothing to reveal there and is refused rather than silently ignored.
        /// </remarks>
        [Option('d', "decrypt", Required = false, HelpText = "以明文而非掩码显示加密字段（参数、环境变量和心跳 URL）。需要 --name。已存储的密码仍保持掩码。")]
        public bool Decrypt { get; set; }
    }
}
