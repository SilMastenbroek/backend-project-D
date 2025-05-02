using System;
using System.Threading.Tasks;
using CreateTasksWorkflow;
using DebugWorkflow;
using ExecuteTasksWorkflow;
using RefactorWorkflow;

class Program
{
    static async Task Main()
    {
        Console.WriteLine("Welke workflow wil je starten?");
        Console.WriteLine("1. Execute Task");
        Console.WriteLine("2. Debug - werkt niet");
        Console.WriteLine("3. Create Task - werkt niet");
        Console.WriteLine("4. Refactor - werkt niet");
        Console.Write("Keuze: ");

        var input = Console.ReadLine();

        switch (input)
        {
            case "1":
                await RunExecute();
                break;
            default:
                Console.WriteLine("Ongeldige keuze.");
                break;
        }
    }

    //static Task RunCreateTask() => CreateTasksWorkflow.Run.CreateAsync();
    //static Task RunDebug() => DebugWorkflow.Run.StartAsync();
    static Task RunExecute() => ExecuteTasksWorkflow.Run.StartAsync();
   // static Task RunRefactor() => Run.StartAsync();
}