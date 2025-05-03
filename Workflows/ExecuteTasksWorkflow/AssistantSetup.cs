using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using AIWorkflow;

namespace ExecuteTasksWorkflow;

public static class AssistantSetup
{
    public static AIAssistant AssistantInstance { get; private set; }
    public static string AssistantId { get; private set; }
    public static string ThreadId { get; private set; }

    public static async Task InitAsync(string assistantKey)
    {
        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

        var apiKey = config["OpenAI:ApiKey"];
        AssistantId = config[$"OpenAI:Assistants:{assistantKey}"];

        if (string.IsNullOrWhiteSpace(AssistantId))
            throw new Exception($"Assistant ID '{assistantKey}' niet gevonden in config.");

        var assistant = new AIAssistant(apiKey, AssistantId);
        ThreadId = await assistant.CreateThreadAsync();
        AssistantInstance = assistant;
    }
}
