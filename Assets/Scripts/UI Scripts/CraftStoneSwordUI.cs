using UnityEngine;

namespace UI_Scripts
{
    public class CraftbuttonUI : MonoBehaviour
    {
    [SerializeField] private ItemClass itemClass;
    [SerializeField] private ItemClass item1;
    [SerializeField] private ItemClass item2;
    [SerializeField] private PlayerInventory playerInventory;
    private bool item1check = false;
    private bool item2check = false;
        public void OnClick()
        {
            for (int i = 0; i < playerInventory.GetItems().Length; i++)
            {
                if (playerInventory.GetItems()[i].GetItem() == item1)
                {
                    item1check = true;
                }

                if (playerInventory.GetItems()[i].GetItem() == item2)
                {
                    item2check = true;
                }
            }

            if (item1check && item2check)
            {
                playerInventory.AddItem(itemClass,1);
                playerInventory.RemoveItem(item1);
                playerInventory.RemoveItem(item2);
            }
            item1check = false;
            item2check = false;
        }

        
    }
}

