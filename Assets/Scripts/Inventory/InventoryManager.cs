using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;
    private int activeNumber;
    public List<Items> playerInventory = new List<Items>();
    public List<Items> FordInventory = new List<Items>();
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("InventoryManager instance initialized.");
        }
        else if (instance != this)
        {
            Debug.LogWarning("Another instance of InventoryManager detected and destroyed.");
            Destroy(gameObject);
            return;
        }
    }

    //public void SetInventory(List<Items> items)
    //{
    //    inventoryItems = new List<Items>(items);
    //}

    //public void UpdateInventory(List<Items> items)
    //{
    //    inventoryItems = new List<Items>(items);
    //}

    public void ItemNumber(InputAction.CallbackContext ctx)
    {
        switch (ctx.ReadValue<float>())
        {
            case 1:
                activeNumber = 0;
                break;
            case 2:
                activeNumber = 1;
                break;
            case 3:
                activeNumber = 2;
                break;
            case 4:
                activeNumber = 3;
                break;
            case 5:
                activeNumber = 4;
                break;
            case 6:
                activeNumber = 5;
                break;
            default:
                break;
        }
    }

    public Items GetActiveItem(List<Items> items)
    {
        return items.Count > activeNumber ? items[activeNumber] : null;
    }
}