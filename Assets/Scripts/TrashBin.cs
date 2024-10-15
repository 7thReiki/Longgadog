using UnityEngine;

public class TrashBin : MonoBehaviour
{
    public void TrashObject(GameObject item)
    {
        // Add logic here to either destroy the object or recycle it
        Destroy(item);
        Debug.Log("Item has been trashed.");
    }
}
