using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class InteractiveAssets : MonoBehaviour
{
    [HideInInspector] public static PlayerController player;
    public GeneralItem[] itemsToDisplay;
    public float detectionRadius = 5f; // radius around this object to detect friends
    public LayerMask detectionLayer; // Specify the layer(s) to detect
    private InteractiveAssetsUI interactiveAssetsUI;
    private List<GameObject> detectedObjects = new List<GameObject>();
    private List<GameObject> itemToDisplayPanelUI;
    [HideInInspector] public bool playerInRange;
    private bool interactiveAssetsUIInDisplay;
    [HideInInspector] public string currentZone;
    [HideInInspector] public Coroutine[] detectCoroutine;
    [HideInInspector] public Coroutine[] removeCoroutine;
    [HideInInspector] public bool[] removeCoroutineIsOn;
    [HideInInspector] public bool[] detectCoroutineIsOn;

    //! Is set in child
    public List<FriendsCondition> ConditionsList { get; set; }

    public List<Species> subListSpecies;

    public virtual void Start()
    {

        player = FindObjectOfType(typeof(PlayerController)) as PlayerController;
        interactiveAssetsUI = FindObjectOfType(typeof(InteractiveAssetsUI)) as InteractiveAssetsUI;
        itemToDisplayPanelUI = new List<GameObject>();

        foreach (Transform panel in transform.GetChild(0).gameObject.transform)
        {
            itemToDisplayPanelUI.Add(panel.gameObject);
        }

        playerInRange = false;
        DisplayItems();
    }

    void Update()
    {
        if (playerInRange && player.interact.action.WasPressedThisFrame() && interactiveAssetsUIInDisplay == false)
        {
            interactiveAssetsUI.SetMeActive(GetComponent<SpriteRenderer>().sprite, itemsToDisplay);
            interactiveAssetsUIInDisplay = true;
        }

        int index = 0;

        //! ConditionsList is a sublist of conditions made in each child
        foreach (FriendsCondition condition in ConditionsList)
        {
            if (detectCoroutineIsOn[index] == false)
            {
                condition.CoRoutineInProgress = true;
                detectCoroutineIsOn[index] = true;
                detectCoroutine[index] = StartCoroutine(DetectObjectsPeriodically(condition, index));
            }
            index++;
        }
    }

    //! Display on top of the interactive asset in the game zones
    private void DisplayItems()
    {
        for (int i = 0; i < itemToDisplayPanelUI.Count; i++)
        {
            itemToDisplayPanelUI[i].SetActive(true);

            if (i < itemsToDisplay.Length && itemsToDisplay[i] != null)
            {
                itemToDisplayPanelUI[i].GetComponent<SpriteRenderer>().sprite = itemsToDisplay[i].GetComponent<SpriteRenderer>().sprite;
                //itemToDisplayPanelUI[i].GetComponent<Transform>().localScale = new Vector3(2f, 2f, 0);
                itemToDisplayPanelUI[i].GetComponent<Transform>().localScale = itemsToDisplay[i].GetComponent<Transform>().localScale / 4;
            }
            else
            {
                //! If no item to display, set the panel inactive
                itemToDisplayPanelUI[i].SetActive(false);
            }
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerInRange = false;
            interactiveAssetsUIInDisplay = false;
            interactiveAssetsUI.SetMeInactive();
        }
    }

    public IEnumerator DetectObjectsPeriodically(FriendsCondition condition, int index)
    {
        //Debug.Log("In DetectObjectsPeriodically");

        // Keep detecting objects every 'interval' seconds
        while (true)
        {
            yield return new WaitForSeconds(condition.DetectionTimer);

            ReactToCloseFriends(condition, index);
        }
    }

    public IEnumerator RemoveObjectsPeriodically(string name)
    {
        int index = 0;

        while (true)
        {
            yield return new WaitForSeconds(5f);

            if (index < itemsToDisplay.Length && itemsToDisplay[index] != null && itemsToDisplay[index].name.Contains(name))
            {
                itemsToDisplay[index] = null;
                index++;
            }
            //! Display on the game vue, on the interactiveAsset
            DisplayItems();

            //! Update the display on the UI vue if open
            if (interactiveAssetsUI.gameObject.activeSelf)
            {
                interactiveAssetsUI.SetMeActive(GetComponent<SpriteRenderer>().sprite, itemsToDisplay);
            }

        }
    }

    public int HowManyAreInRadius(string name)
    {
        int quantityInRadius = 0;

        detectedObjects.Clear(); // Clear the list before updating

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, detectionRadius, detectionLayer);

        foreach (Collider2D collider in colliders)
        {
            GameObject obj = collider.gameObject;
            if (!detectedObjects.Contains(obj))
            {
                detectedObjects.Add(obj);
            }
        }

        foreach (var detectedObject in detectedObjects)
        {
            if (detectedObject.name.Contains(name)) { quantityInRadius++; }
        }

        //! -1 because counted itself if this.name == parameter name 
        if (gameObject.name.Contains(name)) { quantityInRadius -= 1; }

        return quantityInRadius;
    }

    //! If friendQuantity friendName in detectionRadius and in the right zone, add an object to the list 
    public void ReactToCloseFriends(FriendsCondition condition, int index)
    {
        List<string> names = new List<string>();

        foreach (GeneralItem item in itemsToDisplay)
        {
            if (item == null) { continue; }
            names.Add(item.name);
        }

        //! if the zone has changed, stop the coroutines started in the previous zone
        if (currentZone != condition.RequiredZone)
        {
            StopCoroutine(detectCoroutine[index]);
            detectCoroutineIsOn[index] = false;
            condition.CoRoutineInProgress = false;
        }

        //! if item from another zone are on the object but can't stay here in the new zone
        if (currentZone != condition.RequiredZone && names.Contains(condition.ItemRewardName) && removeCoroutineIsOn[index] == false)
        {
            removeCoroutine[index] = StartCoroutine(RemoveObjectsPeriodically(condition.ItemRewardName));
            removeCoroutineIsOn[index] = true;
            return;
        }

        //! All the item has been removed, must stop the coroutine
        if (names.Contains(condition.ItemRewardName) == false && removeCoroutineIsOn[index] == true)
        {
            StopCoroutine(removeCoroutine[index]);
            removeCoroutineIsOn[index] = false;
        }

        //! Stop the coroutine if the item[] is full
        if (itemsToDisplay.Contains(null) == false)
        {
            //Debug.Log("Coroutine Stop, full " + name);
            StopCoroutine(detectCoroutine[index]);
            detectCoroutineIsOn[index] = false;
            condition.CoRoutineInProgress = false;
            return;
        }

        int friendsInRadius = 0;
        friendsInRadius = HowManyAreInRadius(condition.FriendName);

        if (friendsInRadius < condition.FriendQuantity && condition.CoRoutineInProgress == true)
        {
            //Debug.Log("Coroutine Stop, no more friend " + name);
            StopCoroutine(detectCoroutine[index]);
            detectCoroutineIsOn[index] = false;
            condition.CoRoutineInProgress = false;
            return;
        }

        if (currentZone == condition.RequiredZone && friendsInRadius == condition.FriendQuantity && itemsToDisplay.Contains(null))
        {
            //! Find the first null index
            int i = Array.FindIndex(itemsToDisplay, item => item == null);

            GeneralItem loadedObject = Resources.Load<GeneralItem>(condition.LoadedObjectAdress);
            if (loadedObject != null)
            { itemsToDisplay[i] = loadedObject; }
            else
            {
                Debug.LogWarning("Could not load the game object at adress " + condition.LoadedObjectAdress);
            }
            //! Display on the interactiveAsset in th game vue;
            DisplayItems();
            //! Update the display on the UI vue if open
            if (interactiveAssetsUI.gameObject.activeSelf) { interactiveAssetsUI.SetMeActive(GetComponent<SpriteRenderer>().sprite, itemsToDisplay); }
            return;
        }
    }

    public void SetZone(string zoneName)
    {
        currentZone = zoneName;
        //Debug.Log("Current zone: " + currentZone);
    }

    public void ClearZone()
    {
        currentZone = null;
        //Debug.Log("No zone");
    }

}