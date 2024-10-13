using UnityEngine;

public class StorageRoom : MonoBehaviour
{
    public GameObject bunPrefab;
    public GameObject hotdogPrefab;

    public Transform hotdogSpawnPoint;
    public Transform bunSpawnPoint;

    private int maxHotdogs = 5; // Maximum number of hotdogs in storage
    private int maxBuns = 5;    // Maximum number of buns in storage
    private int currentHotdogs = 0; // Current hotdog count
    private int currentBuns = 0;     // Current bun count

    // Start is called before the first frame update
    void Start()
    {
        // Initially spawn max items
        SpawnInitialItems();
    }

    // Method to spawn initial items
    private void SpawnInitialItems()
    {
        for (int i = 0; i < maxHotdogs; i++)
        {
            SpawnHotdog();
        }
        for (int i = 0; i < maxBuns; i++)
        {
            SpawnBuns();
        }
    }

    public void RemoveHotdog()
    {
        if (currentHotdogs > 0)
        {
            currentHotdogs--; // Decrease count
            GameObject hotdogInstance = GameObject.FindGameObjectWithTag("Hotdog"); // Assuming you have tagged hotdog instances
            if (hotdogInstance != null)
            {
                Destroy(hotdogInstance); // Destroy the current hotdog instance
            } // Assuming you want to destroy the current hotdog
            SpawnHotdog(); // Spawn a new hotdog immediately
            Debug.Log("Hotdog removed from storage.");
        }
    }

    public void RemoveBun()
    {
        if (currentBuns > 0)
        {
            currentBuns--; // Decrease count
            GameObject bunInstance = GameObject.FindGameObjectWithTag("Bun"); // Assuming you have tagged hotdog instances
            if (bunInstance != null)
            {
                Destroy(bunInstance); // Destroy the current hotdog instance
            } // Assuming you want to destroy the current bun
            SpawnBuns(); // Spawn a new bun immediately
            Debug.Log("Bun removed from storage.");
        }
    }

    private void SpawnHotdog()
    {
        Instantiate(hotdogPrefab, hotdogSpawnPoint.position, Quaternion.identity);
        currentHotdogs++;
    }

    private void SpawnBuns()
    {
        Instantiate(bunPrefab, bunSpawnPoint.position, Quaternion.identity);
        currentBuns++;
    }
}
