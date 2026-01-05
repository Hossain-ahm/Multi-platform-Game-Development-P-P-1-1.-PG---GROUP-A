using Unity.VisualScripting;
using UnityEngine;

public abstract class ItemClass : ScriptableObject
{
    public string itemName;
    public Sprite itemIcon;
    public bool isStackable = true;

    public abstract ItemClass GetItem();
    public abstract ToolClass GetToolItem();
    public abstract ArmourClass GetArmourItem();
    public abstract MiscClass GetMiscItem();
    public abstract ConsumableClass GetConsumableItem();
}