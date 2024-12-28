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
        _image.color = active ? Color.white : Color.clear;
    }
}