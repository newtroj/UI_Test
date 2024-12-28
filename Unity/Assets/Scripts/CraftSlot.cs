using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class CraftSlot : MonoBehaviour
{
    [SerializeField] private Image _image;
    [field: SerializeField] public int SlotID { get; private set; }
    
    public bool IsActive { get; private set; }

    public void SetSlotActive(bool active)
    {
        IsActive = active;
        if (active)
        {
            _image.color = Color.white;
            transform.DOScale(Vector3.one * 0.95f, 0.5f).SetEase(Ease.OutBounce);
        }
        else
        {
            _image.color = Color.clear;
            transform.localScale = Vector3.one;
        }
    }
}