using CShroudApp.Core.Entities;
using CShroudApp.Core.Interfaces;

namespace CShroudApp.Infrastructure.Platforms.Windows.Services;

using System;
using System.Runtime.Versioning;
using System.Runtime.InteropServices;
using Microsoft.Win32;

[SupportedOSPlatform("windows")]
public class WindowsProxyManager : IProxyManager
{
    private const string REGISTRY_PATH = @"Software\Microsoft\Windows\CurrentVersion\Internet Settings";

    [DllImport("wininet.dll", SetLastError = true)]
    private static extern bool InternetSetOption(IntPtr hInternet, int dwOption, IntPtr lpBuffer, int dwBufferLength);

    private const int INTERNET_OPTION_SETTINGS_CHANGED = 39;
    private const int INTERNET_OPTION_REFRESH = 37;

    public ProxyStruct? GetProxyData()
    {
        try
        {
            using (RegistryKey? key = Registry.CurrentUser.OpenSubKey(REGISTRY_PATH))
            {
                if (key == null) return null;
                var proxyStruct = new ProxyStruct()
                {
                    Enabled = (bool)(key.GetValue("ProxyEnable") ?? false),
                    ExcludedHosts = new List<string>() {},
                    Address = key.GetValue("ProxyAddress")?.ToString() ?? string.Empty,
                };

                return proxyStruct;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        
        return null;
    }
    
    public async Task EnableAsync(string proxyAddress, List<string> excludedHosts)
    {
        try
        {
            using (RegistryKey? key = Registry.CurrentUser.OpenSubKey(REGISTRY_PATH, true))
            {
                if (key == null) throw new Exception("undefined_regedit_key");
                key.SetValue("ProxyEnable", 1);
                key.SetValue("ProxyServer", proxyAddress);
                
                if (excludedHosts.Any())
                {
                    string excluded = string.Join(";", excludedHosts);
                    key.SetValue("ProxyOverride", excluded);
                }
            }
            
            ApplySettings();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при включении прокси: {ex.Message}");
        }
    }

    public async Task DisableAsync(string? oldAddress, List<string>? excludedHosts)
    {
        try
        {
            using (RegistryKey? key = Registry.CurrentUser.OpenSubKey(REGISTRY_PATH, true))
            {
                if (key == null) throw new Exception("undefined_regedit_key");
                key.SetValue("ProxyEnable", 0);
                key.DeleteValue("ProxyServer", false);
                key.DeleteValue("ProxyOverride", false);
            }
            
            ApplySettings();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при отключении прокси: {ex.Message}");
        }
    }

    private static void ApplySettings()
    {
        InternetSetOption(IntPtr.Zero, INTERNET_OPTION_SETTINGS_CHANGED, IntPtr.Zero, 0);
        InternetSetOption(IntPtr.Zero, INTERNET_OPTION_REFRESH, IntPtr.Zero, 0);
    }
}