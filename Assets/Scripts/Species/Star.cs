using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using Unity.UI;

public class Star : Species
{
    new string name = "Star";

    private static bool Awoken = false;

    [HideInInspector] public static int unlockedLevel = 0;

    [HideInInspector] public static int maxUnlockedLevel; // 0, 1 and 2 ; Currently set to 1 because only 2 display in JournalUI;


    public override void Awake()
    {
        if (!Awoken)
        {
            base.Awake();

            speciesDescription.Add(SetDescription());
            conditionsList.Add(SetConditions());

            //SpeciesDatabase.Instance.AddSpecies(this);


            Awoken = true;
        }

    }

    public override int GetUnlockLevel()
    {
        return unlockedLevel;
    }


    public override void IncrementeUnlockedLevel()
    {
        if (unlockedLevel < maxUnlockedLevel)
        {
            unlockedLevel++;
        }
    }

    public SpeciesDescriptions SetDescription()
    {

        SpeciesDescriptions star = new SpeciesDescriptions("Star")
        {
            ScientificName = "Starus starus"
        };
        star.descriptionsText.Add("Test description 0");
        star.descriptionsText.Add("Test description 1");
        star.descriptionsText.Add("Test description 2");

        maxUnlockedLevel = star.descriptionsText.Count;

        return star;
    }


    public FriendsCondition SetConditions()
    {

        FriendsCondition condition = new("BeachRock")
        {
            FriendQuantity = 1,
            RequiredZone = ZonesNames.LowerBeach.ToString(),
            LoadedObjectAdress = "Species/" + "Star",
            ItemRewardName = "Star",
            DetectionTimer = 10f,
            SupportName = "BeachRock"
        };
        return condition;
    }

    public override void UseFromInventory([Optional] int quantity)
    {
        //quantity = 0 meaning the parameter was not passed, default to this.quantity
        if (quantity == 0) { quantity = this.quantity; }
        //Inventory inventory = inventoryManager.GetComponent<Inventory>();
        //! Check if enough item in inventory to use this quantity
        if (inventory.GetQuantityOfThisItem(name) == 0)
        {
            Debug.Log("This item is not is the Inventory : " + name);
        }
        else if (inventory.GetQuantityOfThisItem(name) >= quantity)
        {
            Debug.Log("Used :" + name);

            inventory.GetComponent<UseItem>().RemoveItemFromInventory(name, quantity);
        }
        else
        {
            //! TO DO
            Debug.Log("Not enough " + name + " in inventory, asked/available : " + quantity + "/" + inventory.GetQuantityOfThisItem(name));
        }
    }





}
