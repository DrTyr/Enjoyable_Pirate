using System.Collections.Generic;
using UnityEngine;


public class InteractiveBeachRocks : InteractiveAssets
{

    public override void Start()
    {
        //! Call the Start Method in the parent
        base.Start();

        SetAllInteractionWithInteractiveAssets();

        removeCoroutine = new Coroutine[ConditionsList.Count];
        removeCoroutineIsOn = new bool[ConditionsList.Count];
        detectCoroutine = new Coroutine[ConditionsList.Count];
        detectCoroutineIsOn = new bool[ConditionsList.Count];

    }

    private void SetAllInteractionWithInteractiveAssets()
    {
        ConditionsList = new List<FriendsCondition>();

        // Create condition
        FriendsCondition conditions1 = new("BeachRock")
        {
            FriendQuantity = 3,
            RequiredZone = ZonesNames.UpperBeach.ToString(),
            LoadedObjectAdress = "Items/" + "Carrot",
            ItemRewardName = "Carrot",
            DetectionTimer = 10f
        };

        // Add condition to the list
        ConditionsList.Add(conditions1);

        FriendsCondition conditions2 = new("BeachRock")
        {
            FriendQuantity = 2,
            RequiredZone = ZonesNames.LowerBeach.ToString(),
            LoadedObjectAdress = "Items/" + "Star",
            ItemRewardName = "Star",
            DetectionTimer = 10f
        };

        // Add condition to the list
        ConditionsList.Add(conditions2);

    }
}

public class FriendsCondition
{
    public string FriendName;
    public int FriendQuantity;
    public string RequiredZone;
    public string LoadedObjectAdress;
    public float DetectionTimer; //! Time to detecte sourrounding friend, in seconds IRL (ex : 60f = every 60 secondes)
    //public bool RightZone;
    public string ItemRewardName;
    public bool CoRoutineInProgress;
    public FriendsCondition(string name)
    {
        //! Conditions to Add a object to itemList
        FriendName = name;
        FriendQuantity = 300; // By default, the quantity is too high
        RequiredZone = "zone";
        //string requiredZone = "";
        //! Item added if conditions fullfilled
        //! Folder adress, root is Resources/...
        //! Exemple : "Items/" + "Carrot"
        LoadedObjectAdress = "";
        DetectionTimer = 600.0f; // By default, the time is too high
                                 // RightZone = false;
        ItemRewardName = "";
        CoRoutineInProgress = false;
    }

}