using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CommonFnc;

public class TrelloTask
{
    public string ListName { get; set; } = "";
    public string TaskName { get; set; } = "";
    public string? Description { get; set; }
}

public static class GetTrelloTask
{
    public static async Task<string> FromConsoleAsync()
    {
        var configHelper = new ConfigurationHelper(); // Haalt data op uit appsettings.json via Helper functie
        var service = new TrelloService(configHelper);
        var task = await service.SelectTaskAsync();

        if (task == null)
            return "Geen taak geselecteerd.";

        string summary = $"[{task.ListName}] {task.TaskName}";
        if (!string.IsNullOrWhiteSpace(task.Description))
            summary += "\nBeschrijving: " + task.Description;

        return summary;
    }
}

// Deze blijft intern, je hoeft hem niet aan te roepen vanuit Run.cs
internal class TrelloService
{
    private readonly string _apiKey;
    private readonly string _token;
    private readonly string _boardId;
    private readonly RestClient _client;

    public TrelloService(ConfigurationHelper config)
    {
        _apiKey = config.GetTrelloApiKey();
        _token = config.GetTrelloToken();
        _boardId = config.GetTrelloBoardId();
        _client = new RestClient("https://api.trello.com/1");
    }

    public async Task<TrelloTask?> SelectTaskAsync()
    {
        var tasks = await GetAllTasksAsync();
        if (tasks.Count == 0)
        {
            Console.WriteLine("Geen Trello-taken gevonden.");
            return null;
        }

        Console.WriteLine("Selecteer een taak:");
        for (int i = 0; i < tasks.Count; i++)
            Console.WriteLine($"{i + 1}. [{tasks[i].ListName}] {tasks[i].TaskName}");

        Console.Write("Keuze: ");
        if (!int.TryParse(Console.ReadLine(), out int choice) || choice < 1 || choice > tasks.Count)
        {
            Console.WriteLine("Ongeldige keuze. Eerste taak geselecteerd.");
            choice = 1;
        }

        return tasks[choice - 1];
    }

    private async Task<List<TrelloTask>> GetAllTasksAsync()
    {
        var taskList = new List<TrelloTask>();

        var listRequest = new RestRequest($"/boards/{_boardId}/lists", Method.Get)
            .AddParameter("key", _apiKey)
            .AddParameter("token", _token);
        var listResponse = await _client.ExecuteAsync(listRequest);
        if (!listResponse.IsSuccessful) throw new Exception("Failed to load Trello lists");

        var lists = JArray.Parse(listResponse.Content!);
        foreach (var list in lists)
        {
            var listId = list["id"]!.ToString();
            var listName = list["name"]!.ToString();

            var cardsRequest = new RestRequest($"/lists/{listId}/cards", Method.Get)
                .AddParameter("key", _apiKey)
                .AddParameter("token", _token);
            var cardsResponse = await _client.ExecuteAsync(cardsRequest);
            if (!cardsResponse.IsSuccessful) continue;

            var cards = JArray.Parse(cardsResponse.Content!);
            foreach (var card in cards)
            {
                taskList.Add(new TrelloTask
                {
                    ListName = listName,
                    TaskName = card["name"]!.ToString(),
                    Description = card["desc"]?.ToString()
                });
            }
        }

        return taskList;
    }
}