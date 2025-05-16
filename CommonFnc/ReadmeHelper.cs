using System;
using System.IO;

namespace CommonFnc
{
    public static class ReadmeHelper
    {
        public static string? GetReadmeContents(string folderPath)
        {
            var files = Directory.GetFiles(folderPath);
            foreach (var file in files)
            {
                var fileName = Path.GetFileName(file).ToLower();
                if (fileName == "readme.md" || fileName == "readme.txt" || fileName == "readme")
                {
                    return File.ReadAllText(file);
                }
            }

            Console.WriteLine("📭 Geen README-bestand gevonden in de map.");
            return null;
        }
    }
}
