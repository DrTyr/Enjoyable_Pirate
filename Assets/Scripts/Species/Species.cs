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

            Debug.Log("Species awoken");

            speciesDescription = new List<SpeciesDescriptions>();
            conditionsList = new List<FriendsCondition>();

            Debug.Log(speciesDescription.Count);

            awoken = true;
            //childUnlockLevels = new Dictionary<string, int>();
            // speciesDescription = new List<SpeciesDescriptions>();
            // conditionsList = new List<FriendsCondition>();
        }
    }

    public virtual int GetUnlockLevel()
    {
        return -1;
    }

    public virtual void IncrementeUnlockedLevel()
    {
        //Will be defined in child
    }

    public int FindIndexByCommunName(string communName)
    {
        for (int i = 0; i < speciesDescription.Count; i++)
        {
            if (speciesDescription[i].CommunName == communName)
            {
                return i; // Retourne l'index de l'élément avec le CommunName recherché
            }
        }
        return -1; // Retourne -1 si aucun élément correspondant n'est trouvé
    }

    public SpeciesDescriptions GetDescriptionBySpeciesName(string speciesName)
    {

        Debug.Log("speciesDescription = " + speciesDescription);

        // Parcourt toutes les espèces dans la base de données
        foreach (SpeciesDescriptions description in speciesDescription)
        {
            // Vérifie si le nom de l'espèce correspond au nom recherché
            if (description.CommunName == speciesName)
            {
                // Retourne l'espèce si le nom correspond
                return description;
            }
        }
        // Retourne null si aucune espèce avec ce nom n'est trouvée
        return null;
    }

    // public void AddSpeciesInDictionnary(string name, int descriptionsUnlockLvl)
    // {

    //     if (!childUnlockLevels.ContainsKey(name))
    //     {
    //         childUnlockLevels.Add(name, descriptionsUnlockLvl);
    //     }

    // }

    // public SpeciesDescriptions FindDescription(string childName)
    // {
    //     // Parcourir la liste speciesDescription
    //     foreach (SpeciesDescriptions description in speciesDescription)
    //     {
    //         // Vérifier si le nom correspond à celui de l'enfant recherché
    //         if (description.ScientificName == childName)
    //         {
    //             // Retourner la première description trouvée
    //             return description;
    //         }
    //     }

    //     // Si aucune description correspondante n'est trouvée, retourner une chaîne vide ou une valeur par défaut
    //     return null;
    // }

    // public SpeciesDataBase FindDescription(string childName)
    // {
    //     // Parcourir la liste speciesDescription
    //     foreach (Species species in SpeciesDataBase.speciesDataBase)
    //     {
    //         // Vérifier si le nom correspond à celui de l'enfant recherché
    //         if (description.ScientificName == childName)
    //         {
    //             // Retourner la première description trouvée
    //             return description;
    //         }
    //     }

    //     // Si aucune description correspondante n'est trouvée, retourner une chaîne vide ou une valeur par défaut
    //     return null;
    // }

    // public int GetSpeciesUnlockLevels(string name)
    // {
    //     int index = -1;

    //     if (childUnlockLevels.ContainsKey(name))
    //     {
    //         // Accédez à la valeur associée à la clé "Carrots"
    //         index = childUnlockLevels[name];
    //         childUnlockLevels[name] += 1;

    //         Debug.Log(index);
    //     }

    //     return index;

    // }

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
