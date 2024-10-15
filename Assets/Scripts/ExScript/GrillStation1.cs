using System.Collections;
using UnityEngine;

public class GrillStation1 : MonoBehaviour
{
    [System.Serializable]
    public class GrillSlot
    {
        public Transform slotPosition; // The position where the item will be placed
        public bool isOccupied; // Whether the slot is occupied or not
        public GameObject currentItem; // The raw or cooked item in the slot
        public float cookTime; // Time it takes to cook the item
        public bool isCooking; // Whether the item is currently cooking
        public GameObject cookedItemPrefab; // The cooked version of the item
    }

    public GrillSlot[] bunSlots; // Two slots for buns
    public GrillSlot[] hotdogSlots; // Two slots for hotdogs

    // Place raw item in the grill
    public bool PlaceItem(GameObject rawItem, string itemType)
    {
        if (itemType == "bun")
        {
            return PlaceInSlot(bunSlots, rawItem);
        }
        else if (itemType == "hotdog")
        {
            return PlaceInSlot(hotdogSlots, rawItem);
        }

        return false;
    }

    // Grab the cooked item
    public GameObject GrabCookedItem(string itemType)
    {
        if (itemType == "bun")
        {
            return GrabFromSlot(bunSlots);
        }
        else if (itemType == "hotdog")
        {
            return GrabFromSlot(hotdogSlots);
        }

        return null;
    }

    // Place raw item in the first available slot
    private bool PlaceInSlot(GrillSlot[] slots, GameObject rawItem)
    {
        foreach (GrillSlot slot in slots)
        {
            if (!slot.isOccupied) // If slot is not occupied
            {
                slot.currentItem = Instantiate(rawItem, slot.slotPosition.position, Quaternion.identity);
                slot.isOccupied = true;
                StartCoroutine(CookItem(slot)); // Start cooking process
                return true;
            }
        }
        return false; // No available slots
    }

    // Grab the cooked item from the slot
    private GameObject GrabFromSlot(GrillSlot[] slots)
    {
        foreach (GrillSlot slot in slots)
        {
            if (slot.isOccupied && !slot.isCooking) // If slot is occupied and item is cooked
            {
                GameObject cookedItem = slot.currentItem;
                slot.currentItem = null;
                slot.isOccupied = false;
                return cookedItem; // Return the cooked item
            }
        }
        return null;
    }

    // Cooking coroutine
    private IEnumerator CookItem(GrillSlot slot)
    {
        slot.isCooking = true;
        yield return new WaitForSeconds(slot.cookTime); // Simulate cooking time
        if (slot.currentItem != null)
        {
            Destroy(slot.currentItem); // Destroy raw item
            slot.currentItem = Instantiate(slot.cookedItemPrefab, slot.slotPosition.position, Quaternion.identity); // Spawn cooked item
        }
        slot.isCooking = false;
    }
}
