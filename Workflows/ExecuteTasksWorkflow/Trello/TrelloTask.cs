// Represents a task from Trello, including the list it belongs to.
public class TrelloTask
{
    // The name of the Trello list
    public string ListName { get; set; } = "";

    // The title of the task/card on the board
    public string TaskName { get; set; } = "";

    // Optional description of the task
    public string? Description { get; set; }
}
