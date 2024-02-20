using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public InventoryUI inventoryUI;
    public GeneralItem[] itemsInInventory;
    public int[] quantity;
    private static Inventory instance;
    [HideInInspector] public Chest chestCurrentlyOpen;


    private void Start()
    {
        if (instance != null)
        {
            Debug.LogWarning("Found more than one Inventory in the scene");
        }
        instance = this;

        //GameObject[] slots = Inventory.slots;
    }

    public static Inventory GetInstance()
    {
        return instance;
    }

    public bool IsTheItemInInventory(string itemName, [Optional] int quantityItem)
    {
        if (quantityItem == 0) { quantityItem = 1; }

        for (int i = 0; i < itemsInInventory.Length; i++)
        {
            if (itemsInInventory[i] != null)
            {
                if (itemsInInventory[i].name == itemName && quantity[i] >= quantityItem)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public int GetAmountEmptySlots()
    {
        int amount = 0;

        for (int i = 0; i < itemsInInventory.Length; i++)
        {
            if (itemsInInventory[i] == null)
            {
                amount++;
            }
        }
        return amount;
    }

    public int GetQuantityOfThisItem(string itemName)
    {

        //! Find the first 
        int index = System.Array.FindIndex(itemsInInventory, item => item.name == itemName);

        if (index == -1) { return 0; }

        return quantity[index];

        // for (int i = 0; i < itemsInInventory.Length; i++)
        // {
        //     if (itemsInInventory[i] != null)
        //     {
        //         if (itemsInInventory[i].name == itemName)
        //         {
        //             return quantity[i];
        //         }
        //     }
        // }
        // //! If the item is not found in the inventory, the quantity returned is 0
        // return 0;
    }

    public void InteractWithItem(int index)
    {
        //! Use from a button in a slot display a item

        //! If in a chest, move to object to the chest
        if (chestCurrentlyOpen != null && itemsInInventory[index] != null)
        {
            //! Add the item to the chest
            chestCurrentlyOpen.AddItem(itemsInInventory[index]);

            //! Remove the item form the inventory
            UseItem useItem = GetComponent<UseItem>();
            useItem.RemoveItemFromInventory(itemsInInventory[index].name);
            return;
        }
        //! If not in chest, apply the fonction UseFromInventory from this item
        else
        {
            if (itemsInInventory[index] != null)
            {
                itemsInInventory[index].UseFromInventory();
            }
        }

    }

    public void LinkButtons()
    {
        int index = 0;

        foreach (Button button in inventoryUI.buttons)
        {
            button.onClick.AddListener(() => ButtonClickAction(button, index));
            index++;
        }

    }

    public void ButtonClickAction(Button button, int index)
    {
        Debug.Log("Button clicked: " + button.name);
        Debug.Log("Button clicked index : " + index);
        Debug.Log("Item here : " + itemsInInventory[index].name);

        InteractWithItem(index);

    }


}