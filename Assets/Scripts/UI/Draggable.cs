using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private Item item;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Image image;
    private Vector3 originalPosition;
    private Transform originalParent;
    private Canvas canvas;
    private float originalAlpha = 1f;
    private ISlot originalSlot; // Track the slot this item belongs to

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        image = GetComponent<Image>();
        canvas = GetComponentInParent<Canvas>();
        originalPosition = rectTransform.position;
        originalParent = transform.parent;
        originalAlpha = image.color.a;
        originalSlot = originalParent.GetComponent<ISlot>();

        if (item != null && image != null)
            image.sprite = item.icon;
    }

    public void Initialize(Item newItem, ISlot slot = null)
    {
        item = newItem;
        originalSlot = slot;
        if (image != null && item != null)
            image.sprite = item.icon;
    }

    public Item GetItem() => item;

    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0.6f; // Fade during drag
        transform.SetParent(canvas.transform, true);
        transform.SetAsLastSibling();

        // Notify original slot that item is being removed
        if (originalSlot != null && originalSlot.Item == gameObject)
        {
            originalSlot.RemoveItem();
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1f;

        // Check if dropped on a valid slot
        ISlot newSlot = eventData.pointerEnter?.GetComponent<ISlot>();
        if (newSlot == null || newSlot.Item != null)
        {
            // Snap back to original position/slot
            rectTransform.position = originalPosition;
            transform.SetParent(originalParent, true);
            if (originalSlot != null && originalSlot.IsEmpty)
            {
                originalSlot.PlaceItem(gameObject);
            }
        }
        else
        {
            // Update original slot reference
            originalSlot = newSlot;
            originalPosition = rectTransform.position;
            originalParent = transform.parent;
        }
    }

    public void SetNewOriginalPosition(Vector3 newPosition, Transform newParent)
    {
        originalPosition = newPosition;
        originalParent = newParent;
        originalSlot = newParent.GetComponent<ISlot>();
    }
}