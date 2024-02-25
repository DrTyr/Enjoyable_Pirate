using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using Unity.UI;
using System;

public class SunFlower : Species
{
    //! Must be the name of the Prefab too
    new string name = "SunFlower";


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

        SpeciesDescriptions SunFlower = new SpeciesDescriptions(this.name)
        {
            ScientificName = "Sun Flower"
        };
        SunFlower.descriptionsText.Add("Test description SunFlower 0");
        SunFlower.descriptionsText.Add("Test description SunFlower 1");
        SunFlower.descriptionsText.Add("Test description SunFlower 2");

        maxUnlockedLevel = SunFlower.descriptionsText.Count;

        return SunFlower;
    }


    public FriendsCondition SetConditions()
    {

        FriendsCondition condition = new("Field")
        {
            FriendQuantity = 0,
            RequiredZone = "",
            LoadedObjectAdress = "Species/" + this.name,
            ItemRewardName = this.name,
            DetectionTimer = 2f,
            SupportName = "Field"
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
