using UnityEngine;
using System.Collections.Generic;

public class ObjectDetector : MonoBehaviour
{
    public float detectionRadius = 5f; // Adjust as needed
    public LayerMask detectionLayer; // Specify the layer(s) to detect

    private List<GameObject> detectedObjects = new List<GameObject>();

    void Update()
    {
        DetectObjects();
    }

    public void DetectObjects()
    {
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

        // Now you have a list of detected GameObjects around the current object
        // You can use this list for further processing
    }
}
