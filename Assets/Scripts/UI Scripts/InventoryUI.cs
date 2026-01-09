using UnityEngine;

namespace UI_Scripts
{
    public class InventoryUI : MonoBehaviour
    {
        [SerializeField] private GameObject inventoryUI;
        [SerializeField] PlayerInventory playerInventory;
        // Start is called before the first frame update
        void Start()
        {
            inventoryUI.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                inventoryUI.SetActive(!inventoryUI.activeSelf);
            }
        }
    }
}
