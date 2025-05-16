using System.Threading.Tasks;

public class HelpRouter
{
   public async Task StartWorkflow(HelpType helpType)
   {
       switch (helpType)
       {
           case HelpType.ExecuteTasks:
               await ExecuteTasksWorkflow.Run.StartAsync();
               break;
           case HelpType.CreateTasks:
               throw new NotImplementedException("Create tasks workflow is not implemented yet.");
           case HelpType.Debug:
               throw new NotImplementedException("Debug workflow is not implemented yet.");
           case HelpType.Refactor:
               throw new NotImplementedException("Refactor workflow is not implemented yet.");
           default:
               throw new ArgumentOutOfRangeException();
       }
   }
}
