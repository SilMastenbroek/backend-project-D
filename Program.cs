using System;

namespace AiAssistant
{
    // Enum defining the possible types of help the user can request.
    public enum HelpType
    {
        ExecuteTasks,
        CreateTasks,
        Debug,
        Refactor
    }

    class Program
    {
        static void Main(string[] args)
        {
            // Initialize the router responsible for directing to the correct workflow.
            var router = new HelpRouter();
            HelpType selectedHelp;

            // Prompt the user until a valid input is entered.
            while (true)
            {
                Console.WriteLine("\nWhat type of help do you need? (ExecuteTasks, CreateTasks, Debug, Refactor)");
                var input = Console.ReadLine();

                if (Enum.TryParse(input, true, out selectedHelp))
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter one of the following: ExecuteTasks, CreateTasks, Debug, Refactor");
                }
            }

            try
            {
                // Once valid input is given, start the corresponding workflow.
                router.StartWorkflow(selectedHelp);
            }
            catch (NotImplementedException nie)
            {
                Console.WriteLine($"[TODO] {nie.Message}");
            }
        }
    }

    public class HelpRouter
    {
        // Entry point for executing the workflow logic.
        public void StartWorkflow(HelpType helpType)
        {
            switch (helpType)
            {
                case HelpType.ExecuteTasks:
                    ExecuteTasksWorkflow();
                    break;
                case HelpType.CreateTasks:
                    CreateTasksWorkflow();
                    break;
                case HelpType.Debug:
                    DebugWorkflow();
                    break;
                case HelpType.Refactor:
                    RefactorWorkflow();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void ExecuteTasksWorkflow()
        {
            Console.WriteLine("Executing task workflow...");
        }

        private void CreateTasksWorkflow()
        {
            throw new NotImplementedException("Create tasks workflow is not implemented yet.");
        }

        private void DebugWorkflow()
        {
            throw new NotImplementedException("Debug workflow is not implemented yet.");
        }

        private void RefactorWorkflow()
        {
            throw new NotImplementedException("Refactor workflow is not implemented yet.");
        }
    }
}
