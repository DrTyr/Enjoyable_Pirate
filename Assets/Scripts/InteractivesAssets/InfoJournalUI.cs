using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InfoJournalUI : MonoBehaviour
{
    //public Species speciesDataBase;
    public GameObject ImagePanelDisplay;
    public GameObject NameText;
    public GameObject SpeciesText;
    public GameObject DescriptionPanel;
    //public DataBase dataBase;
    private static InfoJournalUI instance;
    private GameObject CurrentObjectDisplay;
    public Slider[] sliders;
    public TextMeshProUGUI[] descriptionUI;
    //private SpeciesDescriptions speciesDescription;
    public float sliderFillDuration = 2f;

    int unlockedLevel;

    SpeciesDescriptions description;

    void Start()
    {

        gameObject.SetActive(false);

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

    public void SetInfoToDisplay(GeneralItem item)
    {

        gameObject.SetActive(true);

        foreach (Slider slider in sliders)
        {
            slider.gameObject.SetActive(false);
        }

        CurrentObjectDisplay = item.gameObject;

        //! Must Awake the object the first time, otherwise the descriptions are null because the object was never awaked !!
        //CurrentObjectDisplay.GetComponent<Species>().Awake();

        description = CurrentObjectDisplay.GetComponent<Species>().GetDescriptionBySpeciesName(CurrentObjectDisplay.name);


        Debug.Log("CurrentObjectDisplay.name : " + CurrentObjectDisplay.name);
        Debug.Log("description : " + description);

        ImagePanelDisplay.GetComponent<UnityEngine.UI.Image>().sprite = item.GetComponent<SpriteRenderer>().sprite;
        NameText.GetComponent<TextMeshProUGUI>().text = description.CommunName;
        SpeciesText.GetComponent<TextMeshProUGUI>().text = description.ScientificName;
        //DescriptionPanel.GetComponent<TextMeshProUGUI>().text = "Rien a afficher pour le moment";

        unlockedLevel = CurrentObjectDisplay.GetComponent<Species>().GetUnlockLevel();

        Debug.Log("unlockedLevel :" + unlockedLevel);

        for (int i = 0; i < unlockedLevel; i++)
        {

            descriptionUI[i].text = description.descriptionsText[i];

        }

    }

    public void Close()
    {

        gameObject.SetActive(false);

    }

    public void StudyButton()
    {

        if (DataBase.GetInstance().IsThisAlreadyDiscovered(CurrentObjectDisplay) == false)
        {
            DataBase.GetInstance().DiscoverAnItem(CurrentObjectDisplay);
        }

        CurrentObjectDisplay.GetComponent<Species>().IncrementeUnlockedLevel();

        DisplayStudy();
        //descriptionUI[0].text = CurrentObjectDisplay.GetComponent<GeneralItem>().description0;
    }

    public void DisplayStudy()
    {
        //int index = 0;

        // Debug.Log(CurrentObjectDisplay.name);

        // Debug.Log(CurrentObjectDisplay.GetComponent<Species>().name);



        // foreach (Species species in speciesDataBase)
        // {

        //     Debug.Log(species.name);

        // }

        // Debug.Log(CurrentObjectDisplay.GetComponent<Species>().childUnlockLevels.Count);

        // foreach (var kvp in CurrentObjectDisplay.GetComponent<Species>().childUnlockLevels)
        // {
        //     Debug.Log("Child Name: " + kvp.Key + ", Unlock Level: " + kvp.Value);
        // }

        //string foundItem = items.Find(item => item.Name == searchName);


        //SpeciesDescriptions description = CurrentObjectDisplay.GetComponent<Species>().FindDescription(CurrentObjectDisplay.name);

        // Debug.Log(CurrentObjectDisplay.GetComponent<Species>().childUnlockLevels.ContainsKey(CurrentObjectDisplay.name));

        //int unlockLevel = CurrentObjectDisplay.GetComponent<Species>().GetSpeciesUnlockLevels(description.CommunName);

        // if (CurrentObjectDisplay.GetComponent<Species>().childUnlockLevels.ContainsKey(description.CommunName))
        // {
        //     // Accédez à la valeur associée à la clé "Carrots"
        //     index = CurrentObjectDisplay.GetComponent<Species>().childUnlockLevels[CurrentObjectDisplay.name];
        //     CurrentObjectDisplay.GetComponent<Species>().childUnlockLevels[CurrentObjectDisplay.name] += 1;

        //     Debug.Log(index);
        // }
        //descriptionsUnlocklvl;

        //CurrentObjectDisplay.GetComponent<Species>().child.descriptionsUnlocklvl = 1;

        //speciesDescription = CurrentObjectDisplay.GetComponent<Species>().speciesDescription;
        Slider slider = sliders[unlockedLevel];
        slider.gameObject.SetActive(true);
        descriptionUI[unlockedLevel].text = "";

        // Debug.Log(index);
        // Debug.Log(slider);
        // Debug.Log(sliderFillDuration);
        // Debug.Log(descriptionUI[index]);
        // Debug.Log(CurrentObjectDisplay.GetComponent<Species>().speciesDescription.descriptionsText[0]);

        int index = CurrentObjectDisplay.GetComponent<Species>().FindIndexByCommunName(CurrentObjectDisplay.name);

        //Debug.Log("Ok here");

        StartCoroutine(SliderFiller.FillSlider(slider,
        sliderFillDuration,
        descriptionUI[unlockedLevel],
        Species.speciesDescription[index].descriptionsText[unlockedLevel]
        ));

    }

    // public Species GetSpeciesInfo(string name)
    // {

    //     return SpeciesDatabase.Instance.GetSpeciesByName(name);

    // }



}






public static class SliderFiller
{
    public static IEnumerator FillSlider(Slider slider, float fillDuration, TextMeshProUGUI description, string text)
    {
        float elapsedTime = 0f;
        float startValue = slider.value;
        float endValue = 1f;

        while (elapsedTime < fillDuration)
        {
            elapsedTime += Time.deltaTime;
            // float t = Mathf.Clamp01(elapsedTime / fillDuration);
            // slider.value = Mathf.Lerp(startValue, endValue, t);
            slider.value = elapsedTime;
            yield return null;
        }

        slider.value = endValue;
        slider.gameObject.SetActive(false);

        description.gameObject.SetActive(true);
        description.text = text;

    }
}

