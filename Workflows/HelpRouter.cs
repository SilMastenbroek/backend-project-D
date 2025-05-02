public class HelpRouter
{
    public void StartWorkflow(HelpType helpType)
    {
        switch (helpType)
        {
            case HelpType.ExecuteTasks:
                new ExecuteTasksWorkflow().RunAsync();
                break;
            case HelpType.CreateTasks:
                new CreateTasksWorkflow().Run();
                break;
            case HelpType.Debug:
                new DebugWorkflow().Run();
                break;
            case HelpType.Refactor:
                new RefactorWorkflow().Run();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}
