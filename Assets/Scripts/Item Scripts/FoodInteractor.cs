using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodInteractor : MonoBehaviour
{
    [SerializeField] private string interactTag;
    [SerializeField] private PlayerHunger playerHungerScript;
    [SerializeField] private float eatAmount;
    public void Interact()
    {
        playerHungerScript.Eat(eatAmount);
        Destroy(gameObject);
    }

    public string GetInteractText()
    {
        return interactTag;
    }
}
