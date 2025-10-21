using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class PlayerUIInteract : MonoBehaviour
{
    [SerializeField] private GameObject interactUI;

    [SerializeField] private PlayerInteract playerInteract;

    void Start()
    {
        interactUI.SetActive(false);
    }
    void Update()
    {
        
        if (playerInteract.GetfoodInteractor())
        {
            interactUI.SetActive(true);
        }
        else
        {
            interactUI.SetActive(false);
        }
    }
    
   
    
    
}
