using UnityEngine;



[CreateAssetMenu(fileName = "Tool_SO", menuName = "ScriptableObjects/Items/Tool", order = 3)]

public class ToolScriptableObject : ItemScriptableObject
{
    public enum EquippedItemType
    {
        Seed,
        Hoe,
        WateringCan
    }

    public EquippedItemType type;

    public SeedScriptableObject seed;
}
