using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InfoJournalUI : MonoBehaviour
{
    public Button studyButton;
    public GameObject imagePanelDisplay;
    public GameObject nameText;
    public GameObject speciesText;
    private static InfoJournalUI instance;
    private GameObject itemCurrentlyDisplay;
    public Slider[] sliders;
    public TextMeshProUGUI[] descriptionUI;
    public float sliderFillDuration = 2f;
    int unlockedLevel;
    int maxUnlockedLevel;
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
        itemCurrentlyDisplay = item.gameObject;

        gameObject.SetActive(true);

        foreach (Slider slider in sliders)
        {
            slider.gameObject.SetActive(false);
        }

        Debug.Log(item.name);

        description = item.GetComponent<Species>().GetDescriptionBySpeciesName(item.name);
        unlockedLevel = item.GetComponent<Species>().GetUnlockLevel();
        maxUnlockedLevel = item.GetComponent<Species>().GetMaxUnlockLevel();

        imagePanelDisplay.GetComponent<UnityEngine.UI.Image>().sprite = item.GetComponent<SpriteRenderer>().sprite;
        nameText.GetComponent<TextMeshProUGUI>().text = description.CommunName;
        speciesText.GetComponent<TextMeshProUGUI>().text = description.ScientificName;

        for (int i = 0; i < unlockedLevel; i++)
        {
            descriptionUI[i].text = description.descriptionsText[i];
        }

    }



    public void StudyButton()
    {

        // if (DiscoveredSpeciesDataBase.GetInstance().IsThisAlreadyDiscovered(itemCurrentlyDisplay) == false)
        // {
        //     DiscoveredSpeciesDataBase.GetInstance().DiscoverAnItem(itemCurrentlyDisplay);
        // }

        //! If it's the first time this species is studied, add to discoveredDataBase
        if (unlockedLevel == 0)
        {
            DiscoveredSpeciesDataBase.GetInstance().DiscoverAnItem(itemCurrentlyDisplay.gameObject);

        }

        itemCurrentlyDisplay.GetComponent<Species>().IncrementeUnlockedLevel();

        DisplayStudy();

        if (unlockedLevel == maxUnlockedLevel)
        {
            studyButton.gameObject.SetActive(false);
        }

        unlockedLevel = itemCurrentlyDisplay.GetComponent<Species>().GetUnlockLevel();



    }

    public void DisplayStudy()
    {

        Slider slider = sliders[unlockedLevel];
        slider.gameObject.SetActive(true);
        descriptionUI[unlockedLevel].text = "";

        int index = itemCurrentlyDisplay.GetComponent<Species>().FindIndexByCommunName(itemCurrentlyDisplay.name);

        StartCoroutine(SliderFiller.FillSlider(slider,
        sliderFillDuration,
        descriptionUI[unlockedLevel],
        Species.speciesDescription[index].descriptionsText[unlockedLevel]
        ));

    }


    public void Close()
    {
        gameObject.SetActive(false);
    }

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

