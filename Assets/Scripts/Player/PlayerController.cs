using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    public InputActionReference movement, interact, grab;
    //Rigidbody2D rigidbody2d;
    private AgentMover agentMover;
    private Vector2 movementInput;
    private Animator animator;
    public GameObject rayPosition;

    // Start is called before the first frame update
    void Start()
    {
        //rigidbody2d = GetComponent<Rigidbody2D>();
        agentMover = GetComponent<AgentMover>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //nothing can happen here if a dialogue is playing;
        if (DialogueManager.GetInstance().dialogueIsPlaying)
        {
            return;
        }

        Movement();

    }

    void Movement()
    {

        movementInput = movement.action.ReadValue<Vector2>();
        agentMover.movementInput = movementInput;


        // Debug.Log("X : " + movementInput.x);
        // Debug.Log("Y : " + movementInput.y);

        // Déterminer si le personnage regarde vers la droite ou la gauche
        int direction = movementInput.x > 0 ? 1 : -1;

        // Appliquer le décalage du côté opposé en fonction de la direction
        Vector3 newLocalPosition = rayPosition.GetComponent<Transform>().localPosition;
        newLocalPosition.x = Mathf.Abs(newLocalPosition.x) * direction * -1;
        rayPosition.GetComponent<Transform>().localPosition = newLocalPosition;


        animator.SetFloat("X", movementInput.x);
        animator.SetFloat("Y", movementInput.y);


    }

    void FixedUpdate()
    {
        //nothing can happen here if a dialogue is playing;
        if (DialogueManager.GetInstance().dialogueIsPlaying)
        {
            return;
        }

    }

}
