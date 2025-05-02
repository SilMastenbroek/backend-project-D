using Microsoft.Extensions.Configuration;
using System.IO.Compression;

// This class handles the execution of tasks selected by the user from Trello.
public class ExecuteTasksWorkflow
{
    public async Task RunAsync()
    {
        Console.WriteLine("Executing task workflow...");

        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

        var trello = new TrelloService(config);
        var taskList = trello.GetAllTasksAsync().GetAwaiter().GetResult();

        if (taskList.Count == 0)
        {
            Console.WriteLine("No tasks available.");
            return;
        }

        var grouped = taskList.GroupBy(t => t.ListName);
        int number = 1;
        var flatList = new List<TrelloTask>();

        foreach (var group in grouped)
        {
            Console.WriteLine($"\nList: {group.Key}");
            foreach (var task in group)
            {
                Console.WriteLine($"{number++}. {task.TaskName}" +
                    (!string.IsNullOrWhiteSpace(task.Description) ? $" ({task.Description})" : ""));
                flatList.Add(task);
            }
        }

        while (true)
        {
            Console.Write("\nEnter the number of the task you want help with: ");
            var input = Console.ReadLine();

            if (int.TryParse(input, out int index) && index >= 1 && index <= flatList.Count)
            {
                var selectedTask = flatList[index - 1];
                Console.WriteLine($"\nI will now help you with the task: {selectedTask.TaskName}");
                if (!string.IsNullOrWhiteSpace(selectedTask.Description))
                {
                    Console.WriteLine($"Task description: {selectedTask.Description}");
                }

                string folderStructure = ZipFolderExplorer.ShowStructure("test-project.zip");
                string combinedTask = selectedTask.TaskName +
                    (string.IsNullOrWhiteSpace(selectedTask.Description) ? "" : "\n" + selectedTask.Description);

                // TODO: later uitwerken met AIService
                // var aiService = new AiService();
                // var project = new ProjectAiBoot(aiService, folderStructure, combinedTask);
                var project = new ProjectAiBoot(folderStructure, combinedTask);
                await project.StartAsync(); // 🔥 start communicatie met AI

                break;
            }

            Console.WriteLine("That task number does not exist. Please choose a valid task.");
        }
    }
}
