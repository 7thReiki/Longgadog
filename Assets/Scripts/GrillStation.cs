using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrillStation : MonoBehaviour
{
    public int maxItems = 4;  // Maximum number of items that can be on the grill
    private List<GameObject> itemsOnGrill = new List<GameObject>();  // List to store items on the grill
    private Dictionary<GameObject, float> itemCookTimers = new Dictionary<GameObject, float>();  // Track cooking time for each item

    public Sprite cookedHotdogSprite;  // Sprite for the cooked hotdog
    public Sprite cookedBunSprite;     // Sprite for the cooked bun

    public int nonInteractableLayer;
    public int interactableLayer;

    public Transform[] hotdogSlots;   // Array for hotdog slots
    public Transform[] bunSlots;      // Array for bun slots
    private bool[] hotdogSlotOccupied;  // Flags for hotdog slot occupancy
    private bool[] bunSlotOccupied;     // Flags for bun slot occupancy

    void Start()
    {
        // Initialize the occupancy flags
        hotdogSlotOccupied = new bool[hotdogSlots.Length];
        bunSlotOccupied = new bool[bunSlots.Length];
    }

    void Update()
    {
        if (itemsOnGrill.Count > 0)
        {
            List<GameObject> cookedItems = new List<GameObject>();

            // Update each item's cooking time
            foreach (GameObject item in itemsOnGrill)
            {
                if (item != null)
                {
                    // Increase the cooking time for each item
                    itemCookTimers[item] += Time.deltaTime;

                    // Check the type of item and its cooking time
                    if (item.CompareTag("Hotdog") && itemCookTimers[item] >= 5f)  // 5s for hotdog
                    {
                        CookItem(item, cookedHotdogSprite, "CookedHotdog");
                        cookedItems.Add(item);  // Mark this item as cooked
                    }
                    else if (item.CompareTag("Bun") && itemCookTimers[item] >= 3f)  // 3s for bun
                    {
                        CookItem(item, cookedBunSprite, "CookedBun");
                        cookedItems.Add(item);  // Mark this item as cooked
                    }
                }
            }

            // Remove cooked items from the grill
            foreach (GameObject cookedItem in cookedItems)
            {
                itemsOnGrill.Remove(cookedItem);
                itemCookTimers.Remove(cookedItem);
                ReleaseSlot(cookedItem);  // Release the occupied slot
            }
        }
    }

    // Method to place an item on the grill
    public void PlaceItem(GameObject item)
    {
        if (item.CompareTag("Hotdog"))
        {
            // Check for available hotdog slots
            for (int i = 0; i < hotdogSlots.Length; i++)
            {
                if (!hotdogSlotOccupied[i])
                {
                    hotdogSlotOccupied[i] = true;  // Mark the slot as occupied
                    itemsOnGrill.Add(item);
                    itemCookTimers[item] = 0f;  // Initialize cooking time for this item
                    item.transform.SetParent(hotdogSlots[i]);  // Attach item to the specific hotdog slot
                    item.transform.localPosition = Vector3.zero;  // Adjust position on grill
                    item.layer = nonInteractableLayer;
                    Debug.Log(item.name + " is placed on the grill in hotdog slot: " + i);
                    return;
                }
            }
            Debug.Log("No free hotdog slots available!");
        }
        else if (item.CompareTag("Bun"))
        {
            // Check for available bun slots
            for (int i = 0; i < bunSlots.Length; i++)
            {
                if (!bunSlotOccupied[i])
                {
                    bunSlotOccupied[i] = true;  // Mark the slot as occupied
                    itemsOnGrill.Add(item);
                    itemCookTimers[item] = 0f;  // Initialize cooking time for this item
                    item.transform.SetParent(bunSlots[i]);  // Attach item to the specific bun slot
                    item.transform.localPosition = Vector3.zero;  // Adjust position on grill
                    item.layer = nonInteractableLayer;
                    Debug.Log(item.name + " is placed on the grill in bun slot: " + i);
                    return;
                }
            }
            Debug.Log("No free bun slots available!");
        }
        else
        {
            Debug.Log("Item cannot be placed on the grill!");
        }
    }

    // Method to cook an item and change its sprite
    void CookItem(GameObject item, Sprite cookedSprite, string newTag)
    {
        // Change the item to its "cooked" version
        if (item != null && item.GetComponent<SpriteRenderer>() != null)
        {
            item.GetComponent<SpriteRenderer>().sprite = cookedSprite;
            item.tag = newTag;

            item.layer = interactableLayer;

            Debug.Log(item.name + " is cooked!");
        }
    }

    // Method to release the slot when an item is taken
    private void ReleaseSlot(GameObject item)
    {
        if (item.CompareTag("CookedHotdog"))
        {
            for (int i = 0; i < hotdogSlots.Length; i++)
            {
                if (hotdogSlots[i].transform.childCount == 0) // Assuming the cooked item was placed in the slot
                {
                    hotdogSlotOccupied[i] = false; // Mark the slot as free
                    Debug.Log("Hotdog slot " + i + " is now free.");
                }
            }
        }
        else if (item.CompareTag("CookedBun"))
        {
            for (int i = 0; i < bunSlots.Length; i++)
            {
                if (bunSlots[i].transform.childCount == 0) // Assuming the cooked item was placed in the slot
                {
                    bunSlotOccupied[i] = false; // Mark the slot as free
                    Debug.Log("Bun slot " + i + " is now free.");
                }
            }
        }
    }
}
