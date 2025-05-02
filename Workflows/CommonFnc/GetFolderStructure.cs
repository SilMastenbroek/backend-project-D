using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace CommonFnc;

public static class GetFolderStructure
{
    private static readonly HashSet<string> IgnoredFolders = new(StringComparer.OrdinalIgnoreCase)
    {
        "bin", "obj", ".git", "node_modules", ".vs", ".vscode", "dist", "build", "out", "__pycache__", ".idea"
    };

    private static readonly HashSet<string> IgnoredExtensions = new(StringComparer.OrdinalIgnoreCase)
    {
        ".log", ".lock", ".tmp", ".cache", ".exe", ".dll", ".pdb", ".zip", ".rar", ".7z", ".tar", ".db", ".iso",
        ".jpg", ".jpeg", ".png", ".gif", ".mp4", ".mp3", ".webp", ".pdf", ".ico"
    };

    public static string Get(string rootPath)
    {
        if (!Directory.Exists(rootPath))
            throw new DirectoryNotFoundException($"Pad bestaat niet: {rootPath}");

        var sb = new StringBuilder();
        BuildTree(rootPath, sb, "");
        return sb.ToString();
    }

    public static string FromConsole()
    {
        Console.Write("Voer het projectpad in: ");
        string? inputPath = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(inputPath) || !Directory.Exists(inputPath))
        {
            Console.WriteLine("Ongeldig pad. Gebruik standaardpad: C:\\MijnProject");
            inputPath = @"C:\MijnProject";
        }

        return Get(inputPath);
    }

    private static void BuildTree(string path, StringBuilder sb, string indent)
    {
        var dir = new DirectoryInfo(path);
        if (IgnoredFolders.Contains(dir.Name)) return;

        sb.AppendLine(indent + "/" + dir.Name);

        foreach (var subDir in dir.GetDirectories())
            BuildTree(subDir.FullName, sb, indent + "  ");

        foreach (var file in dir.GetFiles())
        {
            if (!IgnoredExtensions.Contains(file.Extension))
                sb.AppendLine(indent + "  - " + file.Name);
        }
    }
}
