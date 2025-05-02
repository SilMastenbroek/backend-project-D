using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;

// This class handles extracting a zip file and capturing the folder structure as a formatted string.
public class ZipFolderExplorer
{
    public static string ShowStructure(string zipPath)
    {
        string extractPath = Path.Combine(Path.GetTempPath(), "unzipped_project_" + Guid.NewGuid());
        ZipFile.ExtractToDirectory(zipPath, extractPath);

        var rootDirs = Directory.GetDirectories(extractPath);
        string projectRoot = rootDirs.Length == 1 ? rootDirs[0] : extractPath;

        var sb = new StringBuilder();
        sb.AppendLine($"{Path.GetFileName(projectRoot)}/");
        BuildTree(projectRoot, "", sb);
        return sb.ToString();
    }

    private static void BuildTree(string startPath, string prefix, StringBuilder sb)
    {
        var entries = Directory.GetFileSystemEntries(startPath)
            .OrderBy(e => Directory.Exists(e) ? 0 : 1)
            .ThenBy(Path.GetFileName)
            .ToList();

        for (int i = 0; i < entries.Count; i++)
        {
            var path = entries[i];
            var isLast = i == entries.Count - 1;
            var connector = isLast ? "└── " : "├── ";
            sb.AppendLine(prefix + connector + Path.GetFileName(path));

            if (Directory.Exists(path))
            {
                var extension = isLast ? "    " : "│   ";
                BuildTree(path, prefix + extension, sb);
            }
        }
    }
}
