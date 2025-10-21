using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using TMPro;

public class PlayerUIInteract : MonoBehaviour
{
    [SerializeField] private GameObject interactUI;
    [SerializeField] private PlayerInteract playerInteract;
    [SerializeField] private TextMeshProUGUI interactText;

    void Start()
    {
        interactUI.SetActive(false);
    }
    void Update()
    {
        
        if (playerInteract.GetfoodInteractor() != null)
        {
            interactUI.SetActive(true);
            interactText.text = playerInteract.GetfoodInteractor().GetInteractText();
        }
        else
        {
            interactUI.SetActive(false);
        }
    }
    
   
    
    
}
