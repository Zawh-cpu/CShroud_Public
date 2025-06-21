using System.Runtime.InteropServices;

namespace CShroudApp.Core.Utils;

public enum Platform
{
    Windows,
    Linux,
    MacOS,
    Android,
    iOS,
    Unknown
}

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
    
    public static Platform GetPlatformRaw()
    {
        if (OperatingSystem.IsWindows()) return Platform.Windows;
        if (OperatingSystem.IsLinux()) return Platform.Linux;
        if (OperatingSystem.IsMacOS()) return Platform.MacOS;
        if (OperatingSystem.IsAndroid()) return Platform.Android;
        if (OperatingSystem.IsIOS()) return Platform.iOS;
        return Platform.Unknown;
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