using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Kasa : MonoBehaviour, Items
{
    public ItemScriptableObject itemScriptableObject;
    [SerializeField] GameObject RequirementItem;
    private Sprite sprite;
    [SerializeField] private TextMeshProUGUI text;
    Items item;
    private void Awake()
    {
        sprite = itemScriptableObject.GetImage();
        item = RequirementItem.GetComponent<Items>();
    }
    public void Interacted()
    {
        if (FordController.items.Contains(item))
        {
            StartCoroutine(ShowDescription());
        }
    }
    public Sprite GetImage()
    {
        return sprite;
    }
    private IEnumerator ShowDescription()
    {
        text.text = itemScriptableObject.GetDescription();
        yield return new WaitForSeconds(4);
        text.text = "";
    }
}
