using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockInteractor : MonoBehaviour,IInteractor
{
    [SerializeField] private string interactTag;
    [SerializeField] private PlayerInventory playerInventory;
    public void Interact()
    {
        playerInventory.AddToInventory("Rock");
        Destroy(gameObject);
    }

    public string GetInteractText()
    {
        return interactTag;
    }
}
