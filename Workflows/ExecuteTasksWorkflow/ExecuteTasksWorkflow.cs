using Microsoft.Extensions.Configuration;
using System.IO.Compression;

// This class handles the execution of tasks selected by the user from Trello.
// It fetches tasks from Trello, allows the user to select one, so that our Assistant can help with the task of the project
public class ExecuteTasksWorkflow
{
    public void Run()
    {
        Console.WriteLine("Executing task workflow...");

        // Load Trello credentials and board ID from configuration file.
        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

        var trello = new TrelloService(config);

        // Fetch all Trello tasks grouped by list
        var taskList = trello.GetAllTasksAsync().GetAwaiter().GetResult();

        if (taskList.Count == 0)
        {
            Console.WriteLine("No tasks available.");
            return;
        }

        // Display tasks grouped by Trello list
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
        
        // Allow user to select a task number until a valid choice is made
        while (true)
        {
            Console.Write("\nEnter the number of the task you want help with: ");
            var input = Console.ReadLine();

            if (int.TryParse(input, out int index) &&
                index >= 1 &&
                index <= flatList.Count)
            {
                var selectedTask = flatList[index - 1];
                Console.WriteLine($"\nI will now help you with the task: {selectedTask.TaskName}");
                if (!string.IsNullOrWhiteSpace(selectedTask.Description))
                {
                    Console.WriteLine($"Task description: {selectedTask.Description}");
                }

                // Display the folder structure of the uploaded zip file
                ZipFolderExplorer.ShowStructure("test-project.zip");
                break;
            }

            Console.WriteLine("That task number does not exist. Please choose a valid task.");
        }
    }
}
