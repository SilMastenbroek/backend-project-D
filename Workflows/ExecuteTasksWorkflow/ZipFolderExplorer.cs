using System.IO.Compression;

public class ZipFolderExplorer
{
    public static void ShowStructure(string zipPath)
    {
        string extractPath = Path.Combine(Path.GetTempPath(), "unzipped_project_" + Guid.NewGuid());
        ZipFile.ExtractToDirectory(zipPath, extractPath);

        var rootDirs = Directory.GetDirectories(extractPath);
        string projectRoot = rootDirs.Length == 1 ? rootDirs[0] : extractPath;

        Console.WriteLine($"\nProject Folder Structure:");
        Console.WriteLine($"{Path.GetFileName(projectRoot)}/");
        PrintTree(projectRoot, "");
    }

    private static void PrintTree(string path, string prefix)
    {
        var entries = Directory.GetFileSystemEntries(path)
                               .OrderBy(e => Directory.Exists(e) ? 0 : 1)
                               .ThenBy(Path.GetFileName)
                               .ToList();

        for (int i = 0; i < entries.Count; i++)
        {
            var item = entries[i];
            var isLast = i == entries.Count - 1;
            var connector = isLast ? "└── " : "├── ";
            Console.WriteLine(prefix + connector + Path.GetFileName(item));
            if (Directory.Exists(item))
            {
                var ext = isLast ? "    " : "│   ";
                PrintTree(item, prefix + ext);
            }
        }
    }
}
