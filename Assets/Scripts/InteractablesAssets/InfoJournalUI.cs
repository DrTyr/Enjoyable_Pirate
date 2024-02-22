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
    //public DataBase dataBase;
    private static InfoJournalUI instance;
    private GameObject CurrentObjectDisplay;
    public Slider[] sliders;
    public TextMeshProUGUI[] descriptionUI;

    public float sliderFillDuration = 2f;

    void Start()
    {

        gameObject.SetActive(false);

        if (instance != null)
        {
            Debug.LogWarning("Found more than one JournalUI in the scene");
        }
        instance = this;



        // foreach(TextMeshProUGUI description in descriptionUI){

        //     description.gameObject.SetActive(false);
        // }

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

        ImagePanelDisplay.GetComponent<UnityEngine.UI.Image>().sprite = item.GetComponent<SpriteRenderer>().sprite;
        NameText.GetComponent<TextMeshProUGUI>().text = item.name;
        SpeciesText.GetComponent<TextMeshProUGUI>().text = "coucou";
        //DescriptionPanel.GetComponent<TextMeshProUGUI>().text = "Rien a afficher pour le moment";

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

        DisplayStudy(0);
        //descriptionUI[0].text = CurrentObjectDisplay.GetComponent<GeneralItem>().description0;
    }

    public void DisplayStudy(int index)
    {
        Slider slider = sliders[index];
        slider.gameObject.SetActive(true);
        //! Must Awake the object the first time, otherwise the descriptions are null because the object was never awaked !!
        CurrentObjectDisplay.GetComponent<GeneralItem>().Awake();
        descriptionUI[index].text = "";
        string text = CurrentObjectDisplay.GetComponent<GeneralItem>().descriptions[index];
        StartCoroutine(SliderFiller.FillSlider(slider, sliderFillDuration, descriptionUI[index], text));

    }

    public void DisplayDescriptionText(string description)
    {


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

