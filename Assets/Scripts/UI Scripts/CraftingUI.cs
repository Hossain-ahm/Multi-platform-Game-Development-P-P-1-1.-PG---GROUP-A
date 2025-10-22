using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftinUI : MonoBehaviour
{
    [SerializeField] private GameObject craftingUI;
    // Start is called before the first frame update
    void Start()
    {
        craftingUI.SetActive(false);
    }

    public void showCraftingUI()
    {
        craftingUI.SetActive(true);
    }
}
