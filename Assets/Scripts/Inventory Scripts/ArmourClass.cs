using UnityEngine;

[CreateAssetMenu(fileName = "New Armour item", menuName = "Item/Armour")]
public class ArmourClass : ItemClass
{

    public ArmourClassEnum armourClass;
    public enum ArmourClassEnum
    {
        shield
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
        return this;
    }

    public override MiscClass GetMiscItem()
    {
        return null;
    }

    public override ConsumableClass GetConsumableItem()
    {
        return null;
    }
}