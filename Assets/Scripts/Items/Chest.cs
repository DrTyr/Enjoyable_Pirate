using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;


public class Chest : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private PlayerController player;

    [Header("Set open and close sprite for this chest")]
    [SerializeField] private Sprite chestOpenSprite;
    [SerializeField] private Sprite chestCloseSprite;
    private Inventory inventory;
    [HideInInspector] public List<SlotManager> slotManagers;
    private Transform chestUI;
    private GameObject canvas;

    [Header("Content/Quantity in the chest at start")]
    public GeneralItem[] contents;
    public int[] quantity;
    private void Start()
    {
        player = FindObjectOfType(typeof(PlayerController)) as PlayerController;
        canvas = GetComponentInChildren<Canvas>().gameObject;
        chestUI = canvas.transform.GetChild(0);
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = chestCloseSprite;
        inventory = FindObjectOfType(typeof(Inventory)) as Inventory;
        //! Defensive test, if quantity is set but content is null, set quantity to 0
        for (int i = 0; i < contents.Length; i++) { if (contents[i] == null) { quantity[i] = 0; } }
        GetSlotsUI();
        canvas.SetActive(false);

    }

    void GetSlotsUI()
    {
        foreach (Transform child in chestUI)
        {
            slotManagers.Add(child.GetComponent<SlotManager>());
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (player != null)
        {
            OpenChest();
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (player != null)
        {
            CloseChest();
        }
    }

    public void OpenChest()
    {
        spriteRenderer.sprite = chestOpenSprite;
        inventory.chestCurrentlyOpen = this;
        canvas.SetActive(true);
        DisplayChestContent();
    }

    public void CloseChest()
    {
        spriteRenderer.sprite = chestCloseSprite;
        inventory.chestCurrentlyOpen = null;
        canvas.SetActive(false);
    }

    public void DisplayChestContent()
    {
        for (int i = 0; i < contents.Length; i++)
        {
            if (contents[i] != null)
            {
                slotManagers[i].DisplayItem(contents[i]);
                slotManagers[i].ChangeItemDisplayQuantity(quantity[i]);
            }
        }
    }

    public void AddItem(GeneralItem item, [Optional] int quantity)
    {
        if (quantity == 0) { quantity = 1; }

        // Check all the content of the chest, first loop to check all the contents
        for (int i = 0; i < contents.Length; i++)
        {
            //! Test if null before otherwise .name does not exist
            if (contents[i] != null)
            {
                //! If this item is already in the chest, item++
                if (contents[i].name == item.name)
                {
                    //! Set the SlotManager Class for the current slot
                    //SlotManager slot = slots[i].GetComponent<SlotManager>();
                    this.quantity[i] += quantity;
                    slotManagers[i].ChangeItemDisplayQuantity(this.quantity[i]);
                    return;
                }
            }
        }

        //! If the objecy is not found, second loop to find the first empty slot
        for (int i = 0; i < contents.Length; i++)
        {
            //! If not and a slot is free, add the item
            if (contents[i] == null)
            {
                //Item store in inventory
                contents[i] = item;
                this.quantity[i] += quantity;
                //SlotManager slot = slots[i].GetComponent<SlotManager>(); ;
                slotManagers[i].DisplayItem(item);
                slotManagers[i].ChangeItemDisplayQuantity(this.quantity[i]);
                return;
            }
        }

        Debug.Log("Could not add the item in the chest");
    }

    public void MoveItemFromChestToInventory(int index)
    {
        //! If there is a object at this index
        if (quantity[index] > 0)
        {
            quantity[index] -= 1;

            //! If quantity is 0 or negative
            if (quantity[index] <= 0)
            {
                //! Remove the image of the Item in the chest Slot;
                slotManagers[index].ResetDisplay();
                //! Get to last item in inventory
                inventory.GetComponent<GetItem>().SetItemInInventory(contents[index]);
                //! free the content slot
                contents[index] = null;
            }
            else
            {
                //! Change the  display quantity
                slotManagers[index].ChangeItemDisplayQuantity(quantity[index]);
                //! Get the item in inventory
                inventory.GetComponent<GetItem>().SetItemInInventory(contents[index]);
            }

        }
    }


}
