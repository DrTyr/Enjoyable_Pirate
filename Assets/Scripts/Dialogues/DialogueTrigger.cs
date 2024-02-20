using UnityEngine;
using UnityEngine.InputSystem;

public class DialogueTrigger : MonoBehaviour
{
    [Header("Visual Cue [Optional]")]
    [SerializeField] private GameObject visualCue;

    [Header("Ink JSON")]
    [SerializeField] private TextAsset inkJSON;

    [Header("Input Manager : interact")]
    [SerializeField] private InputActionReference interact;

    //PlayerInput playerInput;

    private PlayerController player;

    private bool alreadyTalked;

    private bool playerInRange;

    private void Awake()
    {
        playerInRange = false;
        if (visualCue != null) { visualCue.SetActive(false); }
        alreadyTalked = false;
        //playerInput = new PlayerInput();

        player = FindObjectOfType(typeof(PlayerController)) as PlayerController;


    }

    private void Update()
    {

        if (playerInRange && !DialogueManager.GetInstance().dialogueIsPlaying)
        {
            if (visualCue != null) { visualCue.SetActive(true); }

            //bool isSpaceKeyHeld = playerInput.Player.Interact.ReadValue<float>() > 0.1f;

            //! TO DO : need to find as way to use the new input system
            //if (interact.action.IsPressed() && !alreadyTalked)

            //! Start the dialogue if in range of the PNJ, pass the inKJSON file inside the PNJ to be used bay the DialogueManager
            if (player.interact.action.WasPressedThisFrame() && !alreadyTalked)
            {
                //! alreadyTalked is used to not be able to talk again to PNJ without leavinf it's trigger first
                //! Made to not loop on the same dialogue if clicking too fast
                alreadyTalked = true;
                DialogueManager.GetInstance().EnterDialogueMode(inkJSON);
            }
        }
        else
        {
            if (visualCue != null) { visualCue.SetActive(false); }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerInRange = false;
            alreadyTalked = false;
        }

    }



}
