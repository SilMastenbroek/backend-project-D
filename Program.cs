using System;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using RestSharp;
using AIWorkflow;

namespace AiAssistant
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // TODO: Kun je hier een test maken die AIQuickChat aanroept? Maak het heel simpel, gewoon een vraag stellen en het antwoord printen. Puur om te testen of de API goed werkt.
                
                var config = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json")
                    .Build();
                
                Console.WriteLine("Test: AIQuickChat");

                // 🔑 Zet hier je OpenAI API key in (of laad uit config)
                string apiKey = config["OpenAI:ApiKey"];

                // Vraag om te stellen
                string question = "Is dit een test van de quickchat?";

                // Initialiseer quickchat en stel een vraag
                var quickchat = new AIQuickChat(apiKey);
                var response = await quickchat.AskAsync(question);

                Console.WriteLine("✅ Antwoord van OpenAI:");
                Console.WriteLine(response);



            // // Initialize the router responsible for directing to the correct workflow.
            // var router = new HelpRouter();
            // HelpType selectedHelp;

            // // Get all values from HelpType enum to display options as a numbered list
            // var helpOptions = Enum.GetValues(typeof(HelpType)).Cast<HelpType>().ToList();
            // Console.WriteLine("\nWhat type of help do you need?");
            // for (int i = 0; i < helpOptions.Count; i++)
            // {
            //     Console.WriteLine($"{i + 1}. {helpOptions[i]}");
            // }

            // // TODO: Now a number is requested (temporarily to test the backend) this is processed in the frontend by a choice menu
            // // Ask the user to choose a number corresponding to the help type
            // while (true)
            // {
            //     Console.Write("\nEnter the number of the help option: ");
            //     var input = Console.ReadLine();

            //     if (int.TryParse(input, out int index) &&
            //         index >= 1 &&
            //         index <= helpOptions.Count)
            //     {
            //         selectedHelp = helpOptions[index - 1];
            //         break;
            //     }

            //     Console.WriteLine("Invalid input. Please enter a number from the list.");
            // }

            // try
            // {
            //     // Once valid input is given, start the corresponding workflow.
            //     await router.StartWorkflow(selectedHelp);
            // }
            // catch (NotImplementedException nie)
            // {
            //     Console.WriteLine($"[TODO] {nie.Message}");
            // }
        }
    }
}
