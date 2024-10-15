using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    private GameObject heldItem;
    public KeyCode interactKey = KeyCode.E;
    public KeyCode dropKey = KeyCode.Q;  // Key for dropping items
    public KeyCode trashKey = KeyCode.R; // Key for throwing items into the trash bin

    public int interactableLayer;     // Layer where items can be interacted with (cooked items)
    public int nonInteractableLayer;  // Layer where items cannot be interacted with (raw items)

    public float rangeRadius = 0.5f;
    public Transform dropPosition;  // The position where the item will be dropped
    public Transform holdPosition; // New transform for holding items

    private bool isNearTrash = false; // Tracks if player is near the trash bin
    private TrashBin nearbyTrashBin;  // Reference to the nearby trash bin

    void Update()
    {
        if (Input.GetKeyDown(interactKey))
        {
            if (heldItem != null)
            {
                if (heldItem.CompareTag("Hotdog") || heldItem.CompareTag("Bun"))
                {
                    PlaceItemOnGrill();
                }
                else if (heldItem.CompareTag("CookedHotdog") || heldItem.CompareTag("CookedBun") ||
                         heldItem.CompareTag("Ketchup") || heldItem.CompareTag("Mustard"))
                {
                    PlaceItemOnAssemblyStation();
                }
            }
            else
            {
                RemoveItemFromGrill();
                if (heldItem == null)
                {
                    GrabItem();
                }
            }
        }

        if (Input.GetKeyDown(dropKey) && heldItem != null)
        {
            DropItem();
        }

        if (Input.GetKey(trashKey) && heldItem != null && isNearTrash)
        {
            TrashItem();
        }

        // Update the held item position based on the player's facing direction
        if (heldItem != null)
        {
            UpdateHeldItemPosition();
        }
    }

    void UpdateHeldItemPosition()
    {
        // Set the held item position to the holdPosition
        heldItem.transform.position = holdPosition.position;
        heldItem.transform.rotation = holdPosition.rotation; // Optional: set the rotation to match the hold position
    }

    void GrabItem()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, rangeRadius);
        foreach (var hit in hits)
        {
            if ((hit.CompareTag("Hotdog") || hit.CompareTag("Bun") ||
                 hit.CompareTag("CookedHotdog") || hit.CompareTag("CookedBun") ||
                 hit.CompareTag("Ketchup") || hit.CompareTag("Mustard")) &&
                hit.gameObject.layer == interactableLayer)
            {
                if (hit.CompareTag("Hotdog") || hit.CompareTag("Bun"))
                {
                    StorageRoom storageRoom = hit.GetComponentInParent<StorageRoom>();
                    if (storageRoom != null)
                    {
                        if (hit.CompareTag("Hotdog"))
                        {
                            storageRoom.RemoveHotdog();
                        }
                        else if (hit.CompareTag("Bun"))
                        {
                            storageRoom.RemoveBun();
                        }
                    }
                }

                hit.transform.SetParent(null);

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
                if (grillStation.IsGrillOccupied())
                {
                    Debug.Log("Cannot place item on grill, it's already occupied!");
                    return;
                }

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

    void RemoveItemFromGrill()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, rangeRadius);
        foreach (var hit in hits)
        {
            GrillStation grillStation = hit.GetComponent<GrillStation>();
            if (grillStation != null && heldItem == null)
            {
                GameObject itemOnGrill = grillStation.GetCookedItem();
                if (itemOnGrill != null)
                {
                    heldItem = itemOnGrill;
                    itemOnGrill.transform.SetParent(transform);
                    itemOnGrill.transform.localPosition = Vector3.zero;

                    grillStation.ReleaseSlot(itemOnGrill);
                    break;
                }
            }
        }
    }

    void DropItem()
    {
        if (heldItem != null)
        {
            heldItem.transform.SetParent(null);
            heldItem.transform.position = dropPosition.position;
            heldItem = null;
            Debug.Log("Item dropped.");
        }
    }

    void TrashItem()
    {
        if (heldItem != null && nearbyTrashBin != null)
        {
            nearbyTrashBin.TrashObject(heldItem);
            heldItem = null;
            Debug.Log("Item trashed.");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("TrashBin"))
        {
            isNearTrash = true;
            nearbyTrashBin = collision.gameObject.GetComponent<TrashBin>();
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("TrashBin"))
        {
            isNearTrash = false;
            nearbyTrashBin = null;
        }
    }
}
