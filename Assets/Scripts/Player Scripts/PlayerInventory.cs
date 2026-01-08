using System;
using System.Collections;
using System.Collections.Generic;
using Inventory_Scripts;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class PlayerInventory : MonoBehaviour
{
    [SerializeField] private PlayerHunger playerHunger;
    [SerializeField] private GameObject slotholder;
    [SerializeField] private GameObject hotbar;
    [SerializeField] private ItemClass itemClassToAdd;
    [SerializeField] private ItemClass itemClassToRemove;
    [SerializeField] private GameObject movingCursor;
    [SerializeField] private GameObject weaponSlot;
    [SerializeField] private GameObject shieldSlot;
    
    
    private SlotClass[] items;
    private GameObject[] slots;
    
    private SlotClass movingSlot;
    private SlotClass checkslot;
    private SlotClass checkslot2;
    
    private SlotClass tempSlots;
    private SlotClass originalSlots;
    private SlotClass weapon;
    private SlotClass shield;
    public bool isWeaponEquiped = false;
    public bool isshieldEquiped = false;
    private bool isMovingItem;

    private void Start()
    {
        weaponSlot.transform.GetChild(1).GetComponent<Image>().enabled= false;
        weaponSlot.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "";
        shieldSlot.transform.GetChild(1).GetComponent<Image>().enabled= false;
        shieldSlot.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "";
        weapon = new SlotClass();
        shield = new SlotClass();
        slots = new GameObject[slotholder.transform.childCount];
        items = new SlotClass[slots.Length];
        checkslot = new SlotClass();
        for (int i = 0; i < items.Length; i++)
        {
            items[i] = new SlotClass();
        }
        
        
        for (int i = 0; i < slotholder.transform.childCount; i++)
        {
            slots[i] = slotholder.transform.GetChild(i).gameObject;
        }
        
        
        AddItem(itemClassToAdd,1);
        AddItem(itemClassToRemove,1);
        AddItem(itemClassToAdd,1);
        AddItem(itemClassToAdd,1);
        AddItem(itemClassToAdd,1);
        RemoveItem(itemClassToAdd);
        RefreshUI();
    }

    private void Update()
    {
        movingCursor.SetActive(isMovingItem);
        movingCursor.transform.position = Input.mousePosition;
        if (isMovingItem)
            movingCursor.GetComponent<Image>().sprite = movingSlot.GetItem().itemIcon;
        if (Input.GetMouseButtonDown(0))
        {
            if (isMovingItem)
            {
                EndItemMove();
            }
            else
            {
                BeginItemMove();
            }
            
        }

        try
        {
            if (Input.GetMouseButtonDown(1))
            {

                checkslot = new SlotClass(GetClosestSlot());
                if (checkslot.GetItem().GetConsumableItem() != null)
                {
                    playerHunger.Eat(checkslot.GetItem().GetConsumableItem().restoreHunger);
                    RemoveItem(checkslot.GetItem());
                }
                else
                {
                    if (checkslot.GetItem().GetToolItem() != null)
                    {
                        weapon = new SlotClass(checkslot);
                        isWeaponEquiped = true;
                    }
                    else
                    {
                        if (checkslot.GetItem().GetArmourItem() != null)
                        {
                            shield = new SlotClass(checkslot);
                            isshieldEquiped = true;
                        }
                    }
                }
                checkslot.Clear();
            }
        }
        catch { }

        if (isWeaponEquiped)
        {
            weaponSlot.transform.GetChild(1).GetComponent<Image>().enabled = true;
            weaponSlot.transform.GetChild(1).GetComponent<Image>().sprite = weapon.GetItem().itemIcon;
        }
        else
        {
            weaponSlot.transform.GetChild(1).GetComponent<Image>().enabled = false;
        }
        if (isshieldEquiped)
        {
            shieldSlot.transform.GetChild(1).GetComponent<Image>().enabled = true;
            shieldSlot.transform.GetChild(1).GetComponent<Image>().sprite = shield.GetItem().itemIcon;
        }
        else
        {
            shieldSlot.transform.GetChild(1).GetComponent<Image>().enabled = false;
        }
            

        // if (Input.GetKeyDown(KeyCode.R) && (isWeaponEquiped) )
        // {
        //
        //     AddItem(weapon.GetItem(), 1);
        //     weapon.Clear();
        //     isshieldEquiped = false;
        //     isWeaponEquiped = false;
        // }
    }

    private bool BeginItemMove()
    {
        this.originalSlots = GetClosestSlot();
        if (originalSlots == null || originalSlots.GetItem() == null)
            return false;
        
        this.movingSlot = new SlotClass(originalSlots);
        originalSlots.Clear();
        isMovingItem = true;
        RefreshUI();
        return true;
    }

    private bool EndItemMove()
    {
        originalSlots = GetClosestSlot();
        if (originalSlots == null)
        {
            AddItem(movingSlot.GetItem(),movingSlot.GetQuantity());
            movingSlot.Clear();
        }
        else
        {
            if (originalSlots.GetItem() != null)
            {
                if (originalSlots.GetItem() == movingSlot.GetItem())
                {
                    if (originalSlots.GetItem().isStackable)
                    {
                        originalSlots.AddQuantity(movingSlot.GetQuantity());
                        movingSlot.Clear();
                    }
                    else
                    {
                        return false;

                    }
                }
                else
                {
                    tempSlots = new SlotClass(originalSlots);
                    originalSlots.AddItem(movingSlot.GetItem(), movingSlot.GetQuantity());
                    movingSlot.AddItem(tempSlots.GetItem(), tempSlots.GetQuantity());
                    RefreshUI();
                    return true;
                }
            }
            else
            {
                originalSlots.AddItem(movingSlot.GetItem(), movingSlot.GetQuantity());
                movingSlot.Clear();
            }
        }

        isMovingItem = false;
        RefreshUI();
        return true;
    }
    private SlotClass GetClosestSlot()
    {
        for (int i = 0;i < slots.Length; i++)
        {
            if (Vector2.Distance(slots[i].transform.position,Input.mousePosition) <= 32)
                return items[i];
        }
        return null;
    }

    public void RefreshUI()
    {
        
        for (int i = 0; i < slots.Length; i++)
        {
            try
            {
                slots[i].transform.GetChild(1).GetComponent<Image>().enabled= true;
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

    public bool AddItem(ItemClass itemClass,int quantity)
    {
        SlotClass slotClass = ContainsItem(itemClass);
        if (slotClass != null && slotClass.GetItem().isStackable){
            slotClass.AddQuantity(1);
        }
        else
        {
            for (int i = 0; i < items.Length; i++)
            {
                if (items[i].GetItem() == null)
                {
                    items[i].AddItem(itemClass,quantity);
                    break;
                }
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
                int slotToRemove = 0;
                for (int i = 0; i < items.Length; i++)
                {
                    if (items[i].GetItem() == itemClass)
                    {
                        slotToRemove = i;
                        break;
                    }
                }
                items[slotToRemove].Clear();
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
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i].GetItem() == itemClass)
                return items[i];
        }
        return null;
    }
    public int GetswordDmag() { if (weapon.GetItem() == null)
        {
            return 0;
        }
        else {
            return weapon.GetItem().GetToolItem().dmg;
        } 
    }
    
    public SlotClass[] GetItems(){return items;}
}
