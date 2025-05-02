using System.Threading.Tasks;
using AIWorkflow;

namespace ExecuteTasksWorkflow
{
    public static class AssistantSetup
    {
        public static AIAssistant AssistantInstance { get; private set; }
        public static string AssistantId { get; private set; }
        public static string ThreadId { get; private set; }

        public static async Task InitAsync()
        {

            var ai = new AIAssistant(apiKey, AssistantId);
            ThreadId = await ai.CreateThreadAsync();

            // Standaard instructies voor deze workflow
            string instructions = string.Join("\n", new[]
            {
                "Je bent een technische AI-assistent.",
                "Help met het uitvoeren van programmeertaken.",
                "Wees kort, helder en technisch precies.",
                "Vraag door als context ontbreekt."
            });

            await ai.AddMessageAsync("📌 Workflow instructies:\n" + instructions);

            AssistantInstance = ai;
        }
    }
}

