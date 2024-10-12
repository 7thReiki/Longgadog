using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssemblyStation : MonoBehaviour
{
    private GameObject bunOnStation;
    private GameObject hotdogOnStation;
    private GameObject condimentOnStation;

    public Transform bunSpot;
    public Transform hotdogSpot;
    public Transform condimentSpot;

    public Customer currentCustomer;

    // Start is called before the first frame update
    void Start()
    {
        currentCustomer = new GameObject("Customer").AddComponent<Customer>();
        currentCustomer.SetRandomOrder();
        Debug.Log("Customer order: " + currentCustomer.order);
    }

    // Update is called once per frame
    void Update()
    {
        if(IsAssemblyComplete() && currentCustomer != null && !currentCustomer.isServed)
        {
            ServeCustomer();
        }
    }

    public void PlaceItem(GameObject item)
    {
        if (item.CompareTag("CookedBun") && bunOnStation == null)
        {
            bunOnStation = item;
            item.transform.position = bunSpot.position;
            item.transform.SetParent(bunSpot);
        }
        else if (item.CompareTag("CookedHotdog") && hotdogOnStation == null && bunOnStation != null)
        {
            hotdogOnStation = item;
            item.transform.position = hotdogSpot.position;
            item.transform.SetParent(hotdogSpot);
        }
        else if ((item.CompareTag("Ketchup") || item.CompareTag("Mustard")) && condimentOnStation == null && hotdogOnStation != null)
        {
            condimentOnStation = item;
            item.transform.position = condimentSpot.position;
            item.transform.SetParent(condimentSpot);
        }
        else
        {
            Debug.Log("Cannot place this item here or item already exists. ");
        }
    }

    private bool IsAssemblyComplete()
    {
        return bunOnStation != null && hotdogOnStation != null && condimentOnStation != null;
    }

    private void ServeCustomer()
    {
        // Check if the assembled item matches the customer's order
        string assembledHotdog = "Hotdog with " + (condimentOnStation.CompareTag("Ketchup") ? "Ketchup" : "Mustard");
        if (currentCustomer.order == assembledHotdog)
        {
            currentCustomer.ReceiveOrder();  // Serve the current customer
            Debug.Log("Order served: " + assembledHotdog);
        }
        else
        {
            Debug.Log("Order mismatch! Customer ordered: " + currentCustomer.order);
        }

        // Reset the assembly station for the next order
        ResetStation();
    }


    private void ResetStation()
    {
        bunOnStation = null;
        hotdogOnStation = null;
        condimentOnStation = null;

        if(bunSpot.childCount > 0)
        {
            foreach(Transform child in bunSpot)
            {
                Destroy(child.gameObject);
            }
        }

        if(hotdogSpot.childCount > 0)
        {
            foreach(Transform child in hotdogSpot)
            {
                Destroy(child.gameObject);
            }
        }

        if(condimentSpot.childCount > 0)
        {
            foreach(Transform child in condimentSpot)
            {
                /*Destroy(child.gameObject);*/
            }
        }
    }
}
