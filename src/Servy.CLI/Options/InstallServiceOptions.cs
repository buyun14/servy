using CommandLine;
using Servy.Core.Config;

namespace Servy.CLI.Options
{
    /// <summary>
    /// Command options for <c>install</c> command.
    /// Installs a new Windows service with specified parameters.
    /// </summary>
    [Verb("install", HelpText = "安装 Windows 服务。")]
    public class InstallServiceOptions : GlobalOptionsBase
    {
        private const string SecurityWarningPrefix = " SECURITY WARNING: Use the ";
        private const string SecurityWarningSuffixParams = " environment variable instead to avoid exposing sensitive parameters in OS process listings.";
        private const string SecurityWarningSuffixCreds = " environment variable instead to avoid exposing credentials in OS process listings.";
        private const string EnvVarEscapingRecipe = "Enter variables in the format varName=varValue separated by semicolons (;). Use \\= to escape '=', \\\" to escape '\"', \\; to escape ';', \\\\ to escape '\\', and %% to escape '%' (collapses to a single '%'). Supports environment variable expansion, example: VAR1=%ProgramData%\\MyApp; VAR2=%VAR1%\\bin.";

        /// <summary>
        /// Gets or sets the service name.
        /// This option is required and specifies the unique name of the service to install.
        /// </summary>
        [Option('n', "name", Required = true, HelpText = "要安装的服务的唯一名称。")]
        public string? ServiceName { get; set; }

        /// <summary>
        /// Gets or sets the service display name.
        /// </summary>
        [Option("displayName", HelpText = "在 Windows 服务控制台（services.msc）中显示的人类可读名称。如果留空，则改用服务名称。")]
        public string? ServiceDisplayName { get; set; }

        /// <summary>
        /// Gets or sets the service description.
        /// Optional descriptive text about the service.
        /// </summary>
        [Option('d', "description", HelpText = "服务的描述。")]
        public string? ServiceDescription { get; set; }

        /// <summary>
        /// Gets or sets the path to the executable process to run as service.
        /// This option is required.
        /// </summary>
        [Option('p', "path", Required = true, HelpText = "可执行进程的路径。支持环境变量展开，例如：%JAVA_HOME%\\bin\\java.exe")]
        public string? ProcessPath { get; set; }

        /// <summary>
        /// Gets or sets the working directory for the service process.
        /// Optional.
        /// </summary>
        [Option("startupDir", HelpText = "进程的启动目录。支持环境变量展开，例如：%PROGRAMDATA%\\MyApp")]
        public string? StartupDirectory { get; set; }

        /// <summary>
        /// Gets or sets additional command-line parameters for the process.
        /// Optional.
        /// </summary>
        /// <remarks>
        /// Passing parameters via CLI flags is insecure as they are visible
        /// in process listings and shell history. Use the <see cref="AppConfig.ProcessParametersEnvVarName"/> environment
        /// variable instead.
        /// </remarks>
        [Sensitive]
        [Option("params", HelpText = "进程的附加参数。支持环境变量展开，例如：--params=\"--data %ProgramData%\\MyApp --bin %MY_VAR%\\bin\"。" + SecurityWarningPrefix + AppConfig.ProcessParametersEnvVarName + SecurityWarningSuffixParams)]
        public string? ProcessParameters { get; set; }

        /// <summary>
        /// Gets or sets the startup type of the service.
        /// Possible values:
        /// <list type="bullet">
        /// <item><description>Automatic - Service starts automatically during system startup.</description></item>
        /// <item><description>AutomaticDelayedStart - Service starts automatically with a short delay after system startup.</description></item>
        /// <item><description>Manual - Service must be started manually.</description></item>
        /// <item><description>Disabled - Service is disabled and cannot be started.</description></item>
        /// </list>
        /// </summary>
        [Option("startupType", HelpText = "服务启动类型。选项：Automatic、AutomaticDelayedStart、Manual、Disabled。默认为 Automatic。")]
        public string? ServiceStartType { get; set; }

