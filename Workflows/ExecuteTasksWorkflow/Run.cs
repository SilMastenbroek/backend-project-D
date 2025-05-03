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
            await AssistantSetup.InitAsync("Execute");
            var assistant = AssistantSetup.AssistantInstance;
            var threadId = AssistantSetup.ThreadId;

            Console.WriteLine("Thread ID: " + threadId + "\n");

            //Vraag folderstructuur op via console
            string folder = GetFolderStructure.FromConsole();

            //Vraag Trello-taak op via console
            string task = await GetTrelloTask.FromConsoleAsync();

            //Voeg context toe aan de AI-thread
            await assistant.AddMessageAsync("Folderstructuur:\n" + folder);
            await assistant.AddMessageAsync("Trello taak:\n" + task);
            await assistant.AddMessageAsync("Zullen we samen deze taak aanpakken?");

            //Start de AI-run
            string reactie = await assistant.RunAsync();

            Console.WriteLine("\nAI Reactie:\n" + reactie);
        }
    }
}


