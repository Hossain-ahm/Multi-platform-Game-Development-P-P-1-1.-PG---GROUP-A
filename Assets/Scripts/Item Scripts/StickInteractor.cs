using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickInteractor : MonoBehaviour,IInteractor
{
    [SerializeField] private string interactTag;
    public void Interact()
    {
        Debug.Log("Interacting with Stick");
    }

    public string GetInteractText()
    {
        return interactTag;
    }
}
