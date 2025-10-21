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

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log(_items[0]);
        }
    }
}
