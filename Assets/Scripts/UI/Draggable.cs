using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
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
        originalAlpha = image.color.a;

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


    public void UpdatePosition()
    {
        ABaseSlot slot = originalSlot as ABaseSlot;
        if (slot)
            rectTransform.position = slot.GetComponent<RectTransform>().position;
    }

    public Item GetItem() => item;

    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0.6f; // Fade during drag
        transform.SetParent(GameObject.FindGameObjectWithTag("Dragging").transform);

    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.position = Input.mousePosition; 
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1f;

        // Check if dropped on a valid slot
        ISlot newSlot = eventData.pointerEnter?.GetComponent<ISlot>();
        if (newSlot == null || newSlot.Item != null)
        {
            rectTransform.position = originalPosition;
            rectTransform.SetParent(GameObject.FindGameObjectWithTag("Hotbar-Items").transform);
            if (originalSlot != null && originalSlot.IsEmpty)
            {
                originalSlot.PlaceItem(gameObject);
            }
        }
        else
        {
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

    public ISlot GetSlot()
    {
        return (originalSlot);
    }

    public void OnDrop(PointerEventData eventData)
    {
        HotbarSlot inComingObjectSlot = eventData.pointerDrag.GetComponent<Draggable>().GetSlot() as HotbarSlot;
        HotbarSlot currentObjectSlot = originalSlot as HotbarSlot; 

        // if (inComingObjectSlot && currentObjectSlot)
        // {
        //     GameObject currentItem = currentObjectSlot.Item;
        //     GameObject inComingItem = inComingObjectSlot.Item;

        //     currentObjectSlot.RemoveItem(); 
        //     inComingObjectSlot.RemoveItem();

        //     inComingObjectSlot.PlaceItem(currentItem);
        //     currentObjectSlot.PlaceItem(inComingItem);

        //     HotbarManager hotbarManager = GameObject.FindGameObjectWithTag("Hotbar-Manager").GetComponent<HotbarManager>();
        //     Hotbar hotbar = GameObject.FindGameObjectWithTag("Hotbar").GetComponent<Hotbar>();

        //     if (inComingObjectSlot && currentObjectSlot && hotbarManager && hotbar)
        //     {
        //         Item tmp = hotbar.items[inComingObjectSlot.slotIndex];
        //         hotbar.items[inComingObjectSlot.slotIndex] = hotbar.items[currentObjectSlot.slotIndex];
        //         hotbar.items[currentObjectSlot.slotIndex] = tmp;
        //     }
        // }
        GameObject.FindGameObjectWithTag("Hotbar-Manager").GetComponent<HotbarManager>().ChangeHotbarOrder(inComingObjectSlot.slotIndex, currentObjectSlot.slotIndex);
    }
}