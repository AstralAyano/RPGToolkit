using UnityEngine;

[CreateAssetMenu(fileName = "InventoryData", menuName = "InventoryData", order = 1)]
public class InventoryData : ScriptableObject
{
    public InventorySlot[] inventorySlots;
}
