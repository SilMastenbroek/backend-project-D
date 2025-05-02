using Newtonsoft.Json.Linq;
using RestSharp;
using Microsoft.Extensions.Configuration;

public class ExecuteTasksWorkflow
{
    public void Run()
    {
        Console.WriteLine("Executing task workflow...");
        DisplayAndSelectTrelloTaskAsync().GetAwaiter().GetResult();
    }

    private async Task DisplayAndSelectTrelloTaskAsync()
    {
        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

        var apiKey = config["Trello:ApiKey"];
        var token = config["Trello:Token"];
        var boardId = config["Trello:BoardId"];

        var client = new RestClient("https://api.trello.com/1");
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

            foreach (var card in cards)
            {
                string name = card["name"]!.ToString();
                string description = card["desc"]?.ToString();

                Console.WriteLine($"{taskNumber}. {name}" +
                    (!string.IsNullOrWhiteSpace(description) ? $" ({description})" : ""));

                taskList.Add((name, description));
                taskNumber++;
            }
        }

        if (taskList.Count == 0)
        {
            Console.WriteLine("No tasks available.");
            return;
        }

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
}
