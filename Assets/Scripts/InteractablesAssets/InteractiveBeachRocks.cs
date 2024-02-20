using System.Collections.Generic;
using UnityEngine;


public class InteractiveBeachRocks : InteractiveAssets
{

    private List<string> friendName = new List<string>();
    private List<int> friendQuantity = new List<int>();
    private List<string> loadedObjectAdress = new List<string>();
    private float detectionTimer = 5.0f;


    public override void Start()
    {
        //! Call the Start Method is the parent, here InteractiveAssets;
        base.Start();

        SetAllInteractionWithInteractiveAssets();

        // Start the coroutine to perform detection every n seconds
        StartCoroutine(DetectObjectsPeriodically(detectionTimer, friendName, friendQuantity, loadedObjectAdress));

    }



    private void SetAllInteractionWithInteractiveAssets()
    {

        //! Need to be around 3 beachRock
        friendName.Add("BeachRock");
        friendQuantity.Add(3);
        //! Folder adress, root is Resources/...
        loadedObjectAdress.Add("Items/" + "Carrot");

        //Debug.Log("loadedObjectAdress = " + loadedObjectAdress[0]);

    }




}