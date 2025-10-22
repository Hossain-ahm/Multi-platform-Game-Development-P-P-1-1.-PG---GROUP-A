using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickInteractor : MonoBehaviour,IInteractor
{
    [SerializeField] private string interactTag;
    [SerializeField] private PlayerInventory playerInventory;
    public void Interact()
    {
        playerInventory.AddToInventory("Stick");
        Destroy(gameObject);
    }

    public string GetInteractText()
    {
        return interactTag;
    }
}
