using System;
using System.Collections;
using System.Collections.Generic;
using Inventory_Scripts;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class PlayerInventory : MonoBehaviour
{
    [SerializeField] private GameObject slotholder;
    [SerializeField] private ItemClass itemClassToAdd;
    [SerializeField] private ItemClass itemClassToRemove;
    
    public List<SlotClass> items = new List<SlotClass>();
    private GameObject[] slots;

    public void Start()
    {
        slots = new GameObject[slotholder.transform.childCount];
        for (int i = 0; i < slotholder.transform.childCount; i++)
        {
            slots[i] = slotholder.transform.GetChild(i).gameObject;
        }

        
        AddItem(itemClassToAdd);
        RemoveItem(itemClassToRemove);
        RefreshUI();
    }

    public void RefreshUI()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            try
            {
                slots[i].transform.GetChild(1).GetComponent<Image>().sprite = items[i].GetItem().itemIcon;
                if (items[i].GetItem().isStackable)
                    slots[i].transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = items[i].GetQuantity().ToString();
                else
                    slots[i].transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "";
            }
            catch
            {
                slots[i].transform.GetChild(1).GetComponent<Image>().sprite = null;
                slots[i].transform.GetChild(1).GetComponent<Image>().enabled= false;
                slots[i].transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "";
                
                
            }
        }
    }

    public bool AddItem(ItemClass itemClass)
    {
        SlotClass slotClass = ContainsItem(itemClass);
        if (slotClass != null){
            slotClass.AddQuantity(1);
            
        }
        else
        {
            if (items.Count < slots.Length)
            {
                items.Add(new SlotClass(itemClass,1));
            }
            else
            {
                return false;
            }
            
        }
        RefreshUI();
        return true;
    }
    
    public bool RemoveItem(ItemClass itemClass)
    {
        
        SlotClass temp = ContainsItem(itemClass);
        if (temp != null){
            if (temp.GetQuantity() > 1)
                temp.AddQuantity(-1);
            else
            {
                SlotClass slotClassToremove = new SlotClass();
                foreach (SlotClass slotClass in items)
                {
                    if (slotClass.GetItem() == itemClass)
                    {
                        items.Remove(slotClass);
                        break;
                    }
                }
                items.Remove(slotClassToremove);
            }
        }
        else
        {
            return false;
        }
        RefreshUI();
        return true;
    }

    public SlotClass ContainsItem(ItemClass itemClass)
    {
        foreach (SlotClass slotClass in items)
        {
            if (slotClass.GetItem() == itemClass)
            {
                return slotClass;
            }
        }
        return null;
    }
}
