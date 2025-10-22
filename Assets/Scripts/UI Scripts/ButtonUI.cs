using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonUI : MonoBehaviour
{
    [SerializeField] private PlayerInventory playerInventory;

    public void CraftSword()
    {
        if (playerInventory.GetInventory().Contains("Stick") && playerInventory.GetInventory().Contains("Rock"))
        {
            playerInventory.RemoveFromInventory("Stick");
            playerInventory.RemoveFromInventory("Rock");
            
            playerInventory.AddToInventory("Stone Sword");
            

        }
    }
}
