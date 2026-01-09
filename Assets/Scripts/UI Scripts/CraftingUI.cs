using UnityEngine;

namespace UI_Scripts
{
    public class CraftinUI : MonoBehaviour
    {
        [SerializeField] private GameObject craftingUI;
        // Start is called before the first frame update
        void Start()
        {
            craftingUI.SetActive(false);
        }

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.F) && !craftingUI.activeSelf)
            {
                craftingUI.SetActive(false);
            }
        }

        public void showCraftingUI()
        {
            craftingUI.SetActive(true);
        }

    }
}
