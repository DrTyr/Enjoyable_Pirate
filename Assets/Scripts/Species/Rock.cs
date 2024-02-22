using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class Rock : Species
{
    public GameObject RockPrefab;

    public int copyIndex = 0;

    public override void OnTriggerEnter2D(Collider2D other)
    {
        //Rocks rock = gameObject.AddComponent<Rocks>();

        if (player != null && copyIndex == 0)
        {
            Debug.Log("coucou");
            PassCopyToInventory();
            gameObject.SetActive(false);
        }
    }

    public Rock Clone()
    {
        Rock clone = gameObject.AddComponent<Rock>();
        clone.copyIndex = 1;
        return clone;
    }

    public void PassCopyToInventory()
    {
        copyIndex = 1;
        Rock copy = Clone();
        inventory.GetComponent<GetItem>().SetItemInInventory(copy);
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        //Rocks rock = gameObject.AddComponent<Rocks>();

        if (player != null)
        {
            copyIndex = 1;
        }
    }

    public override void UseFromInventory([Optional] int quantity)
    {
        Debug.Log("Instanciate");
        //Rocks copy = Clone();
        //GameObject loadedObject = Resources.Load<GameObject>("Items/" + "Rock");
        GameObject loadedObject = Resources.Load<GameObject>("Items/" + "BeachRock");

        Instantiate(loadedObject, player.transform.position, Quaternion.identity);
        //loadedObject.GetComponent<Rocks>().copyIndex = 0;
        //copy.gameObject.SetActive(true);
    }

}
