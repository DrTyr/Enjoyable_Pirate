using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class UseItem : MonoBehaviour
{
    private Inventory inventory;
    [HideInInspector] public SlotManager slot;
    [HideInInspector] public GetItem getItem;

    private List<SlotManager> slots;

    private void Start()
    {
        inventory = GetComponent<Inventory>();
        getItem = GetComponent<GetItem>();
        slots = inventory.inventoryUI.slots;

    }

    public bool RemoveItemFromInventory(string itemName, [Optional] int quantity)
    {
        if (quantity == 0) { quantity = 1; }

        int positionInInventory = PosItemInIventory(itemName);

        if (positionInInventory == -1)
        {
            Debug.Log("this item is not in the inventory : " + itemName);
            return false;
        }
        else
        {
            inventory.quantity[positionInInventory] -= quantity;


            if (inventory.quantity[positionInInventory] <= 0)
            {
                //! If there is no more of this object 
                slots[positionInInventory].GetComponent<SlotManager>().ResetDisplay();
                inventory.itemsInInventory[positionInInventory] = null;
                return true;
            }
            else
            {
                //! If the quantity is non null
                slots[positionInInventory].GetComponent<SlotManager>().ChangeItemDisplayQuantity(inventory.quantity[positionInInventory]);
                return true;
            }
        }
    }

    //! If true, return the position in inventory, if false return -1;
    public int PosItemInIventory(string itemName)
    {
        if (itemName == "")
        {
            return -1;
        }

        return System.Array.FindIndex(inventory.itemsInInventory, item => item != null && item.name == itemName);

    }


}

