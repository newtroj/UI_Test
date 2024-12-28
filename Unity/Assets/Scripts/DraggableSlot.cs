using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableSlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private Image _draggingImage;
    [SerializeField] private Sprite _coiqCraftSprite;
    
    private RectTransform _draggingRectTransform;
    private Image _slotImage;
    private Canvas _canvas;
    
    private void Awake()
    {
        _canvas = FindObjectOfType<Canvas>();
        _slotImage = GetComponent<Image>();
        _draggingImage.color = Color.clear;
        _draggingRectTransform = _draggingImage.GetComponent<RectTransform>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if(_slotImage.color.a < 1)
            return;
        
        _draggingImage.sprite = _slotImage.sprite;
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
        
        if (eventData.pointerEnter == null || !eventData.pointerEnter.CompareTag("CraftSlot"))
            return;
        
        Image craftSlotImage = eventData.pointerEnter.GetComponent<Image>();
        if (!craftSlotImage) 
            return;
        
        craftSlotImage.sprite = _coiqCraftSprite;
        craftSlotImage.color = Color.white;
    }
}
