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

    public Coroutine detectCoroutine;


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
    }

    private void DisplayItems()
    {
        //Debug.Log(itemToDisplayPanelUI);

        for (int i = 0; i < itemToDisplayPanelUI.Count; i++)
        {
            itemToDisplayPanelUI[i].SetActive(true);

            if (i < itemsToDisplay.Length && itemsToDisplay[i] != null)
            {
                itemToDisplayPanelUI[i].GetComponent<SpriteRenderer>().sprite = itemsToDisplay[i].GetComponent<SpriteRenderer>().sprite;
                itemToDisplayPanelUI[i].GetComponent<Transform>().localScale = new Vector3(2f, 2f, 0);
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

    public IEnumerator DetectObjectsPeriodically(FriendsCondition condition)
    {
        // Keep detecting objects every 'interval' seconds
        while (true)
        {
            yield return new WaitForSeconds(condition.DetectionTimer);

            ReactToCloseFriends(
             condition.FriendName,
             condition.FriendQuantity,
             condition.LoadedObjectAdress,
             condition.RequiredZone);
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

        return quantityInRadius;
    }

    //! If friendQuantity friendName in detectionRadius and in the right zone, add an object to the list 
    public void ReactToCloseFriends(string friendName, int friendQuantity, string loadedObjectAdress, string requiredZone)
    {
        if (currentZone == requiredZone)
        {
            //! Stop the coroutine if the item is full
            if (itemsToDisplay.Contains(null) == false)
            {
                Debug.Log("Coroutine Stop, full");
                StopCoroutine(detectCoroutine);
                return;
            }

            int friendsInRadius = HowManyAreInRadius(friendName);

            if (friendsInRadius < friendQuantity)
            {
                Debug.Log("Coroutine Stop, no more friend");
                StopCoroutine(detectCoroutine);
                return;
            }

            if (friendsInRadius >= friendQuantity && itemsToDisplay.Contains(null))
            {
                //! Find the first null index
                int index = Array.FindIndex(itemsToDisplay, item => item == null);

                GeneralItem loadedObject = Resources.Load<GeneralItem>(loadedObjectAdress);
                if (loadedObject != null)
                { itemsToDisplay[index] = loadedObject; }
                else
                {
                    Debug.LogWarning("Could not load the game object at adress " + loadedObjectAdress);
                }
                DisplayItems();
                return;
            }
        }
    }

    public void RemoveThisItem(string name)
    {
        //Debug.Log("Inside RemoveThisItem");

        for (int i = 0; i < itemsToDisplay.Length; i++)
        {
            if (itemsToDisplay[i] != null && itemsToDisplay[i].name.Contains(name))
            {
                //Debug.Log("itemsToDisplay[i].name = " + itemsToDisplay[i].name);
                itemsToDisplay[i] = null;
            }
        }

        DisplayItems();
    }

    public void SetZone(string zoneName)
    {
        currentZone = zoneName;
        Debug.Log("Current zone: " + currentZone);
    }

    public void ClearZone()
    {
        currentZone = null;
        Debug.Log("No zone");
    }

    public string GetZone()
    {
        return currentZone;
    }

}