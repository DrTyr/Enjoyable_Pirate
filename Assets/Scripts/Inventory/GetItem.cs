using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;


public class GetItem : MonoBehaviour
{
    private Inventory inventory;
    private UseItem useItem;
    private List<SlotManager> slots;

    private void Start()
    {
        inventory = GetComponent<Inventory>();
        useItem = GetComponent<UseItem>();
        slots = inventory.inventoryUI.slots;
    }

    //! Get a item diretly from the item GameObject
    public bool SetItemInInventory(GeneralItem item, [Optional] int quantity)
    {
        if (quantity == 0) { quantity = 1; }

        if (ChangeQuantityIfItemAlreadyInInventory(item.name, quantity)) { return true; }

        if (IsInventoryIsFull()) { return false; }

        return GenerateItemInInventory(item, quantity);
    }

    //! Get a item indiretly and load it
    public bool SetItemInInventory(string itemName, [Optional] int quantity)
    {
        if (quantity == 0) { quantity = 1; }

        if (ChangeQuantityIfItemAlreadyInInventory(itemName, quantity)) { return true; }

        if (IsInventoryIsFull()) { return false; }

        GeneralItem loadedObject = Resources.Load<GeneralItem>("Items/" + itemName);

        if (loadedObject != null)
        {
            return GenerateItemInInventory(loadedObject, quantity);
        }
        else
        {
            Debug.LogError("L'objet avec le nom " + itemName + " n'a pas été trouvé dans les ressources.");
            return false;
        }
    }

    private bool ChangeQuantityIfItemAlreadyInInventory(string itemName, int quantity)
    {

        //! Find the first 
        int index = System.Array.FindIndex(inventory.itemsInInventory, item => item != null && item.name == itemName);

        if (index == -1) { return false; }

        //! Update the the quantity displayed
        inventory.quantity[index] += quantity;
        slots[index].GetComponent<SlotManager>().ChangeItemDisplayQuantity(inventory.quantity[index]);
        return true;



        // for (int i = 0; i < inventory.itemsInInventory.Length; i++)
        // {
        //     GeneralItem currentItem = inventory.itemsInInventory[i];

        //     if (currentItem != null && currentItem.name == itemName)
        //     {
        //         //! Update the the quantity displayed
        //         inventory.quantity[i] += quantity;
        //         slots[i].GetComponent<SlotManager>().ChangeItemDisplayQuantity(inventory.quantity[i]);
        //         return true;
        //     }
        // }

        // return false;

    }

    private bool GenerateItemInInventory(GeneralItem item, int quantity)
    {

        //! Find the first null index
        int index = System.Array.FindIndex(inventory.itemsInInventory, item => item == null);

        //! not found
        if (index == -1) { return false; }

        //! Item store in inventory
        inventory.itemsInInventory[index] = item;
        inventory.quantity[index] += quantity;
        //! Display the item and the quantity
        slots[index].GetComponent<SlotManager>().DisplayItem(item);
        slots[index].GetComponent<SlotManager>().ChangeItemDisplayQuantity(inventory.quantity[index]);
        return true;



        // //! If the objecy is not found in the inventory, second loop to find the first empty slot
        // for (int i = 0; i < inventory.itemsInInventory.Length; i++)
        // {
        //     //! Add the item
        //     if (inventory.itemsInInventory[i] == null)
        //     {
        //         //! Item store in inventory
        //         inventory.itemsInInventory[i] = item;
        //         inventory.quantity[i] += quantity;
        //         //! Display the item and the quantity
        //         slots[i].GetComponent<SlotManager>().DisplayItem(item);
        //         slots[i].GetComponent<SlotManager>().ChangeItemDisplayQuantity(inventory.quantity[i]);
        //         return true;
        //     }
        // }

        // return false;

    }

    public bool IsInventoryIsFull()
    {

        for (int i = 0; i < inventory.itemsInInventory.Length; i++)
        {
            if (inventory.itemsInInventory[i] == null)
            {
                return false;
            }
        }

        return true;
    }

    public void MoveItemFromInventoryToChest(GeneralItem item, [Optional] int quantity)
    {
        Chest chest = inventory.chestCurrentlyOpen;
        chest.AddItem(item);
        useItem.RemoveItemFromInventory(item.name);
    }

}

