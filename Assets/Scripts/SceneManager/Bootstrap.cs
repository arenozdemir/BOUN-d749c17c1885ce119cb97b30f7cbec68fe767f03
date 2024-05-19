using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    public GameObject inventoryManagerPrefab;

    private void Awake()
    {
        // Check if the InventoryManager instance already exists
        if (InventoryManager.instance == null)
        {
            // Instantiate the InventoryManager prefab
            Instantiate(inventoryManagerPrefab);
        }
    }
}
