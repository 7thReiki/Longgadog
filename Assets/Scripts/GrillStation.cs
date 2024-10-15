using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrillStation : MonoBehaviour
{
    public int maxItems = 1; // Number of items the grill can hold
    private GameObject[] itemsOnGrill;
    private float[] itemCookTimers;

    private bool isGrillOccupied = false; // Flag to check if the grill is occupied

    public Sprite cookedHotdogSprite;
    public Sprite cookedBunSprite;
    public Sprite burntHotdogSprite; // New Sprite for burnt hotdogs
    public Sprite burntBunSprite; // New Sprite for burnt buns

    public float overcookTimeHotdog = 10f; // Time limit for overcooking hotdogs
    public float overcookTimeBun = 6f; // Time limit for overcooking buns

    public int nonInteractableLayer;
    public int interactableLayer;

    void Start()
    {
        itemsOnGrill = new GameObject[maxItems]; // Array to hold items on the grill      
        itemCookTimers = new float[maxItems]; // Timers for each item cooking on the grill
    }

    void Update()
    {
        if (isGrillOccupied)
        {
            for (int i = 0; i < maxItems; i++)
            {
                if (itemsOnGrill[i] != null)
                {
                    itemCookTimers[i] += Time.deltaTime;

                    // Hotdog cooking and burning logic
                    if (itemsOnGrill[i].CompareTag("Hotdog"))
                    {
                        // Check for cooked hotdog
                        if (itemCookTimers[i] >= 5f && !itemsOnGrill[i].CompareTag("CookedHotdog") && !itemsOnGrill[i].CompareTag("Burnt"))
                        {
                            CookItem(i, cookedHotdogSprite, "CookedHotdog");
                        }
                        // Check for burnt hotdog
                        else if (itemCookTimers[i] >= overcookTimeHotdog && !itemsOnGrill[i].CompareTag("Burnt"))
                        {
                            BurnItem(i, burntHotdogSprite);
                        }
                    }
                    // Bun cooking and burning logic
                    else if (itemsOnGrill[i].CompareTag("Bun"))
                    {
                        // Check for cooked bun
                        if (itemCookTimers[i] >= 3f && !itemsOnGrill[i].CompareTag("CookedBun") && !itemsOnGrill[i].CompareTag("Burnt"))
                        {
                            CookItem(i, cookedBunSprite, "CookedBun");
                        }
                        // Check for burnt bun
                        else if (itemCookTimers[i] >= overcookTimeBun && !itemsOnGrill[i].CompareTag("Burnt"))
                        {
                            BurnItem(i, burntBunSprite);
                        }
                    }
                }
            }
        }
    }



    public bool IsGrillOccupied()
    {
        return isGrillOccupied;
    }

    // Method to place an item on the grill
    public void PlaceItem(GameObject item)
    {
        for (int i = 0; i < maxItems; i++)
        {
            if (itemsOnGrill[i] == null) // Find an empty slot
            {
                itemsOnGrill[i] = item; // Place the item in the slot
                itemCookTimers[i] = 0f; // Reset the timer for this item
                item.transform.SetParent(transform); // Attach to grill station
                item.transform.localPosition = Vector3.zero; // Reset position
                item.layer = nonInteractableLayer; // Make item non-interactable while cooking

                isGrillOccupied = true; // Set the grill as occupied
                Debug.Log(item.name + " is placed on the grill.");
                return;
            }
        }
        Debug.Log("Cannot place item on grill, it's already occupied!");
    }

    // Method to get a cooked item from the grill
    public GameObject GetCookedItem()
    {
        for (int i = 0; i < maxItems; i++)
        {
            if (itemsOnGrill[i] != null &&
                (itemsOnGrill[i].CompareTag("CookedHotdog") || itemsOnGrill[i].CompareTag("CookedBun")))
            {
                return itemsOnGrill[i]; // Return the cooked item
            }
        }
        return null;
    }

    // Method to release a slot and reset the grill status if necessary
    public void ReleaseSlot(GameObject item)
    {
        for (int i = 0; i < maxItems; i++)
        {
            if (itemsOnGrill[i] == item) // Find the item in the grill slots
            {
                itemsOnGrill[i] = null; // Clear the item from the grill
                itemCookTimers[i] = 0f; // Reset the cooking timer for this slot
                Debug.Log(item.name + " has been removed from the grill, checking if slots are empty.");
                break;
            }
        }

        // Check if all slots are empty
        bool allSlotsEmpty = true;
        for (int i = 0; i < maxItems; i++)
        {
            if (itemsOnGrill[i] != null)
            {
                allSlotsEmpty = false; // If any slot is occupied, break the loop
                break;
            }
        }

        if (allSlotsEmpty)
        {
            isGrillOccupied = false; // Reset the flag only if all slots are empty
            Debug.Log("All slots are empty, grill is now free.");
        }
    }

    // Method to cook an item on the grill
    void CookItem(int index, Sprite cookedSprite, string newTag)
    {
        if (itemsOnGrill[index] != null && itemsOnGrill[index].GetComponent<SpriteRenderer>() != null)
        {
            itemsOnGrill[index].GetComponent<SpriteRenderer>().sprite = cookedSprite; // Change the sprite to the cooked version
            itemsOnGrill[index].tag = newTag; // Update the item's tag to "Cooked"
            itemsOnGrill[index].layer = interactableLayer; // Make the item interactable again
            Debug.Log(itemsOnGrill[index].name + " is cooked!"); // Debug log for cooked item
        }
    }

    // Method to burn an item on the grill
    void BurnItem(int index, Sprite burntSprite)
    {
        if (itemsOnGrill[index] != null && itemsOnGrill[index].GetComponent<SpriteRenderer>() != null)
        {
            itemsOnGrill[index].GetComponent<SpriteRenderer>().sprite = burntSprite; // Change the sprite to the burnt version
            itemsOnGrill[index].tag = "Burnt"; // Update the item's tag to "Burnt"
            itemsOnGrill[index].layer = interactableLayer; // Make the burnt item interactable again
            Debug.Log(itemsOnGrill[index].name + " is burnt!"); // Debug log for burnt item
        }
    }
}
