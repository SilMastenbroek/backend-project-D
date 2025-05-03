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
        Traverse(rootPath, sb, "", true);
        return sb.ToString();
    }

    private static void Traverse(string path, StringBuilder sb, string indent, bool isLast)
    {
        var dir = new DirectoryInfo(path);
        if (IgnoredFolders.Contains(dir.Name)) return;

        sb.AppendLine($"{indent}{(isLast ? "└──" : "├──")} {dir.Name}");
        indent += isLast ? "    " : "│   ";

        var subDirs = dir.GetDirectories();
        var files = dir.GetFiles();

        int fileCount = 0;
        for (int i = 0; i < subDirs.Length; i++)
        {
            Traverse(subDirs[i].FullName, sb, indent, i == subDirs.Length - 1 && files.Length == 0);
        }

        for (int i = 0; i < files.Length; i++)
        {
            if (IgnoredExtensions.Contains(files[i].Extension)) continue;

            var prefix = (i == files.Length - 1) ? "└──" : "├──";
            sb.AppendLine($"{indent}{prefix} {files[i].Name}");
            fileCount++;
        }
    }
}
