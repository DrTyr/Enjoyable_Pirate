using System.Collections.Generic;
using UnityEngine;


public class InteractiveBeachRocks : InteractiveAssets
{
    private string assetName = "BeachRock";


    public override void Start()
    {
        //! Call the Start Method in the parent
        base.Start();

        ConditionsList = new List<FriendsCondition>();

        //! Player can press interact near the object to open a ZomIn Menu to view the species on the item
        zoomInToStudy = true;

        GetItemICanGrow();

        removeCoroutine = new Coroutine[ConditionsList.Count];
        removeCoroutineIsOn = new bool[ConditionsList.Count];
        detectCoroutine = new Coroutine[ConditionsList.Count];
        detectCoroutineIsOn = new bool[ConditionsList.Count];


    }

    public void GetItemICanGrow()
    {

        foreach (FriendsCondition condition in Species.conditionsList)
        {
            if (condition.SupportName == assetName)
            {
                ConditionsList.Add(condition);
            }
        }
    }

    public void GenerateSpeciesUnderIt(Vector3 pos)
    {
        GameObject crab = Resources.Load<GameObject>("Species/" + "Crabs");
        crab.GetComponent<Crabs>().isMoving = true;
        crab.GetComponent<Crabs>().endPosition = new Vector2(pos.x + 5f, pos.y);
        Instantiate(crab, pos, Quaternion.identity);


    }

}

