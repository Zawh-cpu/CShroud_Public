using System.Runtime.InteropServices;
using CShroudApp.Core.Entities;
using CShroudApp.Core.Interfaces;
using Microsoft.Win32;

namespace CShroudApp.Infrastructure.Platforms.Windows.Services;

public class WindowsProxyService : IProxyManager
{
    private readonly IStorageManager _storageManager;

    public ProxyData? CachedProxy
    {
        get => _storageManager.GetValue<ProxyData>("cachedProxy");
        set
        {
            if (value is not null)
                _storageManager.SetValue("cachedProxy", value);
        }
    }
    
    private const string REGISTRY_PATH = @"Software\Microsoft\Windows\CurrentVersion\Internet Settings";

    [DllImport("wininet.dll", SetLastError = true)]
    private static extern bool InternetSetOption(IntPtr hInternet, int dwOption, IntPtr lpBuffer, int dwBufferLength);

    private const int INTERNET_OPTION_SETTINGS_CHANGED = 39;
    private const int INTERNET_OPTION_REFRESH = 37;

    public WindowsProxyService(IStorageManager storageManager)
    {
        _storageManager = storageManager;
    }
    
    public ProxyData? GetProxyData()
    {
        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            throw new PlatformNotSupportedException();
        
        try
        {
            using (RegistryKey? key = Registry.CurrentUser.OpenSubKey(REGISTRY_PATH))
            {
                if (key == null) return null;
                
                var server = key.GetValue("ProxyServer")?.ToString() ?? string.Empty;
                var enabled = (int?)key.GetValue("ProxyEnable") == 1;

                var excludedHostsRaw = key.GetValue("ProxyOverride")?.ToString() ?? string.Empty;
                var excludedHosts = excludedHostsRaw.Split(';', StringSplitOptions.RemoveEmptyEntries);

                var proxyStruct = MakeProxyData(server, excludedHosts, enabled);
                return proxyStruct;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        
        return null;
    }
    
    public void SetupProxy(ProxyData proxy, bool savePreviousProxy = true)
    {
        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            throw new PlatformNotSupportedException();
        
        if (savePreviousProxy)
            CachedProxy = GetProxyData();
        
        try
        {
            using (RegistryKey? key = Registry.CurrentUser.OpenSubKey(REGISTRY_PATH, true))
            {
                if (key == null) throw new Exception("undefined_regedit_key");
                key.SetValue("ProxyEnable", Convert.ToInt32(proxy.Enabled));
                
                if (proxy.Host is not null && proxy.Port is not null)
                    key.SetValue("ProxyServer", MakeProxyAddress(proxy));
                else
                    key.DeleteValue("ProxyServer", false);

                if (proxy.ExcludedHosts is not null)
                    key.SetValue("ProxyOverride", string.Join(";", proxy.ExcludedHosts));
                else
                    key.DeleteValue("ProxyOverride", false);
            }
            
            ApplySettings();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при включении прокси: {ex.Message}");
        }
    }

    public void DisableProxy()
    {
        SetupProxy(new ProxyData(ProxyProtocol.Http, null, null, null, false));
    }

    private static void ApplySettings()
    {
        InternetSetOption(IntPtr.Zero, INTERNET_OPTION_SETTINGS_CHANGED, IntPtr.Zero, 0);
        InternetSetOption(IntPtr.Zero, INTERNET_OPTION_REFRESH, IntPtr.Zero, 0);
    }

    public string MakeProxyAddress(ProxyData proxy)
    {
        if (proxy.Protocol is ProxyProtocol.Socks)
            return $"socks://{proxy.Host}:{proxy.Port}";
        return $"{proxy.Host}:{proxy.Port}";
    }

    public ProxyData? MakeProxyData(string data, string[] excludedHosts, bool enabled)
    {
        var temp = data.Replace("socks:\\", "").Replace("socks://", "").Split(':');
        if (temp.Length != 2) return null;
        if (!uint.TryParse(temp[1], out uint port)) return null;
        
        if (data.StartsWith("socks", StringComparison.InvariantCultureIgnoreCase))
            return new ProxyData(ProxyProtocol.Socks, temp[0], port, excludedHosts, enabled);
        
        return new ProxyData(ProxyProtocol.Http, temp[0], port, excludedHosts, enabled);
    }
}