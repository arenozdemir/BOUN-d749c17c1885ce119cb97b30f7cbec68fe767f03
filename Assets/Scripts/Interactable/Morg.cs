using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Morg : MonoBehaviour, Items
{
    public ItemScriptableObject itemScriptableObject;
    public Sprite GetImage()
    {
        throw new System.NotImplementedException();
    }

    public void Interacted()
    {
        throw new System.NotImplementedException();
    }

    public bool isCollectable()
    {
        return false ;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<Animator>().SetTrigger("CorpseTouch");
        }
    }
}
