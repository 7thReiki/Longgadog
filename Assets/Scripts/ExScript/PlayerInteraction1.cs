using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction1 : MonoBehaviour
{
    public GameObject heldItem; // Reference to the item the player is holding
    public Transform holdPoint; // Where the item is held (e.g., in front of the player)
    public float interactionRadius = 1.2f; // The radius for the overlap circle, slightly increased
    public LayerMask interactableLayer; // LayerMask for items and grill station
    private GrillStation1 currentGrillStation; // Reference to the grill station

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (heldItem == null) // If not holding any item
            {
                // Check for any nearby interactable objects
                CheckForInteractable();
            }
            else // If holding an item, try to place it
            {
                if (currentGrillStation != null)
                {
                    // Try to place the item in the grill
                    PlaceItemInGrill();
                }
                else
                {
                    DropItem(); // Drop item if not near grill
                }
            }
        }
    }

    private void CheckForInteractable()
    {
        Collider2D[] nearbyObjects = Physics2D.OverlapCircleAll(transform.position, interactionRadius, interactableLayer);

        foreach (Collider2D col in nearbyObjects)
        {
            Debug.Log("Nearby Object: " + col.gameObject.name); // See which objects are detected

            if (col.CompareTag("RawBun") || col.CompareTag("RawHotdog"))
            {
                PickUpItem(col.gameObject);
                return;
            }

            if (col.CompareTag("GrillStation"))
            {
                currentGrillStation = col.GetComponent<GrillStation1>();
                GrabCookedItem();
                return;
            }
        }
    }

    private void PickUpItem(GameObject item)
    {
        heldItem = item;
        heldItem.transform.position = holdPoint.position;
        heldItem.transform.parent = holdPoint;
        heldItem.GetComponent<Collider2D>().enabled = false; // Disable collider while holding the item
    }

    private void DropItem()
    {
        if (heldItem != null)
        {
            heldItem.transform.parent = null; // Detach from holdPoint
            heldItem.GetComponent<Collider2D>().enabled = true; // Re-enable collider
            heldItem = null; // Clear the held item
        }
    }

    private void PlaceItemInGrill()
    {
        if (heldItem != null)
        {
            if (heldItem.CompareTag("RawBun"))
            {
                bool placed = currentGrillStation.PlaceItem(heldItem, "bun");
                if (placed)
                {
                    Destroy(heldItem); // Destroy the held item after placing it
                    heldItem = null; // Clear the held item reference
                }
            }
            else if (heldItem.CompareTag("RawHotdog"))
            {
                bool placed = currentGrillStation.PlaceItem(heldItem, "hotdog");
                if (placed)
                {
                    Destroy(heldItem); // Destroy the held item after placing it
                    heldItem = null; // Clear the held item reference
                }
            }
        }
    }

    private void GrabCookedItem()
    {
        GameObject cookedItem = currentGrillStation.GrabCookedItem("bun"); // Check for cooked buns
        if (cookedItem == null)
        {
            cookedItem = currentGrillStation.GrabCookedItem("hotdog"); // Check for cooked hotdogs
        }

        if (cookedItem != null)
        {
            heldItem = cookedItem;
            heldItem.transform.position = holdPoint.position;
            heldItem.transform.parent = holdPoint;
            heldItem.GetComponent<Collider2D>().enabled = false; // Disable collider
        }
    }

    // Optional: Draw the interaction radius in the editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionRadius); // Draw the interaction radius
    }
}
