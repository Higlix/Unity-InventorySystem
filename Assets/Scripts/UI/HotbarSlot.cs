using UnityEngine;
using UnityEngine.EventSystems;

public class HotbarSlot : ABaseSlot
{
    [SerializeField] public int slotIndex = 0;

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

    public override void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            Draggable draggable = eventData.pointerDrag.GetComponent<Draggable>();
            
            if (draggable == null)
                return;

            // Check if slot is occupied
            if (!IsEmpty)
            {
                Debug.Log("SlotFull");
                return ;
            }
            // draggable.GetSlot().RemoveItem();

            // Snap to slot's absolute position (per your latest positioning preference)
            // RectTransform draggedRect = eventData.pointerDrag.GetComponent<RectTransform>();
            // RectTransform slotRect = GetComponent<RectTransform>();
            // draggedRect.position = slotRect.position;

            // // Store item and update Draggable
            // item = eventData.pointerDrag;



            if (draggable.GetSlot() is HotbarSlot)
            {
                HotbarSlot originalHBSlot = draggable.GetSlot() as HotbarSlot;

                HotbarSlot newHBSlot = transform.GetComponent<ISlot>() as HotbarSlot;
                HotbarManager hotbarManager = GameObject.FindGameObjectWithTag("Hotbar-Manager").GetComponent<HotbarManager>();
                Hotbar hotbar = GameObject.FindGameObjectWithTag("Hotbar").GetComponent<Hotbar>();

                if (originalHBSlot && newHBSlot && hotbarManager && hotbar)
                {
                    Debug.Log("Move " + originalHBSlot.slotIndex + " to " + newHBSlot.slotIndex);
                    hotbarManager.ChangeHotbarOrder(originalHBSlot.slotIndex, newHBSlot.slotIndex);
                }
            }
            //GameObject.FindGameObjectWithTag("Hotbar-Manager").GetComponent<HotbarManager>().UpdateHotbar();
            // draggable.SetNewOriginalPosition(slotRect.position, transform);

            // Update visuals
            UpdateSlotVisuals();

            Debug.Log($"Dropped {draggable.GetItem()?.itemName} into {gameObject.name}");
        }
    }

}