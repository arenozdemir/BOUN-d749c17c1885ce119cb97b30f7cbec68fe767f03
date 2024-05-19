using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;
    private int activeNumber;
    public static List<Items> Inventory = new List<Items>(6);
    Inputs inputs;
    private void Awake()
    {
        Inventory.Clear();
        if (instance == null)
        {
            instance = this;
            inputs = new Inputs();
            Debug.Log("InventoryManager instance initialized.");
        }
        else if (instance != this)
        {
            Debug.LogWarning("Another instance of InventoryManager detected and destroyed.");
            Destroy(gameObject);
            return;
        }
        inputs.InventoryActions.Newaction.started += ItemNumber;
    }
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
    public Items GetActiveItem()
    {
        return Inventory.Count > activeNumber ? Inventory[activeNumber] : null;
    }
}