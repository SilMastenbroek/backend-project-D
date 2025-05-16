namespace CommonFnc;

public class ConfigurationHelper
{
    private readonly IConfiguration _config;

    public ConfigurationHelper()
    {
        _config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();
    }

    public string GetFolderPath()
    {
        // TODO: Verwerk dit later in de frontend
        return _config["FolderStructure:Path"];
    }

    public string GetTrelloApiKey() => _config["Trello:ApiKey"];
    public string GetTrelloToken() => _config["Trello:Token"];
    public string GetTrelloBoardId() => _config["Trello:BoardId"];

    public string GetOpenAiApiKey() => _config["OpenAI:ApiKey"];
    public string GetOpenAiAssistantId(string assistantKey) => _config[$"OpenAI:Assistants:{assistantKey}"];
}