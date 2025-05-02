using System;
using System.Threading.Tasks;
using AIWorkflow;
using CommonFnc;

namespace ExecuteTasksWorkflow
{
    public static class Run
    {
        public static async Task StartAsync()
        {
            Console.WriteLine("Execute Task Workflow gestart...\n");

            // 🧠 Initieer AI-assistent en thread
            await AssistantSetup.InitAsync();
            var ai = AssistantSetup.AssistantInstance;
            var threadId = AssistantSetup.ThreadId;

            Console.WriteLine("Thread ID: " + threadId + "\n");

            // 📁 Vraag folderstructuur op via console
            string folder = GetFolderStructure.FromConsole();

            // ✅ Vraag Trello-taak op via console
            string task = await GetTrelloTask.FromConsoleAsync();

            // ➕ Voeg context toe aan de AI-thread
            await ai.AddMessageAsync("Folderstructuur:\n" + folder);
            await ai.AddMessageAsync("Trello taak:\n" + task);
            await ai.AddMessageAsync("Zullen we samen deze taak aanpakken?");

            // 🚀 Start de AI-run
            string reactie = await ai.RunAsync();

            Console.WriteLine("\n🤖 AI Reactie:\n" + reactie);
        }
    }
}


