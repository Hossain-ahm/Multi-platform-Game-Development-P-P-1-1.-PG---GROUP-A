using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    private readonly List<string> _items = new List<string>();
    
    public void AddToInventory(string item) 
    {
        _items.Add(item);
    }

    public void RemoveFromInventory(string item)
    {
        _items.Remove(item);
    }

    public List<string> GetInventory()
    {
        return _items;
    }
}
