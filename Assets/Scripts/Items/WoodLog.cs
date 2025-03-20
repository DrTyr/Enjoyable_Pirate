using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

/// <summary>
/// Represents a TreeLog item in the game, inheriting from GeneralItem.
/// </summary>
public class WoodLog : GeneralItem
{
    /// <summary>
    /// Prefab of the TreeLog.
    /// </summary>
    public GameObject WoodLogPrefab;

    /// <summary>
    /// Index to track if the TreeLog is a copy.
    /// </summary>
    public int copyIndex = 0;

    /// <summary>
    /// Triggered when another collider enters the trigger collider attached to the object where this script is attached.
    /// </summary>
    /// <param name="other">The other Collider2D involved in this collision.</param>
    public override void OnTriggerEnter2D(Collider2D other)
    {
        if (player != null && copyIndex == 0)
        {
            PassCopyToInventory();
            gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Creates a clone of the current TreeLog object.
    /// </summary>
    /// <returns>A new TreeLog object that is a copy of the current one.</returns>
    public WoodLog Clone()
    {
        WoodLog clone = gameObject.AddComponent<WoodLog>();
        clone.copyIndex = 1;
        return clone;
    }

    /// <summary>
    /// Passes a copy of the TreeLog to the player's inventory.
    /// </summary>
    public void PassCopyToInventory()
    {
        copyIndex = 1;
        WoodLog copy = Clone();
        inventory.GetComponent<GetItem>().SetItemInInventory(copy);
    }

    /// <summary>
    /// Triggered when another collider exits the trigger collider attached to the object where this script is attached.
    /// </summary>
    /// <param name="other">The other Collider2D involved in this collision.</param>
    public void OnTriggerExit2D(Collider2D other)
    {
        if (player != null)
        {
            copyIndex = 1;
        }
    }

    /// <summary>
    /// Uses the TreeLog item from the inventory.
    /// </summary>
    /// <param name="quantity">Optional parameter to specify the quantity to use.</param>
    public override void UseFromInventory([Optional] int quantity)
    {
        GameObject loadedObject = Resources.Load<GameObject>("Items/" + "TreeLog");

        Instantiate(loadedObject, player.transform.position, Quaternion.identity);
    }
}