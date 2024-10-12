using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Customer : MonoBehaviour
{
    public string order;
    public bool isServed;

    private static readonly string[] availableOrders = { "Hotdog with Ketchup", "Hotdog with Mustard" }
    ;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetRandomOrder()
    {
        int randomIndex = Random.Range(0, availableOrders.Length);
        order = availableOrders[randomIndex];
    }

    public void ReceiveOrder()
    {
        isServed = true;
        Debug.Log("Customer served: " + order);
    }
}
