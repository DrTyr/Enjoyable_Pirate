using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class InteractiveAssets : MonoBehaviour
{
    [HideInInspector] public static PlayerController player;
    public GeneralItem[] itemsToDisplay;
    public float detectionRadius = 5f; // Adjust as needed
    public LayerMask detectionLayer; // Specify the layer(s) to detect
    //public float detectionTimer = 10f;
    private InteractiveAssetsUI interactiveAssetsUI;
    private List<GameObject> detectedObjects = new List<GameObject>();
    private List<GameObject> itemToDisplayPanelUI;

    [HideInInspector] public bool playerInRange;
    private bool interactiveAssetsUIInDisplay;


    //private string friendName;

    //private int friendQuantity;

    public virtual void Start()
    {
        player = FindObjectOfType(typeof(PlayerController)) as PlayerController;
        interactiveAssetsUI = FindObjectOfType(typeof(InteractiveAssetsUI)) as InteractiveAssetsUI;
        //interactiveAssetsUI = InteractiveAssetsUI.GetInstance();
        itemToDisplayPanelUI = new List<GameObject>();

        foreach (Transform panel in transform.GetChild(0).gameObject.transform)
        {
            itemToDisplayPanelUI.Add(panel.gameObject);
        }

        playerInRange = false;

        //Debug.Log(interactiveAssetsUI);

        DisplayItems();

        // Start the coroutine to perform detection every n seconds
        //StartCoroutine(DetectObjectsPeriodically(detectionTimer, this.friendName, this.friendQuantity));

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

        //Debug.Log("playerNear : " + playerNear);
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerInRange = false;
            interactiveAssetsUIInDisplay = false;
            interactiveAssetsUI.SetMeInactive();

        }

        //playerInRange = false;


    }

    public IEnumerator DetectObjectsPeriodically(float interval, List<string> friendName, List<int> friendQuantity, List<string> loadedObjectAdress)
    {
        // Keep detecting objects every 'interval' seconds
        while (true)
        {
            yield return new WaitForSeconds(interval);

            if (friendName.Count == friendQuantity.Count && friendQuantity.Count == loadedObjectAdress.Count)
            {
                for (int i = 0; i < friendName.Count; i++)
                {
                    ReactToCloseFriends(friendName[i], friendQuantity[i], loadedObjectAdress[i]);
                }

            }

        }
    }

    public int HowManyAreInRadius(string name)
    {
        int quantityInRadius = 0;

        //Debug.Log("In HowManyAreInRadius");
        detectedObjects.Clear(); // Clear the list before updating

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, detectionRadius, detectionLayer);
        // Use Physics.OverlapSphere for 3D detection

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

    //! If friendQuantity friendName or more in detectionRadius, add an object to the list 
    public void ReactToCloseFriends(string friendName, int friendQuantity, string loadedObjectAdress)
    {

        if (HowManyAreInRadius(friendName) >= friendQuantity && itemsToDisplay.Contains(null))
        {
            //Debug.Log(friendQuantity + " " + friendName + " around");

            //! Find the first null index
            int index = System.Array.FindIndex(itemsToDisplay, item => item == null);

            GeneralItem loadedObject = Resources.Load<GeneralItem>(loadedObjectAdress);
            //GeneralItem loadedObject = Resources.Load<GeneralItem>("Items/" + "HealthCollectible");
            if (loadedObject != null)
            { itemsToDisplay[index] = loadedObject; }
            else
            {
                Debug.LogWarning("Could not load the game object at adress " + loadedObjectAdress);
            }

            DisplayItems();

        }

    }

}