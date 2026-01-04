using Unity.VisualScripting;
using UnityEngine;

public class MiscClass : ItemClass
{
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
        return this;
    }

    public override ConsumableClass GetConsumableItem()
    {
        return null;
    }
}