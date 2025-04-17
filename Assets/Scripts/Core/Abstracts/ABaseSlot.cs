using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class BaseSlot : MonoBehaviour, ISlot, IDropHandler
{
    protected GameObject item;
    protected Image image;
    protected BreathingImage breathingScript;
    protected float originalAlpha = 1f;

    public GameObject Item => item;
    public bool IsEmpty => item == null;

    protected virtual void Awake()
    {
        image = GetComponent<Image>();
        breathingScript = GetComponent<BreathingImage>();
        originalAlpha = image.color.a;
    }

    public virtual void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            Draggable draggable = eventData.pointerDrag.GetComponent<Draggable>();
            
            if (draggable == null)
                return;

            // Check if slot is occupied
            if (!IsEmpty)
            {
                Debug.Log("Slot is already occupied!");
                return;
            }

            // Snap to slot's absolute position (per your latest positioning preference)
            RectTransform draggedRect = eventData.pointerDrag.GetComponent<RectTransform>();
            RectTransform slotRect = GetComponent<RectTransform>();
            draggedRect.position = slotRect.position;
            draggedRect.SetParent(transform, true);

            // Store item and update Draggable
            item = eventData.pointerDrag;
            draggable.SetNewOriginalPosition(slotRect.position, transform);

            // Update visuals
            UpdateSlotVisuals();

            Debug.Log($"Dropped {draggable.GetItem()?.itemName} into {gameObject.name}");
        }
    }

    public virtual void PlaceItem(GameObject newItem)
    {
        if (!IsEmpty)
            return;

        item = newItem;
        RectTransform itemRect = item.GetComponent<RectTransform>();
        RectTransform slotRect = GetComponent<RectTransform>();
        itemRect.position = slotRect.position;
        itemRect.SetParent(transform, true);

        Draggable draggable = item.GetComponent<Draggable>();
        if (draggable != null)
        {
            draggable.SetNewOriginalPosition(slotRect.position, transform);
        }

        UpdateSlotVisuals();
    }

    public virtual void RemoveItem()
    {
        if (IsEmpty) 
            return;
        item = null;
        UpdateSlotVisuals();
    }

    public virtual void OnItemUsed()
    {
        // Implemented by specific slot types
    }

    protected virtual void UpdateSlotVisuals()
    {
        if (image != null)
        {
            float newAlpha = IsEmpty ? originalAlpha : 0.3f; // Fade when occupied
            image.color = new Color(image.color.r, image.color.g, image.color.b, newAlpha);
        }

        if (breathingScript != null)
        {
            breathingScript.enabled = IsEmpty; // Breathe only when empty
        }
    }

    // Optional: Detect when item is dragged away
    protected virtual void Update()
    {
        if (item != null && item.transform.parent != transform)
        {
            RemoveItem();
        }
    }
}