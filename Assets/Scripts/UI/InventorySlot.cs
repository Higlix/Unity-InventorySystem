using UnityEngine;

public class InventorySlot : BaseSlot
{
    // No additional behavior needed; inherits all from BaseSlot
    public override void OnItemUsed()
    {
        // Optional: Log or ignore (inventory slots typically don't use items directly)
        if (!IsEmpty)
        {
            Item itemData = item.GetComponent<Draggable>().GetItem();
            Debug.Log($"Inventory Slot: Cannot use {itemData?.itemName} directly.");
        }
    }
}