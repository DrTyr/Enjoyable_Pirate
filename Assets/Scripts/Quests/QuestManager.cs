using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class QuestManager : MonoBehaviour
{
    private QuestDB questDB;
    //private Inventory inventory;
    private static QuestManager instance;

    void Start()
    {
        questDB = QuestDB.GetInstance();

        //inventory = Inventory.GetInstance();

    }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Found more than one Inventory in the scene");
        }
        instance = this;
    }

    public static QuestManager GetInstance()
    {
        return instance;
    }

    void Update()
    {
        //! Check every if the conditions are fullfilled
        TestAllQuestsForFullfiledConditons();
    }


    public List<Quest> GetAllQuestsInProgress()
    {
        List<Quest> questsInProgress = new List<Quest>();

        foreach (Quest quest in questDB.QuestsList)
        {
            if (quest.Status == QuestStatus.InProgress) { questsInProgress.Add(quest); }
        }

        return questsInProgress;
    }

    public string GetQuestStatus(int questID)
    {

        //! inProgress : quest is in progress 
        //! notStarted  : quest is not yet started
        //! completed : quest is over

        //QuestDB questDB = new QuestDB();

        //! Defensive check if the quest list is not empty
        if (questDB.QuestsList.Count > 0)
        {
            for (int i = 0; i < questDB.QuestsList.Count; i++)
            {
                if (questDB.QuestsList[i].QuestID == questID)
                {
                    return questDB.QuestsList[i].Status.ToString();
                }
            }
        }

        return "questNotFound";
    }

    //! Currently take a TAG
    public void ChangeQuestStatus(int questID, string newStatus)
    {
        //! inProgress : quest is in progress 
        //! notStarted  : quest is not yet started
        //! completed : quest is over

        //QuestDB questDB = new QuestDB();

        //! Defensive check if the quest list is not empty
        if (questDB.QuestsList.Count > 0)
        {
            for (int i = 0; i < questDB.QuestsList.Count; i++)
            {
                if (questDB.QuestsList[i].QuestID == questID)
                {
                    switch (newStatus)
                    {
                        case "inProgress":
                            questDB.QuestsList[i].UpdateStatus(QuestStatus.InProgress);
                            return;
                        case "notStarted":
                            questDB.QuestsList[i].UpdateStatus(QuestStatus.NotStarted);
                            return;
                        case "completed":
                            questDB.QuestsList[i].UpdateStatus(QuestStatus.Completed);
                            return;
                        default:
                            Debug.LogWarning("Status undefined");
                            return;
                    }
                }
            }

        }
    }

    public void FulfillACondition(int questID, int conditionIndex)
    {
        bool conditionFound = false;

        foreach (var quest in questDB.QuestsList)
        {
            if (quest.QuestID == questID)
            {
                QuestCondition questCondition = quest.Conditions[conditionIndex];

                if (questCondition.LinkConditionsIndexes[0] == -1)
                {
                    questCondition.IsFulfilled = true;
                    conditionFound = true;
                    return;
                }
                else if (GetIfAConditionIsFullfiled(quest, questCondition))
                {
                    questCondition.IsFulfilled = true;
                    conditionFound = true;
                    return;
                }
            }
        }
        //! TO DO 
        if (conditionFound == false) { Debug.LogWarning("condition NON trouvé"); }
    }

    public void UpdateAllQuestStatus()
    {

        bool allConditionsFulfilled;

        if (questDB.QuestsList.Count > 0)
        {
            for (int i = 0; i < questDB.QuestsList.Count; i++)
            {
                allConditionsFulfilled = true;

                if (questDB.QuestsList[i].Status != QuestStatus.Completed)
                {
                    foreach (var condition in questDB.QuestsList[i].Conditions)
                    {
                        if (!condition.IsFulfilled)
                        {
                            allConditionsFulfilled = false;
                            break; // Sortir de la boucle dès qu'une condition n'est pas remplie
                        }
                    }
                }

                if (allConditionsFulfilled && questDB.QuestsList[i].Status != QuestStatus.Completed)
                {
                    ChangeQuestStatus(questDB.QuestsList[i].QuestID, "completed");
                }
            }
        }

    }

    public void TestAllQuestsForFullfiledConditons()
    {

        foreach (var quest in questDB.QuestsList)
        {
            if (quest.Status != QuestStatus.NotStarted)
            {
                foreach (var condition in quest.Conditions)
                {
                    //! Do not test if no ItemConditionName is set
                    //! Do not test if the conditon is already fullfilled
                    if (condition.ItemConditionName != "" && condition.IsFulfilled != true)
                    {
                        if (Inventory.GetInstance().IsTheItemInInventory(condition.ItemConditionName, condition.quantity))
                        {
                            condition.IsFulfilled = true;
                        }
                    }
                }
            }
        }

        //! Check if a quest is completed after fullfilling conditons
        UpdateAllQuestStatus();

    }

    public bool GetIfAConditionIsFullfiled(Quest quest, QuestCondition condition)
    {
        if (condition.LinkConditionsIndexes.Max() > quest.Conditions.Count)
        {
            Debug.LogWarning("Some link conditions are out a range, check the links index in quest");
        }

        foreach (var linkCondition in condition.LinkConditionsIndexes)
        {
            if (quest.Conditions[linkCondition].IsFulfilled == false)
            {
                return false;
            }
        }
        return true;
    }
}
