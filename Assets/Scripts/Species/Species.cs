using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Species : GeneralItem
{
    //[HideInInspector] public List<string> descriptions = new List<string>();
    [HideInInspector] public SpeciesDescriptions speciesDescription;

    public override void Awake()
    {
        base.Awake();
    }

}

