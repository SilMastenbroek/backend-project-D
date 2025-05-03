
namespace AIWorkflow;
public class AiService
{
    private readonly AIAssistant _assistant;
    private readonly AIQuickChat _quick;

    public AiService(string apiKey, string assistantId)
    {
        _assistant = new AIAssistant(apiKey, assistantId);
        _quick = new AIQuickChat(apiKey);
    }

    // Thread-based assistant
    public async Task<string> CreateThreadAsync() => await _assistant.CreateThreadAsync();
    public async Task AddMessageAsync(string message) => await _assistant.AddMessageAsync(message);
    public async Task<string> RunThreadAsync() => await _assistant.RunAsync();

    // Quick chat
    public async Task<string> AskQuickAsync(string question) => await _quick.AskAsync(question);
}
