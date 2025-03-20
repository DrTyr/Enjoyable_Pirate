using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crabs : MonoBehaviour
{

    //private Vector3 crabPosition;
    //private Vector3 startPosition;
    public Vector2 endPosition;
    private float speed = 2.0f;
    //private bool mustMove;
    public bool isMoving;

    void FixedUpdate()
    {
        //Debug.Log("Is moving : " + isMoving);

        if (isMoving)
        {
            Move();
        }

    }

    private void Move()
    {
        //Debug.Log("moves");

        if (transform.position.x == endPosition.x && transform.position.y == endPosition.y)
        {
            isMoving = false;
            Destroy(gameObject);
        }
        transform.position = Vector2.MoveTowards(transform.position, endPosition, speed * Time.deltaTime);

    }


}
