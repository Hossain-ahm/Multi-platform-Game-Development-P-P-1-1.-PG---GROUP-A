using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NestIneractor : MonoBehaviour,IInteractor
{
    [SerializeField] private GameObject craftingUI;

    public void Interact()
    {
        craftingUI.SetActive(!craftingUI.activeSelf);
    }

    public string GetInteractText()
    {
        return "Press E to Craft";
    }
}
