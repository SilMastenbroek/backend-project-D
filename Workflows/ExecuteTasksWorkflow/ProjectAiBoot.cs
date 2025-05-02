public class ProjectAiBoot
{
    // private readonly AiService _ai;
    private readonly string _folderStructure;
    private readonly string _task;

    // public ProjectAiBoot(AiService aiService, string folderStructure, string task)
    // {
    //     _ai = aiService;
    //     _folderStructure = folderStructure;
    //     _task = task;
    // }

    public ProjectAiBoot(string folderStructure, string task)
    {
        _folderStructure = folderStructure;
        _task = task;
    }

    public async Task StartAsync()
    {
        Console.Clear();
        // await _ai.InitAssistantAsync();

        // 📁 Geef de AI de projectstructuur
        System.Console.WriteLine(_folderStructure);
        // await _ai.AddContextAsync("Hier is de folderstructuur van het project:\n" + _folderStructure);

        // ✅ Geef de AI de taak
        System.Console.WriteLine(_task);
        // await _ai.AddContextAsync("De taak vanuit Trello:\n" + _task);

        // 🗣️ Begin de conversatie in natuurlijke taal
        // await _ai.AddTaskMessageAsync("Hey, we gaan samen aan deze taak werken. Denk je dat je hier al iets over kunt zeggen, of moet ik eerst wat extra context geven?");

        // var reactie = await _ai.RunAssistantAsync();

        // Console.WriteLine("Assistant zegt:\n" + reactie);
    }
}