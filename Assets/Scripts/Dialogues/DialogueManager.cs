using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using Ink.Runtime;
using UnityEngine.EventSystems;
using System.Collections;

//! This class in a singleton, only one can exist in a scene
public class DialogueManager : MonoBehaviour
{
    //! See in the NOTE file, link to youtube tutorials about INK.
    //! The questManager is a singleton managing the advancement of all the quests
    [Header("Quest Manager")][SerializeField] private QuestManager questManager;
    //! The inventoryManager is a singleton managing the player Inventory
    [Header("Inventory Manager")][SerializeField] private GameObject inventoryManager;
    //! Set here the interct button defined in the Input actions
    [Header("Input Manager : interact")][SerializeField] private InputActionReference interact;
    //! Here the UI that display all the dialogue and the asnwers
    [Header("Dialogue UI")][SerializeField] private GameObject dialoguePanel;
    private Animator layoutAnimator;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private TextMeshProUGUI displayNameText;
    [SerializeField] private Animator portraitAnimator;
    [Header("Choices UI")][SerializeField] private GameObject[] choices;
    private TextMeshProUGUI[] choicesText;

    private PlayerController player;

    //! Need to add using Ink.Runtime; to use Story type;
    private Story currentStory;
    public bool dialogueIsPlaying { get; private set; }
    private static DialogueManager instance;

    ///! Variables to manage the dialogue UI
    private const string SPEAKER_TAG = "speaker";
    private const string PORTRAIT_TAG = "portrait";
    private const string LAYOUT_TAG = "layout";

    ///! Variables to manage give, take and use item
    private const string GIVEITEM_TAG = "giveItem";
    private const string TAKEITEM_TAG = "takeItem";
    private const string USEITEM_TAG = "useItem";

    ///! Variables to manages quests
    private const string CHANGEQUESTSTATUS_TAG = "changeQuestStatus";