        /// <summary>
        /// Gets or sets the process priority for the service.
        /// Possible values:
        /// <list type="bullet">
        /// <item><description>Idle</description></item>
        /// <item><description>BelowNormal</description></item>
        /// <item><description>Normal</description></item>
        /// <item><description>AboveNormal</description></item>
        /// <item><description>High</description></item>
        /// <item><description>RealTime</description></item>
        /// </list>
        /// </summary>
        [Option("priority", HelpText = "进程优先级级别。选项：Idle、BelowNormal、Normal、AboveNormal、High、RealTime。默认为 Normal。")]
        public string? ProcessPriority { get; set; }

        /// <summary>
        /// Gets or sets the logical CPUs the process may run on (e.g., '0-3,8' or '0xFF00').
        /// </summary>
        [Option('a', "cpuAffinity", Required = false, HelpText = "进程可以运行的逻辑 CPU（例如：'0-3,8' 或 '0xFF00'）。")]
        public string? CpuAffinity { get; set; }

        /// <summary>
        /// Gets or sets timeout in seconds to wait for the process to start successfully before considering the startup as failed.
        /// Must be between <see cref="AppConfig.MinStartTimeout"/> and <see cref="AppConfig.MaxStartTimeout"/> seconds.
        /// Optional. Defaults to 10 seconds.
        /// </summary>
        [Option("startTimeout", HelpText = "等待进程成功启动的超时秒数，超过后视为启动失败。必须介于 1 到 86400 秒之间。默认为 10 秒。")]
        public string? StartTimeout { get; set; }

