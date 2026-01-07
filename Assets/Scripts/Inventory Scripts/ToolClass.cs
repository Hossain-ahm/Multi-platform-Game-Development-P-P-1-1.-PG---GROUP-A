using UnityEngine;

[CreateAssetMenu(fileName = "New Tool item", menuName = "Item/Tool")]
public class ToolClass : ItemClass
{
    public enum Tooltype
    {
        weapon
    }

    public Tooltype tooltype;

    public override ItemClass GetItem()
    {
        return this;
    }

    public override ToolClass GetToolItem()
    {
        return this;
        ;
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
        return null;
    }
}