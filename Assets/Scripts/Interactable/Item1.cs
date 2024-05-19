using System.Collections;
using TMPro;
using UnityEngine;

public class Item1 : MonoBehaviour, Items
{
    public ItemScriptableObject itemScriptableObject;
    private Sprite sprite;
    [SerializeField] TextMeshProUGUI descriptionText;
    private void Awake()
    {
        sprite = itemScriptableObject.GetImage();
    }
    public void Interacted()
    {
        gameObject.SetActive(false);
    }
    public Sprite GetImage()
    {
        return sprite;
    }
    private IEnumerator ShowDescription()
    {
        descriptionText.text = itemScriptableObject.GetDescription();
        yield return new WaitForSeconds(4);
        descriptionText.text = "";
    }
}
