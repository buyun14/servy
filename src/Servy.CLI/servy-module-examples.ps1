<#
.SYNOPSIS
    演示如何使用 Servy PowerShell 模块管理 Windows 服务的示例脚本。

.DESCRIPTION
    演示 Servy PowerShell 模块用法的示例脚本。

.NOTES
    Author: Akram El Assas
    Module: Servy
    Requires: PowerShell 2.0 or later
    Repository: https://github.com/aelassas/servy
#>

# ----------------------------------------------------------------
# 导入 Servy PowerShell 模块（如果已导入则强制重新加载）
# ----------------------------------------------------------------
$scriptDir = Split-Path -Parent $MyInvocation.MyCommand.Definition
Import-Module (Join-Path $scriptDir "Servy.psm1") -Force

# ----------------------------------------------------------------
# 调整模块级执行设置（可选）
# ----------------------------------------------------------------
Set-ServyConfig -TimeoutSeconds 1200

# ----------------------------------------------------------------
# 显示 CLI 帮助
# ----------------------------------------------------------------
# Get-ServyHelp -Command "install" -Quiet
# Get-ServyHelp -Quiet

# ----------------------------------------------------------------
# 显示 Servy CLI 的当前版本
# ----------------------------------------------------------------
Get-ServyVersion -Quiet

# ----------------------------------------------------------------
# 使用 Servy 安装新的 Windows 服务（splatting）
# ----------------------------------------------------------------
$installParams = @{
    Quiet                      = $true
    Name                       = "DummyService"
    DisplayName                = "示例服务"
    Description                = "示例服务"
    Path                       = "C:\Windows\System32\notepad.exe"
    StartupDir                 = "C:\Windows\Temp"
    Params                     = "--param 2000"
    StartupType                = "Manual"
    Priority                   = "BelowNormal"
    CpuAffinity                = "0x1"
    StartTimeout               = 15
    StopTimeout                = 10
    EnableConsoleUI            = $false
    EnableSizeRotation         = $true
    RotationSize               = 1
    EnableDateRotation         = $true
    DateRotationType           = "Weekly"
    MaxRotations               = 3
    UseLocalTimeForRotation    = $true

    Stdout                     = "C:\Windows\Temp\dummy-stdout.log"
    Stderr                     = "C:\Windows\Temp\dummy-stderr.log"

    EnableHealth               = $true
    HeartbeatInterval          = 5
    MaxFailedChecks            = 1
    MaxRestartAttempts         = 2
    HeartbeatUrl               = "http://localhost:8080/heartbeat"
    HeartbeatUrlTimeoutSeconds = 5
    EnableHeartbeatUrlFlags    = $true

    RecoveryAction             = "RestartService"
    RecoveryOnCleanExit        = $true

    FailureProgramPath         = "C:\Windows\System32\cmd.exe"
    FailureProgramStartupDir   = "C:\Windows\Temp"
    FailureProgramParams       = "/c exit 0 --param 2001"

    EnvVars                    = "var1=val1; var2=val2;"
    AllowOverriddenRuntimeVars = $true
    Deps                       = "Tcpip;Dnscache"
    # User                     = ".\DummySvc"
    # Password                 = ("ChangeMe!" | ConvertTo-SecureString -AsPlainText -Force)

    PreLaunchPath              = "C:\Windows\System32\cmd.exe"
    PreLaunchStartupDir        = "C:\Windows\Temp"
    PreLaunchParams            = "/c exit 0 --param 2002"
    PreLaunchEnv               = "preVar1=val1; preVar2=val2;"
    PreLaunchStdout            = "C:\Windows\Temp\dummy-pre-stdout.log"
    PreLaunchStderr            = "C:\Windows\Temp\dummy-pre-stderr.log"
    PreLaunchTimeout           = 5
    PreLaunchRetryAttempts     = 1
    PreLaunchIgnoreFailure     = $true

    PostLaunchPath             = "C:\Windows\System32\cmd.exe"
    PostLaunchStartupDir       = "C:\Windows\Temp"
    PostLaunchParams           = "/c exit 0 --param 2003"

    EnableDebugLogs            = $true

    PreStopPath                = "C:\Windows\System32\cmd.exe"
    PreStopStartupDir          = "C:\Windows\Temp"
    PreStopParams              = "/c exit 0 --param 2004"
    PreStopTimeout             = 10
    PreStopLogAsError          = $true

    PostStopPath               = "C:\Windows\System32\cmd.exe"
    PostStopStartupDir         = "C:\Windows\Temp"
    PostStopParams             = "/c exit 0 --param 2005"
}

# $env:SERVY_PASSWORD = "ChangeMe!"
Install-ServyService @installParams
# Remove-Item Env:SERVY_PASSWORD

# ----------------------------------------------------------------
# 显示单个服务的完整配置（人类可读）。
# 加密字段默认以掩码显示。
# ----------------------------------------------------------------
Show-ServyService -Name "DummyService" -Quiet

# ----------------------------------------------------------------
# 同上，但以明文显示参数、环境变量和心跳 URL。
# 已存储的密码仍保持掩码。
# ----------------------------------------------------------------
Show-ServyService -Name "DummyService" -Decrypt -Quiet

# ----------------------------------------------------------------
# 列出 Servy 数据库中存储的所有服务
# ----------------------------------------------------------------
Show-ServyServices -Quiet

# ----------------------------------------------------------------
# 仅列出名称或描述匹配关键词的服务
# ----------------------------------------------------------------
Show-ServyServices -Search "Dummy" -Quiet

# ----------------------------------------------------------------
# 导出服务配置（XML）
# ----------------------------------------------------------------
$exportXmlParams = @{
    Quiet          = $true
    Name           = "DummyService"
    ConfigFileType = "xml"
    Path           = "C:\DummyService.xml"
}

Export-ServyServiceConfig @exportXmlParams

# ----------------------------------------------------------------
# 导出服务配置（JSON）
# ----------------------------------------------------------------
$exportJsonParams = @{
    Quiet          = $true
    Name           = "DummyService"
    ConfigFileType = "json"
    Path           = "C:\DummyService.json"
}

Export-ServyServiceConfig @exportJsonParams

# ----------------------------------------------------------------
# 导入之前导出的服务配置
# ----------------------------------------------------------------
$importXmlParams = @{
    Quiet          = $true
    ConfigFileType = "xml"
    Path           = "C:\DummyService.xml"
    Install        = $true
}

Import-ServyServiceConfig @importXmlParams

$importJsonParams = @{
    Quiet          = $true
    ConfigFileType = "json"
    Path           = "C:\DummyService.json"
}

Import-ServyServiceConfig @importJsonParams

# ----------------------------------------------------------------
# 服务生命周期
# ----------------------------------------------------------------
Start-ServyService    -Name "DummyService" -Quiet
Get-ServyServiceStatus -Name "DummyService" -Quiet
Stop-ServyService     -Name "DummyService" -Quiet
Restart-ServyService  -Name "DummyService" -Quiet

# ----------------------------------------------------------------
# 卸载服务
# ----------------------------------------------------------------
Uninstall-ServyService -Name "DummyService" -Quiet
