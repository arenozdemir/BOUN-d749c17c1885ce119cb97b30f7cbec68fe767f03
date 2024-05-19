using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    List<ImageHolder> imageHolders = new List<ImageHolder>();
    List<Image> tempImages = new List<Image>();

    private void Awake()
    {
        foreach (Transform child in transform)
        {
            ImageHolder imageHolder = child.GetComponent<ImageHolder>();
            if (imageHolder != null)
            {
                imageHolders.Add(imageHolder);
            }
        }

        for (int i = 0; i < imageHolders.Count; i++)
        {
            foreach (Transform image in imageHolders[i].transform)
            {
                Image img = image.GetComponent<Image>();
                if (img != null)
                {
                    tempImages.Add(img);
                }
            }
        }
    }
    public void UpdateCanvas(List<Items> items)
    {
        if (items.Count == 0)
        {
            for (int i = 0; i < tempImages.Count; i++)
            {
                tempImages[i].sprite = null;
                tempImages[i].color = new Color(1, 1, 1, 0);
            }
        }
        else
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i] == null)
                {
                    tempImages[i].sprite = null;
                    tempImages[i].color = new Color(1, 1, 1, 0);
                }
                tempImages[i].sprite = items[i].GetImage();
                tempImages[i].color = new Color(1, 1, 1, 1);
            }
        }
    }
}