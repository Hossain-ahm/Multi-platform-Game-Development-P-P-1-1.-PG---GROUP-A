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
        if (Input.GetKeyDown(KeyCode.I))
        {
            foreach (var item in _items)
            {
                Debug.Log(item);
            }
        }
    }
}
