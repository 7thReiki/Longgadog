using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    private GameObject heldItem;
    public KeyCode interactKey = KeyCode.E;

    void Update()
    {
        if (Input.GetKeyDown(interactKey))
        {
            if (heldItem != null)
            {
                PlaceItemOnGrill();
            }
            else
            {
                GrabItem();
            }
        }
    }

    void GrabItem()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 0.5f);
        foreach (var hit in hits)
        {
            if (hit.CompareTag("Hotdog") || hit.CompareTag("Bun"))
            {
                heldItem = hit.gameObject;
                heldItem.transform.SetParent(transform);
                heldItem.transform.localPosition = Vector3.zero;
                break;
            }
        }
    }

    void PlaceObject()
    {

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 0.5f);
        foreach (var hit in hits)
        {

            if (hit.CompareTag("Station"))
            {

                heldItem.transform.SetParent(null);
                heldItem.transform.position = hit.transform.position;
                heldItem = null;
                break;
            }
        }
    }

    void PlaceItemOnGrill()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 0.5f);
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


}