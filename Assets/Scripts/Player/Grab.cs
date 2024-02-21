using UnityEngine;

public class Grab : MonoBehaviour
{
    [SerializeField]
    private Transform grabPoint;

    [SerializeField]
    private Transform rayPoint;

    [SerializeField]
    private float rayDistance;

    private GameObject grabbedObject = null;
    private int LayerIndex;

    void Start()
    {
        LayerIndex = LayerMask.NameToLayer("InteractiveObjects");
    }

    void Update()
    {
        //! Cast a ray though all the object in front
        RaycastHit2D[] hitInfos = Physics2D.RaycastAll(rayPoint.position, transform.right, rayDistance);

        foreach (RaycastHit2D hitInfo in hitInfos)
        {
            if (hitInfo.collider != null && hitInfo.collider.gameObject.layer == LayerIndex)
            {
                //! Grab object
                if (GetComponent<PlayerController>().grab.action.WasPressedThisFrame() && grabbedObject == null)
                {
                    grabbedObject = hitInfo.collider.gameObject;
                    grabbedObject.GetComponent<Rigidbody2D>().isKinematic = true;
                    grabbedObject.GetComponent<Rigidbody2D>().collisionDetectionMode = CollisionDetectionMode2D.Continuous;
                    grabbedObject.transform.position = grabPoint.position;
                    grabbedObject.transform.SetParent(transform);
                    break;
                }
            }
        }

        //! Release the object
        if (GetComponent<PlayerController>().grab.action.WasReleasedThisFrame() && grabbedObject != null)
        {
            grabbedObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;

            grabbedObject.transform.SetParent(null);
            grabbedObject = null;
        }

    }
}
