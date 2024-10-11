using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D rb;
    public float movementSpeed = 5f;
    private Vector2 movement;

    public string InputNameHorizontal;
    public string InputNameVertical;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        movement.x = Input.GetAxisRaw(InputNameHorizontal);
        movement.y = Input.GetAxisRaw(InputNameVertical);
        movement = movement.normalized;
    }

    private void FixedUpdate()
    {
        rb.velocity = movement * movementSpeed;
    }
}