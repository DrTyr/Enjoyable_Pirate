using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class Carrot : Species
{
    new string name = "Carrots";
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

    public override int GetMaxUnlockLevel()
    {
        return maxUnlockedLevel;
    }


    public override void IncrementeUnlockedLevel()
    {
        if (unlockedLevel < maxUnlockedLevel)
        {
            unlockedLevel++;
        }
    }

    public override void UseFromInventory([Optional] int quantity)
    {
        //quantity = 0 meaning the parameter was not passed, default to this.quantity
        if (quantity == 0) { quantity = this.quantity; }
        //Inventory inventory = inventory.GetComponent<Inventory>();
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
            //! TO DO ?
            Debug.Log("Not enough " + name + " in inventory, asked/available : " + quantity + "/" + inventory.GetQuantityOfThisItem(name));
        }
    }

    public SpeciesDescriptions SetDescription()
    {

        SpeciesDescriptions carrots = new SpeciesDescriptions("Carrot")
        {
            ScientificName = "Carotus carotus"
        };
        carrots.descriptionsText.Add("Une carrote qui pousse sur un rocher, étrange");
        carrots.descriptionsText.Add("Cette espèce à l'air adapté à la vie très proche de l'océan");
        carrots.descriptionsText.Add("Il semble qu'il faille que plusieurs rocher soient proche pour que les carottes poussent");

        maxUnlockedLevel = carrots.descriptionsText.Count;

        return carrots;
    }


    public FriendsCondition SetConditions()
    {

        FriendsCondition condition = new("BeachRock")
        {
            FriendQuantity = 2,
            RequiredZone = ZonesNames.UpperBeach.ToString(),
            LoadedObjectAdress = "Species/" + "Carrot",
            ItemRewardName = "Carrot",
            DetectionTimer = 10f,
            SupportName = "BeachRock"
        };

        return condition;

    }



}