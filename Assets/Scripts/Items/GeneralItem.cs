using System.Runtime.InteropServices;
using UnityEngine;


public class GeneralItem : MonoBehaviour
{
    [HideInInspector] public static PlayerController player;
    [HideInInspector] public static Inventory inventory;
    //[Header("Set the UI sprite for this object")]
    //public Sprite UiImage;

    [Header("Name of this object")]
    public new string name;

    public int quantity = 1;

    public void Start()
    {
        player = FindObjectOfType(typeof(PlayerController)) as PlayerController;
        inventory = FindObjectOfType(typeof(Inventory)) as Inventory;
        if (name == "") { Debug.LogWarning("Must set a name for this object : " + this); }
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
