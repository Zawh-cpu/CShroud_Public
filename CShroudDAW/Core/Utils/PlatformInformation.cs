using System.Runtime.InteropServices;

namespace CShroudApp.Core.Utils;

public static class PlatformInformation
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
        return RuntimeInformation.OSArchitecture.ToString();
    }

    public static string GetFullname()
    {
        return GetPlatform() + GetArchitecture();
    }
}