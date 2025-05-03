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

    public static string FromConsole()
    {
        Console.Write("Voer het projectpad in: ");
        var input = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(input) || !Directory.Exists(input))
        {
            Console.WriteLine("Ongeldig pad.");
            return "";
        }

        return BuildTree(input);
    }

    public static string BuildTree(string rootPath)
    {
        var sb = new StringBuilder();
        var rootName = new DirectoryInfo(rootPath).Name;
        sb.AppendLine($"/{rootName}/");
        Traverse(rootPath, sb, indentLevel: 1);
        return sb.ToString();
    }

    private static void Traverse(string path, StringBuilder sb, int indentLevel)
    {
        var indent = new string(' ', indentLevel * 4);
        var dir = new DirectoryInfo(path);

        if (IgnoredFolders.Contains(dir.Name)) return;

        // Subfolders
        foreach (var subDir in dir.GetDirectories())
        {
            if (IgnoredFolders.Contains(subDir.Name)) continue;

            sb.AppendLine($"{indent}{subDir.Name}/");
            Traverse(subDir.FullName, sb, indentLevel + 1);
        }

        // Files
        foreach (var file in dir.GetFiles())
        {
            if (IgnoredExtensions.Contains(file.Extension)) continue;

            sb.AppendLine($"{indent}{file.Name}");
        }
    }
}
