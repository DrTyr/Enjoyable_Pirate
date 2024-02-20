using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

//! For test object to test Use() in item objects
public class GeneralItemUser : MonoBehaviour
{
    public GeneralItem itemToUse;
    //private GameObject itemToUse;

    [Header("Automatic set at start")]
    public PlayerController player;

    [Header("Automatic set at start")]
    public Inventory inventoryManager;

    public int quantity = 3;

    private void Start()
    {
        player = FindObjectOfType(typeof(PlayerController)) as PlayerController;
        inventoryManager = FindObjectOfType(typeof(Inventory)) as Inventory;
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (player != null)
        {
            itemToUse.SendMessage("UseFromInventory", quantity);
        }
    }


}
