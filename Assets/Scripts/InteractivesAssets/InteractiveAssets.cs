using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class InteractiveAssets : MonoBehaviour
{
    [HideInInspector] public static PlayerController player;
    public List<GameObject> speciesToDisplay = new List<GameObject>();

    private int speciesToDisplayListMax;
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
    public List<FriendsCondition> ConditionsList;
    [HideInInspector] public bool zoomInToStudy;

    public virtual void Start()
    {

        player = FindObjectOfType(typeof(PlayerController)) as PlayerController;
        interactiveAssetsUI = FindObjectOfType(typeof(InteractiveAssetsUI)) as InteractiveAssetsUI;
        itemToDisplayPanelUI = new List<GameObject>();

        foreach (Transform panel in transform.GetChild(0).gameObject.transform)
        {
            itemToDisplayPanelUI.Add(panel.gameObject);
        }

        speciesToDisplayListMax = itemToDisplayPanelUI.Count;

        if (speciesToDisplay.Count < speciesToDisplayListMax)
        {
            for (int i = speciesToDisplay.Count; i < speciesToDisplayListMax; i++)
            {
                speciesToDisplay.Add(null);
            }
        }



        playerInRange = false;
        DisplayItems();
    }

    void Update()
    {
        if (zoomInToStudy)
        {
            if (playerInRange && player.interact.action.WasPressedThisFrame() && interactiveAssetsUIInDisplay == false)
            {
                interactiveAssetsUI.SetMeActive(GetComponent<SpriteRenderer>().sprite, speciesToDisplay);
                interactiveAssetsUIInDisplay = true;
            }
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

            if (i < speciesToDisplay.Count && speciesToDisplay[i] != null)
            {
                itemToDisplayPanelUI[i].GetComponent<SpriteRenderer>().sprite = speciesToDisplay[i].GetComponent<SpriteRenderer>().sprite;
                //itemToDisplayPanelUI[i].GetComponent<Transform>().localScale = new Vector3(2f, 2f, 0);
                itemToDisplayPanelUI[i].GetComponent<Transform>().localScale = speciesToDisplay[i].GetComponent<Transform>().localScale / 4;
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
        // Keep detecting objects every 'interval' seconds
        while (true)
        {
            yield return new WaitForSeconds(condition.DetectionTimer);

            ReactToCloseFriends(condition, index);
        }
    }

    public IEnumerator RemoveObjectsPeriodically(string name, int i)
    {
        int index = 0; // Indice de l'objet à supprimer

        while (index < speciesToDisplay.Count)
        {
            yield return new WaitForSeconds(5f);

            if (speciesToDisplay[index] != null && speciesToDisplay[index].name.Contains(name))
            {
                speciesToDisplay[index] = null;
            }

            DisplayItems();

            if (interactiveAssetsUI.gameObject.activeSelf)
            {
                interactiveAssetsUI.SetMeActive(GetComponent<SpriteRenderer>().sprite, speciesToDisplay);
            }

            index++;
        }

    }

    public bool CorrectAmountOf1FriendInRadius(int quantity, string name)
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

        if (quantityInRadius == quantity) { return true; }

        return false;
    }

    public bool AreAllInRadius(List<string> names)
    {
        if (names.Count == 0) { return false; }

        // Clear the list before updating
        detectedObjects.Clear();

        // Get all colliders in the detection radius
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, detectionRadius, detectionLayer);

        // Add unique game objects to the detected objects list
        foreach (Collider2D collider in colliders)
        {
            GameObject obj = collider.gameObject;
            if (!detectedObjects.Contains(obj))
            {
                detectedObjects.Add(obj);
            }
        }

        // Check if all objects in the names list are present in the detected objects
        foreach (string name in names)
        {
            bool found = false;
            foreach (var detectedObject in detectedObjects)
            {
                if (detectedObject.name.Contains(name))
                {
                    found = true;
                    break; // Exit inner loop if the object is found
                }
            }

            // If the object is not found, return false immediately
            if (!found)
            {
                return false;
            }
        }

        // If all objects are found, return true
        return true;
    }


    //! If friendQuantity friendName in detectionRadius and in the right zone, add an object to the list 
    public void ReactToCloseFriends(FriendsCondition condition, int index)
    {
        List<string> names = new List<string>();

        foreach (GameObject item in speciesToDisplay)
        {
            if (item == null) { continue; }
            names.Add(item.name);
            //Debug.Log(" Foreach Start" + item.name);
        }

        bool correctZone;

        if (condition.RequiredZone != "")
        {
            correctZone = currentZone == condition.RequiredZone;
        }
        else
        {
            correctZone = true;
        }

        bool rewardCurrentlyDisplayed = names.Contains(condition.ItemRewardName);
        bool spotAvailable = speciesToDisplay.Contains(null);
        bool rewardCurrentlyBeenRemoved = removeCoroutineIsOn[index] == true;
        bool rewardCurrentlyBeenAdded = condition.CoRoutineInProgress == true;
        bool allDifferentFriendsAround = AreAllInRadius(condition.FriendsList);
        bool correctAmountOf1FriendAround = CorrectAmountOf1FriendInRadius(condition.FriendQuantity, condition.FriendName);

        // Debug.Log("List friend in " + name + " : " + allDifferentFriendsAround);
        // Debug.Log("Friend in " + name + " : " + correctAmountOf1FriendAround);

        // Debug.Log("STEP 1");

        //! if the zone has changed, stop the coroutines started in the previous zone
        if (!correctZone)
        {
            StopCoroutine(detectCoroutine[index]);
            detectCoroutineIsOn[index] = false;
            condition.CoRoutineInProgress = false;
        }

        // Debug.Log("STEP 2");


        //! if item from another zone are on the object but can't stay here in the new zone
        if (!correctZone && rewardCurrentlyDisplayed && !rewardCurrentlyBeenRemoved)
        {
            removeCoroutine[index] = StartCoroutine(RemoveObjectsPeriodically(condition.ItemRewardName, index));
            removeCoroutineIsOn[index] = true;
            return;
        }

        // Debug.Log("STEP 3");


        //! All the item has been removed, must stop the coroutine
        if (!rewardCurrentlyDisplayed && rewardCurrentlyBeenRemoved)
        {
            StopCoroutine(removeCoroutine[index]);
            removeCoroutineIsOn[index] = false;
        }

        // Debug.Log("STEP 4");


        //! Stop the coroutine if the item[] is full
        if (!spotAvailable && speciesToDisplay.Count != 0)
        {
            //Debug.Log("Coroutine Stop, full " + name);
            StopCoroutine(detectCoroutine[index]);
            detectCoroutineIsOn[index] = false;
            condition.CoRoutineInProgress = false;
            return;
        }

        if (condition.ItemRewardName == "Shell")
        {
            Debug.Log("List friend for " + condition.ItemRewardName + " : " + allDifferentFriendsAround);
            Debug.Log("Friend in " + condition.ItemRewardName + " : " + correctAmountOf1FriendAround);
            Debug.Log("correctZone in " + condition.ItemRewardName + " : " + correctZone);
            Debug.Log("Spot avaiblabe in " + name + " : " + spotAvailable);
            Debug.Log("rewardCurrentlyBeenAdded in " + name + " : " + rewardCurrentlyBeenAdded);
        }


        //! Stop the cortoutine if not the right amount of friends around anymore
        if (!correctAmountOf1FriendAround && !allDifferentFriendsAround && rewardCurrentlyBeenAdded)
        {
            //Debug.Log("Coroutine Stop, no more friend " + name);
            StopCoroutine(detectCoroutine[index]);
            detectCoroutineIsOn[index] = false;
            condition.CoRoutineInProgress = false;
            return;
        }



        Debug.Log("STEP 6");


        if (correctZone && correctAmountOf1FriendAround || allDifferentFriendsAround && spotAvailable)
        {
            //! Find the first null index
            int i = speciesToDisplay.FindIndex(item => item == null);

            GameObject loadedObject = Resources.Load<GameObject>(condition.LoadedObjectAdress);
            if (loadedObject != null)
            { speciesToDisplay[i] = loadedObject; }
            else
            {
                Debug.LogWarning("Could not load the game object at adress " + condition.LoadedObjectAdress);
            }
            //! Display on the interactiveAsset in th game vue;
            DisplayItems();
            //! Update the display on the UI vue if open
            if (interactiveAssetsUI.gameObject.activeSelf) { interactiveAssetsUI.SetMeActive(GetComponent<SpriteRenderer>().sprite, speciesToDisplay); }
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