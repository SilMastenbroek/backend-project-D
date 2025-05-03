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

            // Loop die blijft draaien zolang de gebruiker "ja" invoert
            while (true)
            {
                Console.WriteLine("\nReageer met 'ja' om door te gaan of 'nee' om te stoppen:");
                var reactie_gebruiker = Console.ReadLine()?.Trim().ToLower();

                if (reactie_gebruiker == "nee")
                {
                    Console.WriteLine("Programma gestopt.");
                    break;
                }
                else if (reactie_gebruiker == "ja")
                {
                    await assistant.AddMessageAsync(reactie_gebruiker);

                    string reactie2 = await assistant.RunAsync();
                    Console.WriteLine("\nAI Reactie:\n" + reactie2);
                }
                else
                {
                    Console.WriteLine("Ongeldige invoer. Reageer met 'ja' of 'nee'.");
                }
            }


            // TODO: Hier uit uit bovenstaande reactie komt de eerst reactie van de chat. De chat kan vragen stellen waarop ja en nee geantwoord kan worden zoals: AI Reactie: Op basis van de folderstructuur en de Trello-taak lijkt het erop dat we moeten controleren of er een README-bestand aanwezig is in de geselecteerde map. Als dat zo is, moeten we de inhoud gebruiken om een samenvatting te genereren via de quickchat. Als er geen README is, moeten we de gebruiker om meer context vragen. Als je akkoord bent, geef ik je per stap aan wat je moet doen om dit op te lossen. Kun je een loopje maken die blijft lopen zolang de gebruiker na een reactie "ja" reageerd, dan moet onderstaande code steeds weer uitgevoegd. Reageert de gebruiker "nee" stopt het programma

            // var reactie_gebruiker = Console.ReadLine();
            // await assistant.AddMessageAsync(reactie_gebruiker);

            // string reactie2 = await assistant.RunAsync();
            // Console.WriteLine("\nAI Reactie:\n" + reactie2);
        }
    }
}


