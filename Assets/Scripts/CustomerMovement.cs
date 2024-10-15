using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerMovement : MonoBehaviour
{
    public Transform currentTarget; // This will now directly reference the Transform
    public float moveSpeed = 3f;
    private Rigidbody2D rb;
    private Customer customer;


    // Start is called before the first frame update
    void Start()
    {
        customer = GameObject.FindGameObjectWithTag("Customer").GetComponent<Customer>();
        currentTarget = GameObject.FindGameObjectWithTag("Counter").GetComponent<Transform>();
        rb = GetComponent<Rigidbody2D>();
        
        // Find the BaseTarget in the scene
        GameObject CounterPosition = GameObject.Find("CounterPosition"); // Ensure this name matches your target GameObject

        if (CounterPosition != null)
        {
            currentTarget = CounterPosition.transform; // Set the current target to the base target's transform
        }
        else
        {
            Debug.LogWarning("BaseTarget not found in the scene. Ensure it's named correctly.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (currentTarget != null)
        {
            Vector3 direction = (currentTarget.position - transform.position).normalized;
            MoveTowardsTarget(direction);
        }
    }

    void MoveTowardsTarget(Vector3 direction)
    {
        rb.velocity = direction * moveSpeed;
    }

    void OrderComplete()
    {
       /* Customer customer = GetComponent<Customer>();*/
        if(customer.ServedCorrect())
        {
            Vector3 moveRight = Vector3.right; // (1, 0, 0) in world coordinates

            // Move the customer to the right
            rb.velocity = moveRight * moveSpeed;
        }
    }
}
