using System.Collections;
using UnityEngine;

public class Kapı : MonoBehaviour, Items
{
    public ItemScriptableObject itemScriptableObject;
    public GameObject requirement;
    private Items item;
    IEnumerator enumerator;
    Vector3 target;
    public Sprite GetImage()
    {
        throw new System.NotImplementedException();
    }

    public void Interacted()
    {
        Items item = requirement.GetComponent<Items>();
        if (InventoryManager.Inventory.Contains(item))
        {
            gameObject.SetActive(false);
        }
    }
    public bool isCollectable()
    {
        return false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Interacted();
        }
    }
}
