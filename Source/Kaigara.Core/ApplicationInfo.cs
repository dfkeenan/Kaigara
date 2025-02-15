﻿using System.Reflection;

namespace Kaigara;

public record ApplicationInfo
{
    public string ProductName { get; init; }
    public string? CompanyName { get; init; }
    public string Copyright { get; init; }
    public string Version { get; init; }

    public string ApplicationDataPath { get; init; }
    public Uri? IconUri { get; init; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private ApplicationInfo()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    {

    }

    public static ApplicationInfo FromAssembly(Assembly assembly)
    {
        var productName = assembly.GetCustomAttribute<AssemblyProductAttribute>()?.Product ?? assembly.GetName().Name!;
        var version =
               assembly.GetCustomAttribute<AssemblyFileVersionAttribute>()?.Version
            ?? assembly.GetCustomAttribute<AssemblyVersionAttribute>()?.Version
            ?? "1.0.0";

        Uri? iconUri = null;

        if (assembly.GetManifestResourceNames().Contains($"{assembly.GetName().Name}.Application.ico"))
        {
            iconUri = new Uri($"resm:{assembly.GetName().Name}.Application.ico");
        }
#if DEBUG
        var appDataPath = Path.GetDirectoryName(assembly.Location)!;
#else
            string appDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), productName);
#endif
        return new ApplicationInfo
        {
            ProductName = productName,
            CompanyName = assembly.GetCustomAttribute<AssemblyCompanyAttribute>()?.Company,
            Copyright = assembly.GetCustomAttribute<AssemblyCopyrightAttribute>()?.Copyright ?? $"© {DateTime.Now.Year}",
            Version = version,
            ApplicationDataPath = appDataPath,
            IconUri = iconUri,
        };
    }

    public static ApplicationInfo FromEntryAssembly()
        => FromAssembly(Assembly.GetEntryAssembly()!);
}
