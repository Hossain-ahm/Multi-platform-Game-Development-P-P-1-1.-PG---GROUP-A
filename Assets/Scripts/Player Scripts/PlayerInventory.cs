using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public List<ItemClass> items = new List<ItemClass>();

    public void AddItem(ItemClass itemClass)
    {
        items.Add(itemClass);
    }
    
    public void RemoveItem(ItemClass itemClass)
    {
        items.Remove(itemClass);
    }
}