        /// <summary>
        /// Gets or sets timeout in seconds to wait for the process to exit.
        /// Must be between <see cref="AppConfig.MinStopTimeout"/> and <see cref="AppConfig.MaxStopTimeout"/> seconds.
        /// Optional. Defaults to 5 seconds.
        /// </summary>
        [Option("stopTimeout", HelpText = "等待进程退出的超时秒数。必须介于 1 到 86400 秒之间。默认为 5 秒。")]
        public string? StopTimeout { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to enable the console user interface for the service.
        /// </summary>
        [Option("enableConsoleUI", HelpText = "为服务启用控制台用户界面。启用后，标准输出/标准错误重定向将被禁用。")]
        public bool EnableConsoleUI { get; set; }

        /// <summary>
        /// Gets or sets the file path to capture standard output logs.
        /// Optional.
        /// </summary>
        [Option("stdout", HelpText = "标准输出日志文件的路径。")]
        public string? StdoutPath { get; set; }

        /// <summary>
        /// Gets or sets the file path to capture standard error logs.
        /// Optional.
        /// </summary>
        [Option("stderr", HelpText = "标准错误日志文件的路径。")]
        public string? StderrPath { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether size-based log rotation is enabled.
        /// This option is deprecated and is kept for backward compatibility. Use --enableSizeRotation instead.
        /// </summary>
        [Option("enableRotation", HelpText = "已弃用。启用基于大小的日志轮转。此选项仅为向后兼容而保留。请改用 --enableSizeRotation。")]
        public bool EnableRotation { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether size-based log rotation is enabled.
        /// </summary>
        [Option("enableSizeRotation", HelpText = "启用基于大小的日志轮转。")]
        public bool EnableSizeRotation { get; set; }

        /// <summary>
        /// Gets or sets the rotation size in megabytes (MB) for log files.
        /// Must be between <see cref="AppConfig.MinRotationSize"/> and <see cref="AppConfig.MaxRotationSize"/> MB if rotation is enabled.
        /// </summary>
        [Option("rotationSize", HelpText = "日志轮转大小（兆字节 MB）。必须介于 1 到 10240 MB 之间。")]
        public string? RotationSize { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether date-based log rotation is enabled based on the date interval specified by --dateRotationType.
        /// </summary>
        [Option("enableDateRotation", HelpText = "根据 --dateRotationType 指定的日期间隔启用基于日期的日志轮转。当同时启用基于大小和基于日期的轮转时，大小轮转优先。")]
        public bool EnableDateRotation { get; set; }

        /// <summary>
        /// Gets or sets the date rotation type.
        /// Possible values:
        /// <list type="bullet">
        /// <item><description>Daily</description></item>
        /// <item><description>Weekly</description></item>
        /// <item><description>Monthly</description></item>
        /// <item><description>None - Disables date-based rotation; use when only size rotation is desired.</description></item>
        /// </list>
        /// </summary>
        [Option("dateRotationType", HelpText = "日期轮转类型。选项：Daily、Weekly、Monthly、None（None 禁用基于日期的轮转；仅在只需要大小轮转时使用）。")]
        public string? DateRotationType { get; set; }

        /// <summary>
        /// Gets or sets the maximum number of rotated log files to keep.
        /// Must be between <see cref="AppConfig.MinMaxRotations"/> and <see cref="AppConfig.MaxMaxRotations"/>.
        /// Set to 0 for unlimited.
        /// </summary>
        [Option("maxRotations", HelpText = "要保留的轮转日志文件最大数量。必须介于 0 到 10000 之间。设为 0 或留空表示无限制。")]
        public string? MaxRotations { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to use local system time for log rotation.
        /// </summary>
        /// <remarks>
        /// <para>Default is <see cref="AppConfig.DefaultUseLocalTimeForRotation"/> (<c>false</c>).</para>
        /// <para>When <c>true</c>, rotation occurs at local midnight. When <c>false</c>, rotation occurs at UTC midnight.</para>
        /// </remarks>
        [Option("useLocalTimeForRotation", HelpText = "日志轮转使用服务器本地时间而非 UTC。默认为 false。")]
        public bool UseLocalTimeForRotation { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether debug logs are enabled.
        /// When enabled, environment variables and process parameters are recorded in the Servy.Service.log file.
        /// Not recommended for production environments, as these logs may contain sensitive information.
        /// </summary>
        [Option("debug", HelpText = "是否启用调试日志。启用后，环境变量和进程参数会记录到 Servy.Service.log 文件中。不建议在生产环境中使用，因为这些日志可能包含敏感信息。")]
        public bool EnableDebugLogs { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether health monitoring is enabled.
        /// </summary>
        [Option("enableHealth", HelpText = "启用运行状况监视。")]
        public bool EnableHealthMonitoring { get; set; }

        /// <summary>
        /// Gets or sets the heartbeat interval in seconds for health monitoring.
        /// Must be between <see cref="AppConfig.MinHeartbeatInterval"/> and <see cref="AppConfig.MaxHeartbeatInterval"/> seconds if health monitoring is enabled.
        /// </summary>
        [Option("heartbeatInterval", HelpText = "心跳间隔（秒）。必须介于 5 到 86400 秒之间。仅在启用运行状况监视时使用。")]
        public string? HeartbeatInterval { get; set; }

        /// <summary>
        /// Gets or sets the maximum number of failed health checks before recovery action.
        /// Must be between <see cref="AppConfig.MinMaxFailedChecks"/> and <see cref="AppConfig.MaxMaxFailedChecks"/> if health monitoring is enabled.
        /// </summary>
        [Option("maxFailedChecks", HelpText = "允许的最大失败运行状况检查次数。必须介于 1 到 100000 之间。仅在启用运行状况监视时使用。")]
        public string? MaxFailedChecks { get; set; }

        /// <summary>
        /// Gets or sets the recovery action to perform on failure.
        /// Possible values:
        /// <list type="bullet">
        /// <item><description>None - No action will be taken.</description></item>
        /// <item><description>RestartService - Restart the service.</description></item>
        /// <item><description>RestartProcess - Restart the process.</description></item>
        /// <item><description>RestartComputer - Restart the computer.</description></item>
        /// </list>
        /// </summary>
        [Option("recoveryAction", HelpText = "失败时的恢复操作。选项：None、RestartService、RestartProcess、RestartComputer。如果服务在 NT AUTHORITY\\LocalService、NT AUTHORITY\\NetworkService、IIS AppPool 标识（IIS APPPOOL\\...）或无所需权限的用户账户下运行，则 RestartService 和 RestartComputer 操作不可用。仅在启用运行状况监视时使用。")]
        public string? RecoveryAction { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to run recovery action even if the process exits successfully.
        /// </summary>
        [Option("recoveryOnCleanExit", HelpText = "即使进程正常退出也执行恢复操作。默认为 false。仅在启用运行状况监视时使用。")]
        public bool RecoveryOnCleanExit { get; set; }

        /// <summary>
        /// Gets or sets the maximum number of restart attempts after failure.
        /// Must be between <see cref="AppConfig.MinMaxRestartAttempts"/> and <see cref="AppConfig.MaxMaxRestartAttempts"/> if health monitoring is enabled.
        /// Set to 0 for unlimited restart attempts.
        /// </summary>
        [Option("maxRestartAttempts", HelpText = "失败时的最大重启尝试次数。必须介于 0 到 100000 之间。设为 0 表示无限制重启尝试。仅在启用运行状况监视时使用。")]
        public string? MaxRestartAttempts { get; set; }

        /// <summary>
        /// Gets or sets the absolute URL used to send out-of-band diagnostic heartbeat pings (e.g., healthchecks.io).
        /// While the process is healthy, Servy periodically pings this endpoint to confirm service vitality.
        /// </summary>
        /// <remarks>
        /// Passing the URL via CLI flags is insecure as it is visible
        /// in process listings and shell history. Use the <see cref="AppConfig.HeartbeatUrlEnvVarName"/> environment
        /// variable instead.
        /// </remarks>
        [Sensitive]
        [Option("heartbeatUrl", HelpText = "用于带外诊断心跳 ping 的绝对 URL。仅在启用运行状况监视时使用。" + SecurityWarningPrefix + AppConfig.HeartbeatUrlEnvVarName + SecurityWarningSuffixParams)]
        public string? HeartbeatUrl { get; set; }

        /// <summary>
        /// Gets or sets the HTTP request timeout in seconds for external heartbeat URL pings.
        /// Value must be between <see cref="AppConfig.MinHeartbeatUrlTimeoutSeconds"/> and <see cref="AppConfig.MaxHeartbeatUrlTimeoutSeconds"/>.
        /// Default is <see cref="AppConfig.DefaultHeartbeatUrlTimeoutSeconds"/>.
        /// </summary>
        [Option("heartbeatUrlTimeoutSeconds", HelpText = "外部心跳 URL 请求的超时秒数。必须介于 2 到 30 秒之间。默认为 10 秒。仅在启用运行状况监视时使用。")]
        public string? HeartbeatUrlTimeoutSeconds { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether extended flags (/start, /fail) are appended to the heartbeat URL during service startup and failure events.
        /// Default is <see cref="AppConfig.DefaultEnableHeartbeatUrlFlags"/>.
        /// </summary>
        [Option("enableHeartbeatUrlFlags", HelpText = "服务启动和失败时在心跳 URL 后追加 /start 和 /fail。仅在启用运行状况监视时使用。")]
        public bool EnableHeartbeatUrlFlags { get; set; }

        /// <summary>
        /// Gets or sets the failure program path.
        /// Optional.
        /// </summary>
        [Option("failureProgramPath", HelpText = "失败处理程序路径。配置在托管进程以非零退出码退出时（恢复已禁用）或所有恢复操作重试均失败后（恢复已启用）运行的脚本或可执行文件。进程启动失败时不会运行该程序；该路径会停止服务。支持环境变量展开，例如：%JAVA_HOME%\\bin\\java.exe")]
        public string? FailureProgramPath { get; set; }

        /// <summary>
        /// Gets or sets the failure program startup directory.
        /// Optional. If not set, defaults to the service working directory.
        /// </summary>
        [Option("failureProgramStartupDir", HelpText = "指定失败处理程序的启动目录。如未设置，默认使用服务工作目录。支持环境变量展开，例如：%PROGRAMDATA%\\MyApp")]
        public string? FailureProgramStartupDir { get; set; }

        /// <summary>
        /// Gets or sets additional command-line parameters for the failure program.
        /// Optional.
        /// </summary>
        /// <remarks>
        /// Passing parameters via CLI flags is insecure as they are visible
        /// in process listings and shell history. Use the <see cref="AppConfig.FailureProgramParametersEnvVarName"/> environment
        /// variable instead.
        /// </remarks>
        [Sensitive]
        [Option("failureProgramParams", HelpText = "失败处理程序的附加参数。" + SecurityWarningPrefix + AppConfig.FailureProgramParametersEnvVarName + SecurityWarningSuffixParams)]
        public string? FailureProgramParameters { get; set; }

        /// <summary>
        /// Gets or sets environment variables for the process.
        /// Optional.
        /// </summary>
        /// <remarks>
        /// Passing environment variables via CLI flags is insecure as they are visible
        /// in process listings and shell history. Use the <see cref="AppConfig.EnvironmentVariablesEnvVarName"/> environment
        /// variable instead.
        /// </remarks>
        [Sensitive]
        [Option("envVars", HelpText = "进程的环境变量。" + EnvVarEscapingRecipe + SecurityWarningPrefix + AppConfig.EnvironmentVariablesEnvVarName + SecurityWarningSuffixParams)]
        public string? EnvironmentVariables { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to allow overriding of protected runtime variables (e.g., <c>JAVA_HOME</c>, <c>JAVA_OPTS</c>, <c>CATALINA_OPTS</c>) by the service's environment variables.
        /// </summary>
        [Option("allowOverriddenRuntimeVars", HelpText = "允许服务的环境变量覆盖受保护的运行时变量（例如 JAVA_HOME、JAVA_OPTS、CATALINA_OPTS）。")]
        public bool AllowOverriddenRuntimeVars { get; set; }

        /// <summary>
        /// Gets or sets Windows service dependencies.
        /// Optional.
        /// </summary>
        [Option("deps", HelpText = "指定此服务依赖的一个或多个 Windows 服务名称（非显示名称），用分号（;）分隔。每个服务名称只能包含字母、数字、连字符、下划线、句点、空格和美元符号（$），可选择以 '+' 前缀来引用加载顺序组，且不得超过 256 个字符。此服务启动时，Windows 会自动启动处于停止状态的依赖项；如果某个依赖项被禁用或启动失败，则此服务将无法启动。")]
        public string? ServiceDependencies { get; set; }

        /// <summary>
        /// Gets or sets the Windows service account username.
        /// Optional.
        /// </summary>
        [Option(
            "user",
            HelpText = "服务账户用户名（例如：.\\username、DOMAIN\\username、DOMAIN\\gMSA$，或 NT AUTHORITY\\LocalService、NT AUTHORITY\\NetworkService、NT SERVICE\\MyService、IIS APPPOOL\\MyPool 等内置标识）。" +
                       "对于内置账户、虚拟账户和 gMSA 账户，请将 --password 选项或 " + AppConfig.PasswordEnvVarName + " 环境变量留空。" +
                       "如果未设置此选项，服务将以 Local System 身份运行。"
        )]
        public string? User { get; set; }

        /// <summary>
        /// Gets or sets the Windows service account password.
        /// </summary>
        /// <remarks>
        /// Passing passwords via CLI flags is insecure as they are visible
        /// in process listings and shell history. Use the <see cref="AppConfig.PasswordEnvVarName"/> environment
        /// variable instead.
        /// </remarks>
        [Sensitive]
        [Option("password", HelpText = "服务账户的密码。" + SecurityWarningPrefix + AppConfig.PasswordEnvVarName + SecurityWarningSuffixCreds)]
        public string? Password { get; set; }

        /// <summary>
        /// Gets or sets the pre-launch executable path.
        /// Optional.
        /// </summary>
        [Option("preLaunchPath", HelpText = "预启动可执行文件路径。配置在主服务启动前运行的可选脚本或可执行文件。这对于准备配置、获取机密或其他设置任务很有用。如果预启动脚本失败，除非启用 --preLaunchIgnoreFailure，否则服务将不会启动。支持环境变量展开，例如：%JAVA_HOME%\\bin\\java.exe")]
        public string? PreLaunchPath { get; set; }

        /// <summary>
        /// Gets or sets the pre-launch startup directory.
        /// Optional. If not set, defaults to the service working directory.
        /// </summary>
        [Option("preLaunchStartupDir", HelpText = "指定预启动可执行文件的启动目录。如未设置，默认使用服务工作目录。支持环境变量展开，例如：%PROGRAMDATA%\\MyApp")]
        public string? PreLaunchStartupDir { get; set; }

        /// <summary>
        /// Gets or sets additional parameters for the pre-launch executable.
        /// Optional.
        /// </summary>
        /// <remarks>
        /// Passing parameters via CLI flags is insecure as they are visible
        /// in process listings and shell history. Use the <see cref="AppConfig.PreLaunchParametersEnvVarName"/> environment
        /// variable instead.
        /// </remarks>
        [Sensitive]
        [Option("preLaunchParams", HelpText = "预启动可执行文件的附加参数。" + SecurityWarningPrefix + AppConfig.PreLaunchParametersEnvVarName + SecurityWarningSuffixParams)]
        public string? PreLaunchParameters { get; set; }

        /// <summary>
        /// Gets or sets environment variables for the pre-launch executable.
        /// Optional.
        /// </summary>
        /// <remarks>
        /// Passing environment variables via CLI flags is insecure as they are visible
        /// in process listings and shell history. Use the <see cref="AppConfig.PreLaunchEnvironmentVariablesEnvVarName"/> environment
        /// variable instead.
        /// </remarks>
        [Sensitive]
        [Option("preLaunchEnv", HelpText = "预启动可执行文件的环境变量。" + EnvVarEscapingRecipe + SecurityWarningPrefix + AppConfig.PreLaunchEnvironmentVariablesEnvVarName + SecurityWarningSuffixParams)]
        public string? PreLaunchEnvironmentVariables { get; set; }

        /// <summary>
        /// Gets or sets the file path to capture standard output logs.
        /// Optional.
        /// </summary>
        [Option("preLaunchStdout", HelpText = "预启动可执行文件的标准输出日志文件路径。")]
        public string? PreLaunchStdoutPath { get; set; }

        /// <summary>
        /// Gets or sets the file path to capture standard error logs.
        /// Optional.
        /// </summary>
        [Option("preLaunchStderr", HelpText = "预启动可执行文件的标准错误日志文件路径。")]
        public string? PreLaunchStderrPath { get; set; }

        /// <summary>
        /// Gets or sets the timeout for the pre-launch executable.
        /// Must be between <see cref="AppConfig.MinPreLaunchTimeoutSeconds"/> and <see cref="AppConfig.MaxPreLaunchTimeoutSeconds"/> seconds.
        /// Optional.
        /// </summary>
        [Option("preLaunchTimeout", HelpText = "预启动可执行文件的超时。必须介于 0 到 86400 秒之间。将超时设为 0 即以\"即发即忘\"模式运行预启动钩子。设为 0 时，钩子启动后立即启动服务，不等待其完成。仅用于不影响服务启动或正常运行的任务。在\"即发即忘\"模式下不支持标准输出/标准错误重定向和重试。")]
        public string? PreLaunchTimeout { get; set; }

        /// <summary>
        /// Gets or sets the pre-launch retry attempts.
        /// Must be between <see cref="AppConfig.MinPreLaunchRetryAttempts"/> and <see cref="AppConfig.MaxPreLaunchRetryAttempts"/>.
        /// Optional.
        /// </summary>
        [Option("preLaunchRetryAttempts", HelpText = "预启动可执行文件失败时的重试次数。必须介于 0 到 100000 之间。")]
        public string? PreLaunchRetryAttempts { get; set; }

        /// <summary>
        /// Gets or sets the pre-launch ignore failure flag.
        /// Optional.
        /// </summary>
        [Option("preLaunchIgnoreFailure", HelpText = "忽略失败，即使预启动可执行文件失败也启动服务。")]
        public bool PreLaunchIgnoreFailure { get; set; }

        /// <summary>
        /// Gets or sets the post-launch executable path.
        /// Optional.
        /// </summary>
        [Option("postLaunchPath", HelpText = "启动后可执行文件路径。配置在进程成功启动后运行的可选脚本或可执行文件。支持环境变量展开，例如：%JAVA_HOME%\\bin\\java.exe")]
        public string? PostLaunchPath { get; set; }

        /// <summary>
        /// Gets or sets the post-launch startup directory.
        /// Optional. If not set, defaults to the service working directory.
        /// </summary>
        [Option("postLaunchStartupDir", HelpText = "指定启动后可执行文件的启动目录。如未设置，默认使用服务工作目录。支持环境变量展开，例如：%PROGRAMDATA%\\MyApp")]
        public string? PostLaunchStartupDir { get; set; }

        /// <summary>
        /// Gets or sets additional parameters for the post-launch executable.
        /// Optional.
        /// </summary>
        /// <remarks>
        /// Passing parameters via CLI flags is insecure as they are visible
        /// in process listings and shell history. Use the <see cref="AppConfig.PostLaunchParametersEnvVarName"/> environment
        /// variable instead.
        /// </remarks>
        [Sensitive]
        [Option("postLaunchParams", HelpText = "启动后可执行文件的附加参数。" + SecurityWarningPrefix + AppConfig.PostLaunchParametersEnvVarName + SecurityWarningSuffixParams)]
        public string? PostLaunchParameters { get; set; }

        /// <summary>
        /// Gets or sets the pre-stop executable path.
        /// Optional.
        /// </summary>
        [Option("preStopPath", HelpText = "停止前可执行文件路径。配置在主服务停止前运行的可选脚本或可执行文件。可用于优雅关闭任务，例如通知外部系统或排空资源。停止前进程会同步运行，并在运行期间延长服务停止超时。将超时设为 0 即以\"即发即忘\"模式运行停止前进程。支持环境变量展开，例如：%JAVA_HOME%\\bin\\java.exe")]
        public string? PreStopPath { get; set; }

        /// <summary>
        /// Gets or sets the pre-stop startup directory.
        /// Optional. If not set, defaults to the service working directory.
        /// </summary>
        [Option("preStopStartupDir", HelpText = "指定停止前可执行文件的启动目录。如未设置，默认使用服务工作目录。支持环境变量展开，例如：%PROGRAMDATA%\\MyApp")]
        public string? PreStopStartupDir { get; set; }

        /// <summary>
        /// Gets or sets additional parameters for the pre-stop executable.
        /// Optional.
        /// </summary>
        /// <remarks>
        /// Passing parameters via CLI flags is insecure as they are visible
        /// in process listings and shell history. Use the <see cref="AppConfig.PreStopParametersEnvVarName"/> environment
        /// variable instead.
        /// </remarks>
        [Sensitive]
        [Option("preStopParams", HelpText = "停止前可执行文件的附加参数。" + SecurityWarningPrefix + AppConfig.PreStopParametersEnvVarName + SecurityWarningSuffixParams)]
        public string? PreStopParameters { get; set; }

        /// <summary>
        /// Gets or sets the timeout for the pre-stop executable.
        /// Must be between <see cref="AppConfig.MinPreStopTimeoutSeconds"/> and <see cref="AppConfig.MaxPreStopTimeoutSeconds"/> seconds.
        /// Optional.
        /// </summary>
        [Option("preStopTimeout", HelpText = "停止前可执行文件的超时。将超时设为 0 即以\"即发即忘\"模式运行停止前进程。必须介于 0 到 86400 秒之间。")]
        public string? PreStopTimeout { get; set; }

        /// <summary>
        /// Gets or sets a flag to log pre-stop failure as error.
        /// Optional.
        /// </summary>
        [Option("preStopLogAsError", HelpText = "将停止前失败记录为错误。")]
        public bool PreStopLogAsError { get; set; }

        /// <summary>
        /// Gets or sets the post-stop executable path.
        /// Optional.
        /// </summary>
        [Option("postStopPath", HelpText = "停止后可执行文件路径。配置在托管进程及其所有子进程退出后运行的可选脚本或可执行文件。停止后进程以\"即发即忘\"模式启动，不会阻塞服务关闭。支持环境变量展开，例如：%JAVA_HOME%\\bin\\java.exe")]
        public string? PostStopPath { get; set; }

        /// <summary>
        /// Gets or sets the post-stop startup directory.
        /// Optional. If not set, defaults to the service working directory.
        /// </summary>
        [Option("postStopStartupDir", HelpText = "指定停止后可执行文件的启动目录。如未设置，默认使用服务工作目录。支持环境变量展开，例如：%PROGRAMDATA%\\MyApp")]
        public string? PostStopStartupDir { get; set; }

        /// <summary>
        /// Gets or sets additional parameters for the post-stop executable.
        /// Optional.
        /// </summary>
        /// <remarks>
        /// Passing parameters via CLI flags is insecure as they are visible
        /// in process listings and shell history. Use the <see cref="AppConfig.PostStopParametersEnvVarName"/> environment
        /// variable instead.
        /// </remarks>
        [Sensitive]
        [Option("postStopParams", HelpText = "停止后可执行文件的附加参数。" + SecurityWarningPrefix + AppConfig.PostStopParametersEnvVarName + SecurityWarningSuffixParams)]
        public string? PostStopParameters { get; set; }
    }
}
