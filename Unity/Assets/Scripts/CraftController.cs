using System.Collections.Generic;
using UnityEngine;

public class CraftController : MonoBehaviour
{
    [SerializeField] private CraftUIController _uiController;
    [SerializeField] private List<CraftSlot> _slots;
    
    public static CraftController Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void OnCraftMaterialAdded(int slotIndex)
    {
        _slots[slotIndex].SetSlotActive(true);
        
        if(CanCraft())
            _uiController.EnableCraftButton();
    }

    public void OnCraftMaterialRemoved(int slotIndex)
    {
        _slots[slotIndex].SetSlotActive(false);
        _uiController.DisableCraftButton();
    }

    public bool CanCraft()
    {
        for (var index = 0; index < _slots.Count; index++)
        {
            CraftSlot slot = _slots[index];
            if (slot.IsActive) 
                continue;

            return false;
        }

        return true;
    }

    public void OnCraftFinished()
    {
        ResetSlots();
    }
    
    public void ResetSlots()
    {
        for (var index = 0; index < _slots.Count; index++)
        {
            CraftSlot slot = _slots[index];
            slot.SetSlotActive(false);
        }
    }
}