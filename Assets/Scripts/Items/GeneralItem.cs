using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;


public class GeneralItem : MonoBehaviour
{
    [HideInInspector] public static PlayerController player;
    [HideInInspector] public static Inventory inventory;
    //[Header("Set the UI sprite for this object")]
    //public Sprite UiImage;

    public int quantity = 1;

    public virtual void Awake()
    {

        //Debug.Log("Item awake");

        player = Object.FindFirstObjectByType<PlayerController>();
        inventory = Object.FindFirstObjectByType<Inventory>();

        if (player == null)
        {
            Debug.LogWarning("PlayerController not found in the scene.");
        }

        if (inventory == null)
        {
            Debug.LogWarning("Inventory not found in the scene.");
        }
    }

    public virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (player != null)
        {
            inventory.GetComponent<GetItem>().SetItemInInventory(this);
            //Destroy(gameObject);
        }
    }

    public virtual void UseFromInventory([Optional] int quantity)
    {
        Debug.Log("Use item not defined");

    }

}
