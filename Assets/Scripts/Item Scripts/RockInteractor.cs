using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class RockInteractor : MonoBehaviour,IInteractor
{
    [SerializeField] private string interactTag;
    [SerializeField] private PlayerInventory playerInventory;
    [SerializeField] private ItemClass itemClass;
    public void Interact()
    {
        playerInventory.AddItem(itemClass,1);
        Destroy(gameObject);
    }

    public string GetInteractText()
    {
        return interactTag;
    }
}
