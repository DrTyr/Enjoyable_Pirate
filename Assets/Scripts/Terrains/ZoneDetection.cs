using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ZonesNames
{
    UpperBeach,
    LowerBeach
}

public class ZoneDetection : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        //! If the object is beachRocks, change its var zoneName to the current zone
        if (other.TryGetComponent<InteractiveBeachRocks>(out var beachRocks))
        {
            string zoneName = transform.name;
            beachRocks.SetZone(zoneName);
        }
    }

    //! Problème avec GRAB, si l'object rentre en étant porté, OnTriggerEnter2D s'active
    //! Mais au moment de le poser OnTriggerExit2D s'active
    //! TO DO : A corriger

    // private void OnTriggerExit2D(Collider2D other)
    // {
    //     // Vérifier si l'objet qui est sorti est un InteractiveBeachRocks
    //     if (other.TryGetComponent<InteractiveBeachRocks>(out var beachRocks))
    //     {
    //         // Obtenir le nom de la zone à partir du nom de l'enfant de Beach
    //         string zoneName = transform.name;
    //         //Debug.Log("Object in zone: " + zoneName);
    //         beachRocks.ClearZone();
    //     }
    // }
}
