using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssemblyStation : MonoBehaviour
{
    public GameObject condimentKetchup;
    public GameObject condimentMustard;
    public Transform ketchupSpot;
    public Transform mustardSpot;

    private GameObject bunOnStation;
    private GameObject hotdogOnStation;
    private GameObject condimentOnStation;

    public Transform bunSpot;
    public Transform hotdogSpot;
    public Transform condimentSpot;

    public Customer currentCustomer;

    private Vector3 originalCondimentPos;

    // Start is called before the first frame update
    void Start()
    {
        SpawnMustard();
        SpawnKetchup();

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
            /*item.transform.SetParent(bunSpot);*/
        }
        else if (item.CompareTag("CookedHotdog") && hotdogOnStation == null && bunOnStation != null)
        {
            hotdogOnStation = item;
            item.transform.position = hotdogSpot.position;
            /*item.transform.SetParent(hotdogSpot);*/
        }
        else if ((item.CompareTag("Ketchup") || item.CompareTag("Mustard")) && condimentOnStation == null && hotdogOnStation != null)
        {
            condimentOnStation = item;
            item.transform.position = condimentSpot.position;
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
            DestroyAssemblyItems();

            if (condimentOnStation.CompareTag("Ketchup"))
            {
                SpawnKetchup();
            }
            else if (condimentOnStation.CompareTag("Mustard"))
            {
                SpawnMustard();
            }
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

        currentCustomer = new GameObject("Customer").AddComponent<Customer>();
        currentCustomer.SetRandomOrder();
        Debug.Log("Customer order: " + currentCustomer.order);
    }

    private void DestroyAssemblyItems()
    {
        if (bunOnStation != null)
        {
            Destroy(bunOnStation);
        }

        if (hotdogOnStation != null)
        {
            Destroy(hotdogOnStation);
        }

        if (condimentOnStation != null)
        {
            Destroy(condimentOnStation);
        }
    }

    private void SpawnKetchup()
    {
        Instantiate(condimentKetchup, ketchupSpot.position, transform.rotation);
    }

    private void SpawnMustard()
    {
        Instantiate(condimentMustard, mustardSpot.position, transform.rotation);
    }
}
