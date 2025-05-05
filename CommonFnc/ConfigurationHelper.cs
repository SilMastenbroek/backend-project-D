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
}