using Unity.VisualScripting;
using UnityEngine;
[CreateAssetMenu(fileName = "New Misc item", menuName = "Item/Misc")]
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