using AiAssistantApi.Models;

namespace AiAssistantApi.Services
{
    // Contains the logic for routing to different help workflows
    public class HelpRouter
    {
        public string StartWorkflow(HelpType helpType)
        {
            return helpType switch
            {
                HelpType.ExecuteTasks => ExecuteTasksWorkflow(),
                HelpType.CreateTasks => CreateTasksWorkflow(),
                HelpType.Debug => DebugWorkflow(),
                HelpType.Refactor => RefactorWorkflow(),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        private string ExecuteTasksWorkflow()
        {
            return "Executing task workflow...";
        }

        private string CreateTasksWorkflow()
        {
            throw new NotImplementedException("Create tasks workflow is not implemented yet.");
        }

        private string DebugWorkflow()
        {
            throw new NotImplementedException("Debug workflow is not implemented yet.");
        }

        private string RefactorWorkflow()
        {
            throw new NotImplementedException("Refactor workflow is not implemented yet.");
        }
    }
}
