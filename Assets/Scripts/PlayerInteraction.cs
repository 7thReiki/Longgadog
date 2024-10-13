using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    private GameObject heldItem;
    public KeyCode interactKey = KeyCode.E;

    public int interactableLayer;     // Layer where items can be interacted with (cooked items)
    public int nonInteractableLayer;  // Layer where items cannot be interacted with (raw items)

    public float rangeRadius = 0.5f;

    void Update()
    {
        if (Input.GetKeyDown(interactKey))
        {
            if (heldItem != null)
            {
                if (heldItem.CompareTag("Hotdog") || heldItem.CompareTag("Bun"))
                {
                    PlaceItemOnGrill(); // Place on grill if raw
                }
                else if (heldItem.CompareTag("CookedHotdog") || heldItem.CompareTag("CookedBun") || heldItem.CompareTag("Ketchup") || heldItem.CompareTag("Mustard"))
                {
                    // Call ReleaseSlot before placing the item on assembly station
                    PlaceItemOnAssemblyStation(); // Place on assembly station if cooked
                }
            }
            else
            {
                GrabItem(); // Grab an item if not holding one
            }
        }
    }

    void GrabItem()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, rangeRadius);
        foreach (var hit in hits)
        {
            // Check if the hit object is a hotdog, bun, or a cooked item, and if it's interactable
            if ((hit.CompareTag("Hotdog") || hit.CompareTag("Bun") ||
                 hit.CompareTag("CookedHotdog") || hit.CompareTag("CookedBun") ||
                 hit.CompareTag("Ketchup") || hit.CompareTag("Mustard")) &&
                hit.gameObject.layer == interactableLayer)
            {

                // If it's a raw item, check the StorageRoom for removal
                if (hit.CompareTag("Hotdog") || hit.CompareTag("Bun"))
                {
                    StorageRoom storageRoom = hit.GetComponentInParent<StorageRoom>();
                    if (storageRoom != null)
                    {
                        if (hit.CompareTag("Hotdog"))
                        {
                            storageRoom.RemoveHotdog(); // Remove hotdog from storage
                        }
                        else if (hit.CompareTag("Bun"))
                        {
                            storageRoom.RemoveBun(); // Remove bun from storage
                        }
                    }
                }

                hit.transform.SetParent(null); // This ensures the item is no longer considered on the station

                // Hold the item in the player’s hand
                heldItem = hit.gameObject;
                heldItem.transform.SetParent(transform); // Attach to player
                heldItem.transform.localPosition = Vector3.zero; // Reset position to player's local
                break; // Exit loop once an item is grabbed
            }
        }
    }

    void PlaceItemOnGrill()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, rangeRadius);
        foreach (var hit in hits)
        {
            GrillStation grillStation = hit.GetComponent<GrillStation>();
            if (grillStation != null)
            {
                grillStation.PlaceItem(heldItem);
                heldItem.transform.SetParent(null);
                heldItem = null;
                break;
            }
        }
    }

    void PlaceItemOnAssemblyStation()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, rangeRadius);
        foreach (var hit in hits)
        {
            AssemblyStation assemblyStation = hit.GetComponent<AssemblyStation>();
            if (assemblyStation != null)
            {
                assemblyStation.PlaceItem(heldItem);
                heldItem.transform.SetParent(null);
                heldItem = null;
                break;
            }
        }
    }

}
