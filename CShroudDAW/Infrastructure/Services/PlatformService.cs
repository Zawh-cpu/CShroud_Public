using System.Runtime.InteropServices;

namespace CShroudDAW.Infrastructure.Services;

public static class PlatformService
{
    public static string GetPlatform()
    {
        if (OperatingSystem.IsWindows()) return "Windows";
        if (OperatingSystem.IsLinux()) return "Linux";
        if (OperatingSystem.IsMacOS()) return "MacOS";
        if (OperatingSystem.IsAndroid()) return "Android";
        if (OperatingSystem.IsIOS()) return "iOS";
        return String.Empty;
    }

    public static string GetArchitecture()
    {
        var a = RuntimeInformation.OSArchitecture;
        return a.ToString();
    }

    public static string GetFullname()
    {
        return GetPlatform() + GetArchitecture();
    }
}