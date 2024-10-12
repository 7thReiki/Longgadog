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
            }
        }
    }

    // Method to place an item on the grill
    public void PlaceItem(GameObject item)
    {
        if (itemsOnGrill.Count < maxItems && item != null)
        {
            itemsOnGrill.Add(item);
            itemCookTimers[item] = 0f;  // Initialize cooking time for this item
            item.transform.SetParent(transform);  // Attach item to the grill station
            item.transform.localPosition = new Vector3(0, 0, 0);  // Optional: adjust position on grill

            item.layer = nonInteractableLayer;
            Debug.Log(item.name + " is placed on the grill and set to layer: " + LayerMask.LayerToName(nonInteractableLayer));
        }
        else
        {
            Debug.Log("Grill is full! Max items: " + maxItems);
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

        }
        Debug.Log(item.name + " is cooked!");
    }
}