using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableSlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private Image _draggingImage;
    
    private RectTransform _draggingRectTransform;
    private Image _slotImageToDrag;
    private Canvas _canvas;
    
    private void Awake()
    {
        _canvas = FindObjectOfType<Canvas>();
        _slotImageToDrag = GetComponent<Image>();
        _draggingImage.color = Color.clear;
        _draggingRectTransform = _draggingImage.GetComponent<RectTransform>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if(_slotImageToDrag.color.a < 1)
            return;
        
        _draggingImage.sprite = _slotImageToDrag.sprite;
        _draggingImage.color = Color.white;
        _draggingRectTransform.position = transform.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        _draggingRectTransform.anchoredPosition += eventData.delta / _canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _draggingImage.color = Color.clear;
        
        if (!eventData.pointerEnter)
            return;
        
        CraftSlot slot = eventData.pointerEnter.transform.parent.GetComponent<CraftSlot>();
        if (!slot) 
            return;
        
        CraftController.Instance.OnCraftMaterialAdded(slot.SlotID);
    }
}