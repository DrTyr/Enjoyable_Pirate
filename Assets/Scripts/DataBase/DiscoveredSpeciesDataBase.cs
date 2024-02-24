using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscoveredSpeciesDataBase : MonoBehaviour
{

    private static DiscoveredSpeciesDataBase instance;

    void Start()
    {

        instance = this;
    }

    public static DiscoveredSpeciesDataBase GetInstance()
    {

        return instance;
    }

    public void DiscoverAnItem(GameObject item)
    {

        DiscoveredSpecies.discoveredSpecies.Add(item);

    }

    public bool IsThisAlreadyDiscovered(GameObject item)
    {

        if (DiscoveredSpecies.discoveredSpecies.Contains(item))
        {
            return true;
        }

        return false;
    }

}


public static class DiscoveredSpecies
{
    public static List<GameObject> discoveredSpecies;

}
