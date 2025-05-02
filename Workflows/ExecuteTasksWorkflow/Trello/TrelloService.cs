using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using RestSharp;
using System.Collections.Generic;
using System.Threading.Tasks;

// This service connects to the Trello API and retrieves tasks from a specified board.
// It requires API credentials and a board ID, which are loaded from appsettings.json.
public class TrelloService
{
    private readonly string _apiKey;
    private readonly string _token;
    private readonly string _boardId;
    private readonly RestClient _client;

    // Constructor: initializes the TrelloService with configuration values
    public TrelloService(IConfiguration config)
    {
        _apiKey = config["Trello:ApiKey"];
        _token = config["Trello:Token"];
        _boardId = config["Trello:BoardId"];
        _client = new RestClient("https://api.trello.com/1");
    }

    // Retrieves all tasks from the board, grouped by their list name
    public async Task<List<TrelloTask>> GetAllTasksAsync()
    {
        var taskList = new List<TrelloTask>();

        // Get all lists from the specified board
        var listRequest = new RestRequest($"/boards/{_boardId}/lists", Method.Get)
            .AddParameter("key", _apiKey)
            .AddParameter("token", _token);
        var listResponse = await _client.ExecuteAsync(listRequest);
        if (!listResponse.IsSuccessful) throw new Exception("Failed to load Trello lists");

        var lists = JArray.Parse(listResponse.Content);
        foreach (var list in lists)
        {
            var listId = list["id"]!.ToString();
            var listName = list["name"]!.ToString();

            // Get all cards (tasks) from each list
            var cardsRequest = new RestRequest($"/lists/{listId}/cards", Method.Get)
                .AddParameter("key", _apiKey)
                .AddParameter("token", _token);
            var cardsResponse = await _client.ExecuteAsync(cardsRequest);
            if (!cardsResponse.IsSuccessful) continue;

            var cards = JArray.Parse(cardsResponse.Content);
            foreach (var card in cards)
            {
                var name = card["name"]!.ToString();
                var description = card["desc"]?.ToString();

                // Add each card to the result list
                taskList.Add(new TrelloTask
                {
                    ListName = listName,
                    TaskName = name,
                    Description = description
                });
            }
        }

        return taskList;
    }
}
