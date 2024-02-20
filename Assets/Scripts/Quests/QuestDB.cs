using System.Collections.Generic;
using UnityEngine;

public class QuestDB : MonoBehaviour
{
    [HideInInspector] public List<Quest> QuestsList { get; private set; }

    private static QuestDB instance;


    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Found more than one QuestDB in the scene");
        }
        instance = this;

        GenerateQuestDB();

    }

    public static QuestDB GetInstance()
    {
        return instance;
    }

    public void GenerateQuestDB()
    {
        QuestsList = new List<Quest>();

        // Create quest
        Quest quest1 = new Quest("Parler au PNJ")
        {
            Description = "Trouvez un monsieur vert accompagné d'une madame jaune",
            QuestID = 1,
            Status = QuestStatus.InProgress
        };
        quest1.Conditions.Add(new QuestCondition()
        {
            ConditionName = "Avoir parlé au PNJ",
            ConditionIndex = 0,
        });

        // Add quest to the list
        QuestsList.Add(quest1);

        // Create quest
        Quest quest2 = new Quest("Collecte de sacs et de carrotes")
        {
            Description = "On besoin de récupérer des sacs et des carrotes, 3 de chaques feront l'affaire",
            QuestID = 2,
            //Status = QuestStatus.NotStarted
        };
        quest2.Conditions.Add(new QuestCondition()
        {
            ConditionName = "Avoir 3 sacs dans l'inventaire",
            ConditionIndex = 0,
            ItemConditionName = "Bag",
            quantity = 3,

        });
        quest2.Conditions.Add(new QuestCondition()
        {
            ConditionName = "Avoir 3 carottes dans l'inventaire",
            ConditionIndex = 1,
            ItemConditionName = "Carrot",
            quantity = 3,

        });
        quest2.Conditions.Add(new QuestCondition()
        {
            ConditionName = "Reparler au PNJ",
            ConditionIndex = 2,
            LinkConditionsIndexes = new int[] { 0, 1 }
        });

        // Add quest to the list
        QuestsList.Add(quest2);

        // Create quest
        Quest quest3 = new Quest("Construire une maison")
        {
            Description = "Réunissez 5 bois pour construire la maison",
            QuestID = 3,
            Status = QuestStatus.InProgress
        };
        quest3.Conditions.Add(new QuestCondition()
        {
            ConditionName = "Réunir 5 bois",
            ConditionIndex = 0,
            ItemConditionName = "WoodLogs",
            quantity = 5,
        });
        // quest3.Conditions.Add(new QuestCondition()
        // {
        //     ConditionName = "Construire la maison",
        //     ConditionIndex = 1,
        // });

        // Add quest to the list
        QuestsList.Add(quest3);
    }


}
