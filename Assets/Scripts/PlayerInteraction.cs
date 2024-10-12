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
            // Only allow grabbing items that are on the 'interactableLayer'
            if ((hit.CompareTag("Hotdog") || hit.CompareTag("Bun") || hit.CompareTag("CookedHotdog") || hit.CompareTag("CookedBun") || hit.CompareTag("Ketchup") || hit.CompareTag("Mustard")) && hit.gameObject.layer == interactableLayer)
            {
                heldItem = hit.gameObject;
                heldItem.transform.SetParent(transform);
                heldItem.transform.localPosition = Vector3.zero;
                break;
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
            if(assemblyStation != null)
            {
                assemblyStation.PlaceItem(heldItem);
                heldItem.transform.SetParent(null);
                heldItem = null;
                break;
            }
        }
    }


}