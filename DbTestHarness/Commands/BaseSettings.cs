using System.ComponentModel;
using System.Text.Json;
using DbTestHarness.Models;
using Spectre.Console.Cli;

namespace DbTestHarness.Commands;

public class BaseSettings : CommandSettings
{
    private UserConfig? _userConfig;

    [CommandOption("-c|--config")]
    [Description("Configuration file to use")]
    public string? ConfigFile { get; init; }

    public UserConfig GetConfig()
    {
        _userConfig ??= ReadConfig(ConfigFile ?? GetPlatformPath()).GetAwaiter().GetResult();

        return _userConfig;
    }

    private static string GetPlatformPath()
    {
        // Check for XDG_CONFIG_HOME first (Linux/macOS)
        var xdgConfigHome = Environment.GetEnvironmentVariable("XDG_CONFIG_HOME");
        if (!string.IsNullOrEmpty(xdgConfigHome))
        {
            return Path.Combine(xdgConfigHome, "server-test-harness", "config.json");
        }

        if (OperatingSystem.IsWindows())
        {
            // Windows: Use AppData\Roaming
            var winConfigDir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

            return Path.Combine(winConfigDir, "Server Test Harness", "config.json");
        }

        // Linux/macOS: Use ~/.config (XDG Base Directory Specification)
        var configDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".config");

        return Path.Combine(configDir, "server-test-harness", "config.json");
    }

    private static async Task<UserConfig> ReadConfig(string path)
    {
        UserConfig? config = null;

        if (File.Exists(path))
        {
            var stream = File.OpenRead(path);
            config = await JsonSerializer.DeserializeAsync<UserConfig>(stream);
        }

        if (config is null)
            throw new FileNotFoundException($"Configuration file not found: {path}");

        return config;
    }
}