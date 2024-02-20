using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InfoJournalUI : MonoBehaviour
{
    public GameObject ImagePanelDisplay;

    public GameObject NameText;
    public GameObject SpeciesText;

    public GameObject DescriptionPanel;

    private static InfoJournalUI instance;


    void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Found more than one JournalUI in the scene");
        }
        instance = this;
    }


    public static InfoJournalUI GetInstance()
    {
        return instance;
    }

    void Start()
    {

        gameObject.SetActive(false);
    }

    public void SetInfoToDisplay(GeneralItem item)
    {

        gameObject.SetActive(true);

        ImagePanelDisplay.GetComponent<UnityEngine.UI.Image>().sprite = item.GetComponent<SpriteRenderer>().sprite;
        NameText.GetComponent<TextMeshProUGUI>().text = item.name;
        SpeciesText.GetComponent<TextMeshProUGUI>().text = "coucou";
        //DescriptionPanel.GetComponent<TextMeshProUGUI>().text = "Rien a afficher pour le moment";


    }
}
