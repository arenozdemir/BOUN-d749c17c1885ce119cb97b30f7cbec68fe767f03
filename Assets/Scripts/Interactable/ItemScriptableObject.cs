using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Item", menuName = "ScriptableObjects/ItemScriptableObject", order = 1)]
public class ItemScriptableObject : ScriptableObject
{
    public Sprite itemSprite;
    public AudioClip audio;
    public string description;
    public Sprite GetImage()
    {
        return itemSprite;
    }
    public string GetDescription()
    {
        return description;
    }
    public AudioClip GetAudio()
    {
        return audio;
    }
}
