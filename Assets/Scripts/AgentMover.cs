using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentMover : MonoBehaviour
{
    private Rigidbody2D rgb2d;

    [SerializeField]
    private float maxSpeed = 8, acceleration = 50, decceleration = 100;

    [SerializeField] private float currentSpeed = 0;

    private Vector2 oldMovementInput;

    public Vector2 movementInput { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        rgb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (movementInput.magnitude > 0 && currentSpeed >= 0)
        {
            oldMovementInput = movementInput;
            currentSpeed += acceleration * maxSpeed * Time.deltaTime;
        }
        else
        {
            currentSpeed -= decceleration * maxSpeed * Time.deltaTime;
        }

        currentSpeed = Mathf.Clamp(currentSpeed, 0, maxSpeed);
        rgb2d.velocity = oldMovementInput * currentSpeed;

    }
}
