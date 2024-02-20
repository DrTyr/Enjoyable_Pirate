using System.Collections;
using System.Collections.Generic;
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
    // Start is called before the first frame update
    void Start()
    {
        LayerIndex = LayerMask.NameToLayer("InteractiveObjects");
    }

    // Update is called once per frame
    void Update()
    {

        RaycastHit2D hitInfo = Physics2D.Raycast(rayPoint.position, transform.right, rayDistance);

        //Debug.Log("Dans le main");

        //if (hitInfo.collider != null && hitInfo.collider.transform.parent != null && hitInfo.collider.transform.parent.gameObject.layer == LayerIndex)
        if (hitInfo.collider != null && hitInfo.collider.gameObject.layer == LayerIndex)
        {
            //! Grab object
            if (GetComponent<PlayerController>().grab.action.WasPressedThisFrame() && grabbedObject == null)
            {
                Debug.Log("dans le if");

                grabbedObject = hitInfo.collider.gameObject;
                grabbedObject.GetComponent<Rigidbody2D>().isKinematic = true;
                grabbedObject.GetComponent<Rigidbody2D>().collisionDetectionMode = CollisionDetectionMode2D.Continuous;
                grabbedObject.transform.position = grabPoint.position;
                grabbedObject.transform.SetParent(transform);
            }


            Debug.DrawRay(rayPoint.position, transform.right * rayDistance);


        }

        //! Release the object
        if (GetComponent<PlayerController>().grab.action.WasReleasedThisFrame() && grabbedObject != null)
        {
            Debug.Log("dans le else");
            //grabbedObject.GetComponent<Rigidbody2D>().isKinematic = false;
            grabbedObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;

            grabbedObject.transform.SetParent(null);
            grabbedObject = null;
        }

    }
}
