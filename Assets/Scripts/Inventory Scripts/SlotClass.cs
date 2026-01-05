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
        public SlotClass(SlotClass slot)
        {
            item = slot.item;
            quantity = slot.quantity;
        }

        public SlotClass(ItemClass item, int quantity)
        {
            this.item = item;
            this.quantity = quantity;
        }

        public void Clear()
        {
            item = null;
            this.quantity = 0;
        }

        public ItemClass GetItem(){
            return item;}
        public int GetQuantity(){
            return quantity;
        }
        public void AddQuantity(int quantity) {this.quantity += quantity;}
        public void AddItem (ItemClass itemClass, int quantity){
            this.item = itemClass;
            this.quantity = quantity;
        }
    }
}