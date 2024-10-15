using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Customer : MonoBehaviour
{
    public string order;    // The customer's order
    public bool isServed;   // Flag to check if the customer is served

    // List of available orders
    private static readonly string[] availableOrders = { "Hotdog with Ketchup", "Hotdog with Mustard" };

    // Start is called before the first frame update
    void Start()
    {
        SetRandomOrder(); // Assign a random order to the customer when they spawn
    }

    // Update is called once per frame
    void Update()
    {
        // (Optional) You can add any update logic if necessary
    }

    // Assigns a random order to the customer from the available orders
    public void SetRandomOrder()
    {
        int randomIndex = Random.Range(0, availableOrders.Length);
        order = availableOrders[randomIndex];
        Debug.Log("Customer's order: " + order);
    }

    // Call this when the customer is served
    public void ReceiveOrder()
    {
        isServed = true;  // Set the customer as served
        Debug.Log("Customer served: " + order);
    }

    // Returns whether the customer was served correctly
    public bool ServedCorrect()
    {
        return isServed;  // This checks if the customer was served
    }
}
