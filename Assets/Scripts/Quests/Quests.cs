using System.Collections.Generic;

// This enum represents the possible status of a quest.
public enum QuestStatus
{
    NotStarted, // Indicates that the quest has not been started yet.
    InProgress, // Indicates that the quest is currently in progress.
    Completed   // Indicates that the quest has been completed.
}

// This class represents a condition that must be fulfilled for a quest.
public class QuestCondition
{
    // The name of the condition.
    public string ConditionName { get; set; }

    // The index of the condition.
    public int ConditionIndex { get; set; }

    // A boolean indicating whether the condition is fulfilled or not.
    public bool IsFulfilled { get; set; }

    public string ItemConditionName;

    public int quantity;

    //! If a condition need another condition to be validate first
    public int[] LinkConditionsIndexes;

    // Constructor to initialize the condition with a given name.
    public QuestCondition()
    {
        IsFulfilled = false; // By default, the condition is not fulfilled.
        LinkConditionsIndexes = new int[] { -1 }; // By default, no link condition
    }

}

// This class represents a quest in the game.
public class Quest
{
    // The name of the quest.
    public string NameUI;
    public string Name;
    public int QuestID;
    public string Description;
    // The status of the quest (NotStarted, InProgress, Completed).
    public QuestStatus Status;

    // The list of conditions associated with the quest.
    public List<QuestCondition> Conditions { get; private set; }

    // Constructor to initialize the quest with a given name.
    public Quest(string name)
    {
        NameUI = name;
        Name = name.Replace(" ", "");
        Status = QuestStatus.NotStarted; // By default, the quest status is NotStarted.
        Conditions = new List<QuestCondition>(); // Initialize the list of conditions.
        Description = "No description"; // By default, display no description
    }

    public void UpdateStatus(QuestStatus newStatus)
    {
        Status = newStatus;
    }
}
