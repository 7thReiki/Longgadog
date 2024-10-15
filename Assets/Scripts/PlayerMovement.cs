using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D rb;
    public Animator animator; // Ensure this is assigned in the Inspector
    public float movementSpeed = 5f;
    private Vector2 movement;

    public string InputNameHorizontal = "Horizontal"; // Default Unity Input
    public string InputNameVertical = "Vertical"; // Default Unity Input

    // New variable to hold the transform of the item
    public Transform holdItemTransform; // Assign this in the Inspector

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        movement.x = Input.GetAxisRaw(InputNameHorizontal);
        movement.y = Input.GetAxisRaw(InputNameVertical);
        movement = movement.normalized;

        // Update animator parameters if animator is assigned
        if (movement != Vector2.zero && animator != null)
        {
            animator.SetFloat("Horizontal", movement.x);
            animator.SetFloat("Vertical", movement.y);
        }

        // Update the item's position based on the character's facing direction
        UpdateItemPosition();
    }

    private void FixedUpdate()
    {
        rb.velocity = movement * movementSpeed;
    }

    private void UpdateItemPosition()
    {
        if (holdItemTransform != null)
        {
            // Only update item position if the player is moving
            if (movement.x != 0 || movement.y != 0)
            {
                // Set the position of the holdItemTransform based on the character's direction
                if (movement.x > 0) // Moving right
                {
                    holdItemTransform.position = new Vector3(transform.position.x + 0.5f, transform.position.y, transform.position.z);
                    holdItemTransform.localScale = new Vector3(1, 1, 1); // Item facing right
                    holdItemTransform.rotation = Quaternion.Euler(0, 0, 90); // Reset rotation (facing right)
                }
                else if (movement.x < 0) // Moving left
                {
                    holdItemTransform.position = new Vector3(transform.position.x - 0.5f, transform.position.y, transform.position.z);
                    holdItemTransform.localScale = new Vector3(-1, 1, 1); // Item facing left
                    holdItemTransform.rotation = Quaternion.Euler(0, 0, -90); // Rotate 180 degrees (facing left)
                }
                else if (movement.y > 0) // Moving up
                {
                    holdItemTransform.position = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z - 10f); // Item below player
                    holdItemTransform.localScale = new Vector3(1, 1, 1); // Item facing up
                    holdItemTransform.rotation = Quaternion.Euler(0, 0, 180); // Rotate to face up
                }
                else if (movement.y < 0) // Moving down
                {
                    holdItemTransform.position = new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z);
                    holdItemTransform.localScale = new Vector3(1, -1, 1); // Item facing down
                    holdItemTransform.rotation = Quaternion.Euler(0, 0, -180); // Rotate to face down
                }
            }
            // If the player is not moving, do nothing to keep the item in its last position
        }
    }
}
