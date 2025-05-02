using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AIWorkflow;

public class AIAssistant : IAsyncDisposable
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;
    private readonly string _assistantId;
    private string _threadId;

    public string CurrentThreadId => _threadId;

    public AIAssistant(string apiKey, string assistantId)
    {
        _apiKey = apiKey;
        _assistantId = assistantId;
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri("https://api.openai.com/v1/")
        };
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);
    }

    public async Task<string> CreateThreadAsync()
    {
        var response = await _httpClient.PostAsync("threads", new StringContent("{}", Encoding.UTF8, "application/json"));
        var json = await response.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(json);
        _threadId = doc.RootElement.GetProperty("id").GetString();
        return _threadId;
    }

    public async Task AddMessageAsync(string content)
    {
        if (_threadId == null) throw new Exception("Thread not initialized");
        var payload = new { role = "user", content };
        var body = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
        await _httpClient.PostAsync($"threads/{_threadId}/messages", body);
    }

    public async Task<string> RunAsync()
    {
        if (_threadId == null) throw new Exception("Thread not initialized");

        var runPayload = new { assistant_id = _assistantId };
        var runBody = new StringContent(JsonSerializer.Serialize(runPayload), Encoding.UTF8, "application/json");
        var runRes = await _httpClient.PostAsync($"threads/{_threadId}/runs", runBody);
        var runJson = await runRes.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(runJson);
        var runId = doc.RootElement.GetProperty("id").GetString();

        while (true)
        {
            await Task.Delay(1000);
            var statusRes = await _httpClient.GetAsync($"threads/{_threadId}/runs/{runId}");
            var statusJson = await statusRes.Content.ReadAsStringAsync();
            using var statusDoc = JsonDocument.Parse(statusJson);
            var status = statusDoc.RootElement.GetProperty("status").GetString();

            if (status == "completed") break;
            if (status == "failed") throw new Exception("Run failed.");
        }

        var messagesRes = await _httpClient.GetAsync($"threads/{_threadId}/messages");
        var messagesJson = await messagesRes.Content.ReadAsStringAsync();
        using var msgDoc = JsonDocument.Parse(messagesJson);
        return msgDoc.RootElement.GetProperty("data")[0].GetProperty("content")[0].GetProperty("text").GetProperty("value").GetString();
    }

    public async ValueTask DisposeAsync()
    {
        if (_threadId != null)
        {
            await _httpClient.DeleteAsync($"threads/{_threadId}");
            _threadId = null;
        }
        _httpClient.Dispose();
    }
}
