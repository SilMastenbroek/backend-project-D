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
            // string reactie = await assistant.RunAsync();
             // TODO: Verwijder deze regel in productie, dit is een test om te kijken wat er gebeurt als de AI om class & method lines vraagt. Hierna kan bovenstaande weer aan
            string reactie = "AI.RequestContext.Class & Method Lines";
            Console.WriteLine("AI Reactie:\n" + reactie);

            // Check of gevoelige data aangevraagd wordt
            if (reactie.Contains("AI.RequestContext.Class & Method Lines"))
            {
                Console.WriteLine("\nDe AI vraagt toestemming om class & method lines te gebruiken, mag dit?");
                var toestemming = Console.ReadLine();

                // Contextuele prompt voor quickchat
                string toestemmingPrompt = "Het volgende bericht geeft de gebruiker aan jou om toestemming te geven om class & method lines te gebruiken. Jij moet hieruit concluderen of de gebruiker toestemming geeft. Andwoord met ja als de gebruiker toestemming geeft en antwoord nee als de gebruiker geen toestemming geeft, LET OP VOEG NIKS EXTRA's TOE ALLEEN JA OF NEE ALS ANTWOORD. Hier het response van de gebruiker: " + toestemming;

                // Gebruik AIQuickChat
                var config = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json")
                    .Build();
                var quickchat = new AIQuickChat(config["OpenAI:ApiKey"]);
                string quickReply = await quickchat.AskAsync(toestemmingPrompt);
                string response = quickReply.Trim().ToLower();

                if (response.Contains("ja") || response.Contains("yes") || response.Contains("akkoord") || response.Contains("ok"))
                {
                    Console.WriteLine("Toestemming gegeven. Context wordt gedeeld met AI-assistent.");

                    // TODO: hier voeg je de echte context toe (vervang placeholder)
                    // string classInfo = "public class Voorbeeld { void Methode() { ... } }";
                    // await assistant.AddMessageAsync("Class & method lines:\n" + classInfo);
                }
                else
                {
                    Console.WriteLine("Geen toestemming. Context wordt niet gedeeld.");
                }
            }

            // TODO deze loop kan je gebruiken om de gebruiker te vragen of ze verder willen gaan met de AI-assistent
            // Loop die blijft draaien zolang de gebruiker "ja" invoert
            // while (true)
            // {
            //     Console.WriteLine("\nReageer met 'ja' om door te gaan of 'nee' om te stoppen:");
            //     var reactie_gebruiker = Console.ReadLine()?.Trim().ToLower();

            //     if (reactie_gebruiker == "nee")
            //     {
            //         Console.WriteLine("Programma gestopt.");
            //         break;
            //     }
            //     else if (reactie_gebruiker == "ja")
            //     {
            //         await assistant.AddMessageAsync(reactie_gebruiker);

            //         string reactie2 = await assistant.RunAsync();
            //         Console.WriteLine("\nAI Reactie:\n" + reactie2);
            //     }
            //     else
            //     {
            //         Console.WriteLine("Ongeldige invoer. Reageer met 'ja' of 'nee'.");
            //     }
            // }


            // TODO: Hier kan je de AIQuickChat functionaliteit aanroepen als dat nodig is

            // var config = new ConfigurationBuilder()
            //     .AddJsonFile("appsettings.json")
            //     .Build();
            
            // Console.WriteLine("Test: AIQuickChat");

            // // 🔑 Zet hier je OpenAI API key in (of laad uit config)
            // string apiKey = config["OpenAI:ApiKey"];

            // // Vraag om te stellen
            // string question = "Is dit een test van de quickchat?";

            // // Initialiseer quickchat en stel een vraag
            // var quickchat = new AIQuickChat(apiKey);
            // var response = await quickchat.AskAsync(question);

            // Console.WriteLine("✅ Antwoord van OpenAI:");
            // Console.WriteLine(response);
        }
    }
}