    //! This class can only exist once
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Found more than one Dialogue Manager in the scene");
        }
        instance = this;

    }

    public static DialogueManager GetInstance()
    {
        return instance;
    }

    private void Start()
    {
        player = FindObjectOfType(typeof(PlayerController)) as PlayerController;

        //! Desactivate the dialogue Panel and so dialogueIsPlaying to false, no dialogue playing at start
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);

        //! Get the animator component to manage later modification of the Layout via TAG
        layoutAnimator = dialoguePanel.GetComponent<Animator>();

        //get all choices in the UI and get each corresponding TextMeshProUGUI to display text later
        choicesText = new TextMeshProUGUI[choices.Length];
        int index = 0;
        foreach (GameObject choices in choices)
        {
            choicesText[index] = choices.GetComponentInChildren<TextMeshProUGUI>();
            index++;
        }

    }

    private void Update()
    {
        //! If no dialogue is currently playing, Update fonction has nothing to do
        if (!dialogueIsPlaying)
        {
            return;
        }

        //! handle continuing to the next dialogue
        //! Call Continu story if interaction button pressed
        if (currentStory.currentChoices.Count == 0 && player.interact.action.WasPressedThisFrame())
        {
            ContinueStory();
        }

    }

    public void EnterDialogueMode(TextAsset inkJSON)
    {
        //! Set the inkJSON.text (the PNJ dialogue file) as currentStory
        currentStory = new Story(inkJSON.text);
        //! Activate the daloguePanel and set dialogueIsPlaying to true showing that the player is interacting with a PNJ dialogue
        dialogueIsPlaying = true;
        dialoguePanel.SetActive(true);

        //! As written, call the function to bind all the seleted external function
        //! Use to call C# functions from a Ink file
        BindExternalFunctions(currentStory);

        //! Reset data to not get info from the last dialog
        displayNameText.text = "???";
        portraitAnimator.Play("default");
        layoutAnimator.Play("right");

        ContinueStory();

    }

    private void ExitDialogueMode()
    {
        //Reset everything
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
        dialogueText.text = "";

        UnBindExternalFunctions(currentStory);
    }

    private void ContinueStory()
    {
        //canContinue is true or false, automatic set when creating the Ink file
        if (currentStory.canContinue)
        {
            //! Set text for the current dialogue line
            dialogueText.text = currentStory.Continue();

            //! Display choices, if any, for this dialogue line
            DisplayChoices();
            //! Handle tags in the ink file to use them, they are define as const at the top of this file to be used more easily
            //! currentTags is a intergrated Ink fonction, give tag from this part of the dialogue
            HandleInkTags(currentStory.currentTags);
        }
        else
        {
            //! If canContinu = false, the ink file is over and the dialogue quit
            ExitDialogueMode();
        }
    }

    //! Set each Tag to something, used to pass informations from the inkFile to C# code
    private void HandleInkTags(List<string> currentTags)
    {
        //! loop though each tag and handle it
        foreach (string tag in currentTags)
        {
            //! Parse the tag
            //! Exemple : #speaker:green because speaker as key and green as value
            string[] splitTag = tag.Split(':');
            //defensive test to handle error
            if (splitTag.Length != 2)
            {
                Debug.LogWarning("Tag could not be parsed : " + tag);
            }
            //! .Trim() eliminate space at the begening or the end
            string tagKey = splitTag[0].Trim();
            string tagValue = splitTag[1].Trim();
            switch (tagKey)
            {
                case SPEAKER_TAG:
                    displayNameText.text = tagValue;
                    break;
                case PORTRAIT_TAG:
                    portraitAnimator.Play(tagValue);
                    break;
                case LAYOUT_TAG:
                    layoutAnimator.Play(tagValue);
                    break;
                case GIVEITEM_TAG:
                    //! Exemple of use for this Tag : #giveItem:apple=2 ; #giveItem:itemName=quantity

                    //! !!Here, the name in the Tag must be name of the object Prefab in the ressouce folder !!

                    string[] Split_GIVEITEM_TAG_Value = tagValue.Split('=');
                    string GIVEITEMName = Split_GIVEITEM_TAG_Value[0].Trim();
                    string GIVEITEMQuantity = Split_GIVEITEM_TAG_Value[1].Trim();

                    inventoryManager.GetComponent<GetItem>().SetItemInInventory(GIVEITEMName, int.Parse(GIVEITEMQuantity));
                    break;
                case TAKEITEM_TAG:
                    //! Exemple of use for this Tag : #takeItem:apple=2 ; #takeItem:itemName=quantity
                    string[] Split_TAKEITEM_TAG_Value = tagValue.Split('=');
                    string TAKEITEMName = Split_TAKEITEM_TAG_Value[0].Trim();
                    string TAKEITEMquantity = Split_TAKEITEM_TAG_Value[1].Trim();

                    //! Test if found in inventory
                    if (inventoryManager.GetComponent<Inventory>().IsTheItemInInventory(TAKEITEMName, int.Parse(TAKEITEMquantity)))
                    {
                        //! call the function for this object
                        inventoryManager.GetComponent<UseItem>().RemoveItemFromInventory(TAKEITEMName, int.Parse(TAKEITEMquantity));
                    }
                    else
                    {   //! TO DO Item not in the inventory or not in the right quantity
                        Debug.LogWarning("Could not find the object in the inventory : " + TAKEITEMName);
                    }
                    break;
                case USEITEM_TAG:
                    //! Exemple of use for this Tag : #takeItem:apple=2 ; #takeItem:itemName=quantity
                    string[] Split_USEITEM_TAG_Value = tagValue.Split('=');
                    string USEITEM_TAGName = Split_USEITEM_TAG_Value[0].Trim();
                    string USEITEM_TAGquantity = Split_USEITEM_TAG_Value[1].Trim();
                    //! Find the game object type GeneralItem that has the same name as the tagValue and apply his type
                    //! Meaning that all items must be of type GeneralItem;
                    GeneralItem usedItem = GameObject.Find(USEITEM_TAGName).GetComponent<GeneralItem>();
                    //! Test if found in inventory
                    if (inventoryManager.GetComponent<Inventory>().IsTheItemInInventory(usedItem.name))
                    {
                        //! call the function between "" for this object
                        usedItem.SendMessage("UseFromInventory", int.Parse(USEITEM_TAGquantity));
                    }
                    else
                    {   //! TO DO Item not in the inventory
                        Debug.LogWarning("Could not find the object in the inventory : " + tag);
                    }
                    break;
                case CHANGEQUESTSTATUS_TAG:
                    //! Exemple of use for this Tag : #changeQuestStatus:questID=completed
                    string[] splitCHANGEQUESTSTATUS_TAG_Value = tagValue.Split('=');
                    string questID = splitCHANGEQUESTSTATUS_TAG_Value[0].Trim();
                    string newStatus = splitCHANGEQUESTSTATUS_TAG_Value[1].Trim();
                    questManager.GetComponent<QuestManager>().ChangeQuestStatus(int.Parse(questID), newStatus);
                    break;
                default:
                    Debug.LogWarning("Tag came in the switch but cant be handle : " + tag);
                    break;
            }

        }
    }

    private void DisplayChoices()
    {
        //! Get the choicis associated to this line of dialogue from the inkFile.
        List<Choice> currentChoices = currentStory.currentChoices;

        //! Check if the UI is able to support this number of choices
        if (currentChoices.Count > choices.Length)
        {
            Debug.LogError("More choices given than the UI can support, number given : " + currentChoices.Count);
        }

        int index = 0;

        foreach (Choice choice in currentChoices)
        {
            //! Activate the choices UI to display them
            choices[index].gameObject.SetActive(true);
            choicesText[index].text = choice.text;
            index++;
        }

        //! Go through the remaining choices in the UI to make sure they are not display
        //! Used if the choices in the ink File are < at the number of choices that can be display by the UI

        for (int i = index; i < choices.Length; i++)
        {
            choices[i].gameObject.SetActive(false);
        }

        StartCoroutine(SelectFirstChoice());

    }

    private IEnumerator SelectFirstChoice()
    {
        //! This is part is directly from the tutorial, maybe there is a better way
        //! Unity Event system need to be clear first then select in a different frame
        //! this code allow to choose a choice
        EventSystem.current.SetSelectedGameObject(null);
        yield return new WaitForEndOfFrame();
        EventSystem.current.SetSelectedGameObject(choices[0]);

    }

    public void MakeChoice(int choiceIndex)
    {
        //! In the UI, the choices panels are link to a button, pressing the button call this function
        //! Each button must be set to a different index (aka this button number in the list)
        //! So the index can be get to passe to the Ink file telling it what choice has been made
        currentStory.ChooseChoiceIndex(choiceIndex);
        ContinueStory();
    }

    //! Link to all the external functions
    void BindExternalFunctions(Story currentStory)
    {
        // Use Lamba function to get the string Status
        currentStory.BindExternalFunction("GetQuestStatus", (int questID) =>
        {
            return questManager.GetQuestStatus(questID);
        });

        // Use Lamba function to change a question conditon status
        currentStory.BindExternalFunction("FulfillACondition", (int questID, int conditionIndex) =>
        {
            questManager.FulfillACondition(questID, conditionIndex);
        });

        currentStory.BindExternalFunction("UpdateAllQuestStatus", () =>
        {
            questManager.UpdateAllQuestStatus();
        });

        currentStory.BindExternalFunction("IsTheItemInInventory", (string itemName, int quantity) =>
        {
            inventoryManager.GetComponent<Inventory>().IsTheItemInInventory(itemName, quantity);
        });


    }


    void UnBindExternalFunctions(Story currentStory)
    {
        currentStory.UnbindExternalFunction("GetQuestStatus");
        currentStory.UnbindExternalFunction("FulfillACondition");
        currentStory.UnbindExternalFunction("UpdateAllQuestStatus");
        currentStory.UnbindExternalFunction("IsTheItemInInventory");
    }



}
