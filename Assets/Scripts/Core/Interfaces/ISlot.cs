using UnityEngine;

public interface ISlot
{
    GameObject Item { get; }            // The item currently in the slot (Draggable GameObject)
    bool IsEmpty { get; }               // True if the slot is empty
    void PlaceItem(GameObject item);    // Place a Draggable item in the slot
    void RemoveItem();                  // Remove the item from the slot
    void OnItemUsed();                  // Handle slot-specific actions (e.g., hotbar use)
}
