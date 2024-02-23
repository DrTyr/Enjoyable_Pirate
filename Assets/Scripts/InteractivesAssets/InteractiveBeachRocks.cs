using System.Collections.Generic;
using UnityEngine;


public class InteractiveBeachRocks : InteractiveAssets
{


    public override void Start()
    {
        //! Call the Start Method in the parent
        base.Start();

        subListSpecies = new List<Species>();
        ConditionsList = new List<FriendsCondition>();

        GetItemICanGrow();

        removeCoroutine = new Coroutine[ConditionsList.Count];
        removeCoroutineIsOn = new bool[ConditionsList.Count];
        detectCoroutine = new Coroutine[ConditionsList.Count];
        detectCoroutineIsOn = new bool[ConditionsList.Count];


    }

    public void GetItemICanGrow()
    {

        List<Species> allSpecies = SpeciesDatabase.Instance.GetAllSpecies();

        foreach (Species species in allSpecies)
        {
            foreach (FriendsCondition condition in Species.conditionsList)
            {
                if (condition.SupportName == "BeachRock")
                {
                    subListSpecies.Add(species);
                    ConditionsList.Add(condition);
                }
            }

        }
    }

}

