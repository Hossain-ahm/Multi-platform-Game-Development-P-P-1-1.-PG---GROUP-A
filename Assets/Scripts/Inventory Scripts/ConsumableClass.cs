using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Consumable item", menuName = "Item/Consumable")]
    public class ConsumableClass : ItemClass
    {
        public float restoreHunger;
        public ConsumableType consumableType;
        public enum ConsumableType
        {
            food,
            potion
        }
        public override ItemClass GetItem()
        {
            return this;
        }

        public override ToolClass GetToolItem()
        {
            return null;
        }

        public override ArmourClass GetArmourItem()
        {
            return null;
        }

        public override MiscClass GetMiscItem()
        {
            return null;
        }

        public override ConsumableClass GetConsumableItem()
        {
            return this;
        }
    }
