using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractiveAssetsUI : MonoBehaviour
{
    //! the main object than inherit from InteractiveAssets
    private GameObject ObjectToDisplay;
    //! The object that will appear on it
    private GeneralItem[] items;
    //! The location where the objects will appear on ObjectToDisplay
    private List<GameObject> placeHolders;
    //! Each place holder has a button
    private List<Button> buttons;
    public Sprite defaultSprite;

    private static InteractiveAssetsUI instance;

    void Start()
    {
        instance = this;

        gameObject.SetActive(false);

        ObjectToDisplay = transform.Find("ObjectToDisplay").gameObject;

        placeHolders = new List<GameObject>();
        buttons = new List<Button>();

        foreach (Transform child in ObjectToDisplay.transform)
        {
            placeHolders.Add(child.gameObject);
            buttons.Add(child.gameObject.GetComponentInChildren<Button>());

        }

    }


    void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Found more than one Interactive Asset UI in the scene");
        }
        instance = this;
    }


    public static InteractiveAssetsUI GetInstance()
    {
        return instance;
    }

    private void DisplayItems(GeneralItem[] itemsToDisplay)
    {
        int index = 0;

        //! Replace the placeholder default sprite with the item at this location
        //! If no item to display, set the placeholder inactif
        foreach (var placeHolder in placeHolders)
        {
            if (index < itemsToDisplay.Length && itemsToDisplay[index] != null)
            {
                placeHolder.SetActive(true);
                placeHolder.GetComponent<UnityEngine.UI.Image>().sprite = itemsToDisplay[index].GetComponent<SpriteRenderer>().sprite;
                placeHolder.GetComponent<Transform>().localScale = new Vector3(1f, 1f, 0);
            }
            else
            {
                placeHolder.SetActive(false);
            }

            index++;
        }
    }

    private void LinkTheButtons()
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            if (placeHolders[i].activeSelf)
            {
                buttons[i].onClick.AddListener(() => ButtonClickAction(i));
            }
        }

    }

    private void ButtonClickAction(int index)
    {
        InfoJournalUI.GetInstance().SetInfoToDisplay(items[index]);
    }


    public void RemoveButtonClickListeners()
    {
        foreach (Button button in buttons)
        {
            button.onClick.RemoveAllListeners();
        }
    }

    //! Call when investigating a Interactive Object
    public void SetMeActive(Sprite sprite, GeneralItem[] itemsToDisplay)
    {

        gameObject.SetActive(true);
        ObjectToDisplay.GetComponent<UnityEngine.UI.Image>().sprite = sprite;
        DisplayItems(itemsToDisplay);
        LinkTheButtons();

    }

    //! Call not investigating a Interactive Object anymore
    public void SetMeInactive()
    {
        ObjectToDisplay.GetComponent<UnityEngine.UI.Image>().sprite = defaultSprite;

        foreach (Transform child in ObjectToDisplay.transform)
        {
            child.gameObject.GetComponent<UnityEngine.UI.Image>().sprite = defaultSprite;
        }
        RemoveButtonClickListeners();
        gameObject.SetActive(false);
    }


}
