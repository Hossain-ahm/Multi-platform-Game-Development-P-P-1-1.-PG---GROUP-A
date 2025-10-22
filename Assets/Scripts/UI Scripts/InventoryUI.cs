using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private GameObject inventoryUI;
    [SerializeField] private TextMeshProUGUI interactText;
    [SerializeField] PlayerInventory playerInventory;
    // Start is called before the first frame update
    void Start()
    {
        inventoryUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        var text = "";
;       foreach (var item in playerInventory.GetInventory())
        {
            text += item.ToString() + ',';
        }
        interactText.text = text;
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            inventoryUI.SetActive(!inventoryUI.activeSelf);
        }
    }
}
