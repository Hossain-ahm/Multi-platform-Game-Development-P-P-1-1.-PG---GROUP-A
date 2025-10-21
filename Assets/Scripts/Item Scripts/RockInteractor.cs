using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockInteractor : MonoBehaviour,IInteractor
{
    [SerializeField] private string interactTag;
    public void Interact()
    {
        Debug.Log("Interacting with Rock");
    }

    public string GetInteractText()
    {
        return interactTag;
    }
}
