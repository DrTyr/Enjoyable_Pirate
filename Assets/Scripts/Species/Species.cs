using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Species : GeneralItem
{
    [HideInInspector] public static List<SpeciesDescriptions> speciesDescription;

    [HideInInspector] public static List<FriendsCondition> conditionsList;


    private static bool awoken = false;

    public override void Awake()
    {
        if (awoken == false)
        {
            base.Awake();

            speciesDescription = new List<SpeciesDescriptions>();
            conditionsList = new List<FriendsCondition>();

            awoken = true;
        }
    }

    public virtual int GetUnlockLevel()
    {
        return -1;
        //Will be defined in child
    }

    public virtual void IncrementeUnlockedLevel()
    {
        //Will be defined in child
    }

    public virtual int GetMaxUnlockLevel()
    {
        return -1;
        //Will be defined in child

    }

    public int FindIndexByCommunName(string communName)
    {
        for (int i = 0; i < speciesDescription.Count; i++)
        {
            if (speciesDescription[i].CommunName == communName)
            {
                return i;
            }
        }
        return -1;
    }

    public SpeciesDescriptions GetDescriptionBySpeciesName(string speciesName)
    {

        foreach (SpeciesDescriptions description in speciesDescription)
        {
            if (description.CommunName == speciesName)
            {
                return description;
            }
        }
        return null;
    }

    public void DiscoverSpecies(string name)
    {



    }

}



public class SpeciesDescriptions
{
    public string CommunName;
    public string ScientificName;
    public List<string> descriptionsText;

    public SpeciesDescriptions(string name)
    {
        CommunName = name;
        ScientificName = "";
        descriptionsText = new List<string>();
    }

}
