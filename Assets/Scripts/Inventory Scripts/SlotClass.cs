namespace Inventory_Scripts
{
    public class SlotClass
    {
        private ItemClass item;
        private int quantity;

        public SlotClass()
        {
            item = null;
            quantity = 0;
        }

        public SlotClass(ItemClass item, int quantity)
        {
            this.item = item;
            this.quantity = quantity;
        }

        public ItemClass GetItem(){
            return item;}
        public int GetQuantity(){
            return quantity;
        }
        public void AddQuantity(int quantity) {this.quantity += quantity;}
    }
}