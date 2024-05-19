using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public static class SaveSystem
{
    public static void SavePosition(Vector3 position, string prefix, string identifier)
    {
        PlayerPrefs.SetFloat(prefix + identifier + "PosX", position.x);
        PlayerPrefs.SetFloat(prefix + identifier + "PosY", position.y);
        PlayerPrefs.SetFloat(prefix + identifier + "PosZ", position.z);
        PlayerPrefs.Save();
    }

    public static Vector3 LoadPosition(string prefix, string identifier)
    {
        float x = PlayerPrefs.GetFloat(prefix + identifier + "PosX", 0);
        float y = PlayerPrefs.GetFloat(prefix + identifier + "PosY", 0);
        float z = PlayerPrefs.GetFloat(prefix + identifier + "PosZ", 0);
        return new Vector3(x, y, z);
    }

    public static bool HasSavedPosition(string prefix, string identifier)
    {
        return PlayerPrefs.HasKey(prefix + identifier + "PosX") &&
               PlayerPrefs.HasKey(prefix + identifier + "PosY") &&
               PlayerPrefs.HasKey(prefix + identifier + "PosZ");
    }
    public static void SaveInventory(List<Items> items, string prefix, string identifier)
    {
        for (int i = 0; i < items.Count; i++)
        {
            PlayerPrefs.SetString(prefix + identifier + "Item_" + i, items[i].GetType().Name);
        }
        PlayerPrefs.SetInt(prefix + identifier + "ItemCount", items.Count);
        PlayerPrefs.Save();
    }

    public static List<Items> LoadInventory(string prefix, string identifier)
    {
        List<Items> items = new List<Items>();
        int itemCount = PlayerPrefs.GetInt(prefix + identifier + "ItemCount", 0);
        for (int i = 0; i < itemCount; i++)
        {
            string itemName = PlayerPrefs.GetString(prefix + identifier + "Item_" + i, "");
            Items item = GameObject.FindObjectOfType(System.Type.GetType(itemName)) as Items;
            if (item != null)
            {
                items.Add(item);
            }
        }
        return items;
    }
}