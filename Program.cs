using System;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using RestSharp;


namespace AiAssistant
{
    // Enum defining the possible types of help the user can request.
    public enum HelpType
    {
        ExecuteTasks,
        CreateTasks,
        Debug,
        Refactor
    }

    class Program
    {
        static void Main(string[] args)
        {
            // Initialize the router responsible for directing to the correct workflow.
            var router = new HelpRouter();
            HelpType selectedHelp;

            // Get all values from HelpType enum to display options as a numbered list
            var helpOptions = Enum.GetValues(typeof(HelpType)).Cast<HelpType>().ToList();
            Console.WriteLine("\nWhat type of help do you need?");
            for (int i = 0; i < helpOptions.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {helpOptions[i]}");
            }

            // TODO: Now a number is requested (temporarily to test the backend) this is processed in the frontend by a choice menu
            // Ask the user to choose a number corresponding to the help type
            while (true)
            {
                Console.Write("\nEnter the number of the help option: ");
                var input = Console.ReadLine();

                if (int.TryParse(input, out int index) &&
                    index >= 1 &&
                    index <= helpOptions.Count)
                {
                    selectedHelp = helpOptions[index - 1];
                    break;
                }

                Console.WriteLine("Invalid input. Please enter a number from the list.");
            }

            try
            {
                // Once valid input is given, start the corresponding workflow.
                router.StartWorkflow(selectedHelp);
            }
            catch (NotImplementedException nie)
            {
                Console.WriteLine($"[TODO] {nie.Message}");
            }
        }
    }

    public class HelpRouter
    {
        // Entry point for executing the workflow logic.
        public void StartWorkflow(HelpType helpType)
        {
            switch (helpType)
            {
                case HelpType.ExecuteTasks:
                    ExecuteTasksWorkflow();
                    break;
                case HelpType.CreateTasks:
                    CreateTasksWorkflow();
                    break;
                case HelpType.Debug:
                    DebugWorkflow();
                    break;
                case HelpType.Refactor:
                    RefactorWorkflow();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void ExecuteTasksWorkflow()
        {
            Console.WriteLine("Executing task workflow...");
            DisplayAndSelectTrelloTaskAsync().GetAwaiter().GetResult();
        }

        // Retrieves tasks from a Trello board and lets the user select one
        private async Task DisplayAndSelectTrelloTaskAsync()
        {

            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            // TODO: Process in frontend
            var apiKey = config["Trello:ApiKey"];
            var token = config["Trello:Token"];
            var boardId = config["Trello:BoardId"];

            var client = new RestClient("https://api.trello.com/1");

            // Get all lists from the Trello board
            var listRequest = new RestRequest($"/boards/{boardId}/lists", Method.Get);
            listRequest.AddParameter("key", apiKey);
            listRequest.AddParameter("token", token);
            var listResponse = await client.ExecuteAsync(listRequest);

            if (!listResponse.IsSuccessful)
            {
                Console.WriteLine("Error retrieving lists: " + listResponse.ErrorMessage);
                return;
            }

            var lists = JArray.Parse(listResponse.Content);
            var taskList = new List<(string TaskName, string? Description)>();
            int taskNumber = 1;

            // Iterate through each list and retrieve its cards (tasks)
            foreach (var list in lists)
            {
                string listId = list["id"]!.ToString();
                string listName = list["name"]!.ToString();

                Console.WriteLine($"\nList: {listName}");

                var cardsRequest = new RestRequest($"/lists/{listId}/cards", Method.Get);
                cardsRequest.AddParameter("key", apiKey);
                cardsRequest.AddParameter("token", token);
                var cardsResponse = await client.ExecuteAsync(cardsRequest);

                if (!cardsResponse.IsSuccessful)
                {
                    Console.WriteLine("  (Error retrieving cards)");
                    continue;
                }

                var cards = JArray.Parse(cardsResponse.Content);
                if (cards.Count == 0)
                {
                    Console.WriteLine("  (No tasks in this list)");
                    continue;
                }

                // TODO: Now a number is added to the tasks (temporarily to test the backend) this will be picked up later in the frontend
                // Add each card to the task list and display it with a number
                foreach (var card in cards)
                {
                    string name = card["name"]!.ToString();
                    string description = card["desc"]?.ToString();

                    Console.WriteLine($"{taskNumber}. {name}" + (!string.IsNullOrWhiteSpace(description) ? $" ({description})" : ""));
                    taskList.Add((name, description));
                    taskNumber++;
                }
            }

            if (taskList.Count == 0)
            {
                Console.WriteLine("No tasks available.");
                return;
            }

            // TODO: Now a number is requested (temporarily to test the backend) this is processed in the frontend by a choice menu
            // Let user choose a task by number
            int selectedIndex = -1;
            while (true)
            {
                Console.Write("\nEnter the number of the task you want help with: ");
                var input = Console.ReadLine();

                if (int.TryParse(input, out selectedIndex) &&
                    selectedIndex >= 1 &&
                    selectedIndex <= taskList.Count)
                {
                    var selectedTask = taskList[selectedIndex - 1];
                    Console.WriteLine($"\nI will now help you with the task: {selectedTask.TaskName}");
                    if (!string.IsNullOrWhiteSpace(selectedTask.Description))
                    {
                        Console.WriteLine($"Task description: {selectedTask.Description}");
                    }
                    break;
                }

                Console.WriteLine("Invalid choice. Please enter a valid task number.");
            }
        }

        private void CreateTasksWorkflow()
        {
            throw new NotImplementedException("Create tasks workflow is not implemented yet.");
        }

        private void DebugWorkflow()
        {
            throw new NotImplementedException("Debug workflow is not implemented yet.");
        }

        private void RefactorWorkflow()
        {
            throw new NotImplementedException("Refactor workflow is not implemented yet.");
        }
    }
}
