using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using Unity.UI;
using System;

public class Shell : Species
{
    //! Must be the name of the Prefab too
    new string name = "Shell";

    private static bool Awoken = false;

    [HideInInspector] public static int unlockedLevel = 0;

    [HideInInspector] public static int maxUnlockedLevel;

    public override void Awake()
    {
        if (!Awoken)
        {
            base.Awake();

            speciesDescription.Add(SetDescription());
            conditionsList.Add(SetConditions());

            SpeciesDatabase.Instance.AddSpecies(this);


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

        SpeciesDescriptions shell = new SpeciesDescriptions(this.name)
        {
            ScientificName = "Coquillus coquillatus"
        };
        shell.descriptionsText.Add("Test description shell 0");
        shell.descriptionsText.Add("Test description shell 1");
        shell.descriptionsText.Add("Test description shell 2");

        maxUnlockedLevel = shell.descriptionsText.Count;

        return shell;
    }


    public FriendsCondition SetConditions()
    {

        FriendsCondition condition = new("BeachRock")
        {
            FriendQuantity = 0,
            RequiredZone = ZonesNames.UpperBeach.ToString(),
            LoadedObjectAdress = "Species/" + this.name,
            ItemRewardName = this.name,
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
