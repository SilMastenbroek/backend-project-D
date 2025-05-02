

namespace AIWorkflow;

public class ProjectAi
{
    private readonly AiService _ai;

    public ProjectAi(AiService ai)
    {
        _ai = ai;
    }

    public Task<string> CreateThreadAsync() => _ai.CreateThreadAsync();
    public Task AddMessageAsync(string message) => _ai.AddMessageAsync(message);
    public Task<string> RunThreadAsync() => _ai.RunThreadAsync();
    public Task<string> QuickAskAsync(string question) => _ai.AskQuickAsync(question);
    public string GetThreadId() => _ai.GetThreadId();
}