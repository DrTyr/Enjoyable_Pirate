using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class InteractiveAssetsUI : MonoBehaviour
{
    //private InteractablesAssets assetToDisplay;

    //private UnityEngine.UI.Image imageToDisplay;
    //private UnityEngine.UI.Image SmallImagePanel;
    private GameObject ObjectToDisplay;
    //private List<GeneralItem> itemsToDisplay;

    //private List<GameObject> placeHolders;

    // List<GameObject> placeHolders = new List<GameObject>();
    private List<GameObject> placeHolders;
    private List<Button> buttons;

    //static System.Random rnd = new System.Random();

    public Sprite defaultSprite;

    private GeneralItem[] items;

    //private Dictionary<int, GeneralItem> correspondingItem;

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

        //correspondingItem = new Dictionary<int, GeneralItem>();


        //Debug.Log("Buttons.Count = " + buttons.Count);

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

        // // Créer une liste de numéros entre 0 et placeHolders.Count
        // List<int> numbers = Enumerable.Range(0, placeHolders.Count).ToList();
        // // Liste pour stocker les index générés aléatoirement
        // List<int> rndIndex = new List<int>();

        // for (int i = 0; i < placeHolders.Count; i++)
        // {
        //     if (numbers.Count > 0)
        //     {
        //         // Générer un index aléatoire
        //         int randomIndex = rnd.Next(numbers.Count);

        //         // Enregistrer l'index généré aléatoirement
        //         rndIndex.Add(numbers[randomIndex]);

        //         // Retirer cet index de la liste pour éviter les duplications
        //         numbers.RemoveAt(randomIndex);
        //     }
        // }

        // int[] index = new int[placeHolders.Count];

        // //Debug.Log("placeHolders.Count = " + placeHolders.Count);

        // for (int i = 0; i < itemsToDisplay.Length; i++)
        // {
        //     //Debug.Log("indices[i] = " + rndIndex[i]);
        //     if (itemsToDisplay[i] == null) { break; }

        //     placeHolders[i].SetActive(true);
        //     placeHolders[i].GetComponent<UnityEngine.UI.Image>().sprite = itemsToDisplay[i].GetComponent<SpriteRenderer>().sprite;

        //     index[i] = -1;

        //Debug.Log("rndIndex[i] = " + rndIndex[i]);
        //Debug.Log("itemToDisplay[i] = " + itemsToDisplay[i]);

        //! Keep track of what item is where
        //correspondingItem.Add(rndIndex[i], itemsToDisplay[i]);

        //items[i] = itemToDisplay[i];
        // }

        // foreach (var number in index)
        // {
        //     Debug.Log("index = " + number);
        // }

        //! Set inactive the empty placeholders
        // for (int i = 0; i < placeHolders.Count; i++)
        // {
        //     if (index[i] != -1) { placeHolders[i].SetActive(false); }
        // }

        int index = 0;

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
                int index = i; // Capture the current index in a local variable
                buttons[i].onClick.AddListener(() => ButtonClickAction(buttons[index], index));
            }
        }

    }

    // Méthode à exécuter lorsque le bouton est cliqué
    private void ButtonClickAction(Button button, int index)
    {
        // Debug.Log("Button clicked: " + button.name);
        // Debug.Log("Button clicked index : " + index);
        // Debug.Log("Item here : " + correspondingItem[index].name);

        InfoJournalUI.GetInstance().SetInfoToDisplay(items[index]);
    }

    // Méthode pour retirer les écouteurs d'événements onClick de chaque bouton
    public void RemoveButtonClickListeners()
    {
        foreach (Button button in buttons)
        {
            // Retirer tous les écouteurs d'événements onClick associés à ce bouton
            button.onClick.RemoveAllListeners();
        }
    }

    public void SetMeActive(Sprite sprite, GeneralItem[] itemsToDisplay)
    {
        //Debug.Log(sprite);
        //Debug.Log(itemsToDisplay);

        gameObject.SetActive(true);
        //ObjectToDisplay.SetActive(true);
        ObjectToDisplay.GetComponent<UnityEngine.UI.Image>().sprite = sprite;
        //items = assetToDisplay.itemsToDisplay;
        DisplayItems(itemsToDisplay);
        LinkTheButtons();

    }

    public void SetMeInactive()
    {
        //! Empty the dictionnary
        // correspondingItem.Clear();

        ObjectToDisplay.GetComponent<UnityEngine.UI.Image>().sprite = defaultSprite;

        foreach (Transform child in ObjectToDisplay.transform)
        {
            child.gameObject.GetComponent<UnityEngine.UI.Image>().sprite = defaultSprite;
        }
        RemoveButtonClickListeners();
        gameObject.SetActive(false);



    }


}
