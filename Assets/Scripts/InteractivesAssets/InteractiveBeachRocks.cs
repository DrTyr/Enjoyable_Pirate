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

