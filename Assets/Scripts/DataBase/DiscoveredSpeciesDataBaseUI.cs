using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class DataBaseUI : MonoBehaviour
{
    public GameObject displayPanel;
    public GameObject button;
    public GameObject Exitbutton;

    //private List<GameObject> discoveredItems = new List<GameObject>();
    private List<GameObject> placeHolders = new List<GameObject>();
    private List<Button> placeHoldersButtons = new List<Button>();
    private List<GameObject> discoveredSpecies;

    void Start()
    {

        DiscoveredSpecies.discoveredSpecies = new();

        foreach (Transform child in displayPanel.transform)
        {
            placeHolders.Add(child.gameObject);
            placeHoldersButtons.Add(child.gameObject.GetComponentInChildren<Button>());

        }

        gameObject.SetActive(false);

    }

    public void DisplayDataBase()
    {
        gameObject.SetActive(true);

        int index = 0;

        discoveredSpecies = DiscoveredSpecies.discoveredSpecies;

        //! Replace the placeholder default sprite with the item at this location
        //! If no item to display, set the placeholder inactif
        foreach (var placeHolder in placeHolders)
        {
            if (index < discoveredSpecies.Count && discoveredSpecies[index] != null)
            {
                placeHolder.SetActive(true);
                placeHolder.GetComponent<UnityEngine.UI.Image>().sprite = discoveredSpecies[index].GetComponent<SpriteRenderer>().sprite;
                placeHolder.GetComponent<Transform>().localScale = new Vector3(1f, 1f, 0);
            }
            // else
            // {
            //     placeHolder.SetActive(false);
            // }

            index++;
        }
    }

    public void ExitDataBase()
    {

        gameObject.SetActive(false);

    }



}

