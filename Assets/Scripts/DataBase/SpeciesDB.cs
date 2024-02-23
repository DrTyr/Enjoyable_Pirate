using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeciesDatabase : MonoBehaviour
{
    private static SpeciesDatabase instance;
    private List<Species> allSpecies = new List<Species>();

    // Méthode statique pour obtenir l'instance unique de la base de données
    public static SpeciesDatabase Instance
    {
        get
        {
            // Si l'instance n'existe pas encore, la créer
            if (instance == null)
            {
                GameObject databaseObject = new GameObject("SpeciesDatabase");
                instance = databaseObject.AddComponent<SpeciesDatabase>();
            }
            return instance;
        }
    }

    // Méthode pour ajouter une espèce à la base de données
    public void AddSpecies(Species species)
    {
        allSpecies.Add(species);
    }

    // Méthode pour obtenir toutes les espèces de la base de données
    public List<Species> GetAllSpecies()
    {
        return allSpecies;
    }

    // Méthode pour récupérer une espèce par son nom
    public Species GetSpeciesByName(string speciesName)
    {
        // Parcourt toutes les espèces dans la base de données
        foreach (Species species in allSpecies)
        {
            // Vérifie si le nom de l'espèce correspond au nom recherché
            if (species.name == speciesName)
            {
                // Retourne l'espèce si le nom correspond
                return species;
            }
        }
        // Retourne null si aucune espèce avec ce nom n'est trouvée
        return null;
    }

    // Méthode pour modifier une espèce et la réenregistrer
    public void ModifySpecies(string speciesName, Species modifiedSpecies)
    {
        // Parcourt toutes les espèces dans la base de données
        for (int i = 0; i < allSpecies.Count; i++)
        {
            // Vérifie si le nom de l'espèce correspond au nom recherché
            if (allSpecies[i].name == speciesName)
            {
                // Remplace l'espèce existante par la nouvelle espèce modifiée
                allSpecies[i] = modifiedSpecies;
                // Sort de la boucle une fois que la modification est effectuée
                break;
            }
        }
        // Enregistre la base de données modifiée si nécessaire
    }

}
