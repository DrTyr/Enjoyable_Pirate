using System.Runtime.InteropServices;
using UnityEngine;

public class Carrot : GeneralItem
{
    new string name = "Carrot";

    [HideInInspector] public bool discovered = false;
    public string description0 = "Une carrote qui pousse sur rocher, étrange";
    public string description1 = "Il semble qu'il faille que plusieurs rocher soient proche pour que les carottes poussent";

    public override void Awake()
    {

        base.Awake();

        descriptions.Clear();
        descriptions.Add(description0);
        descriptions.Add(description1);

    }

    public void LoadDescriptions()
    {
        if (descriptions == null)
        {
            descriptions.Add(description0);
            descriptions.Add(description1);
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


}