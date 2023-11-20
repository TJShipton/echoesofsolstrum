using UnityEngine.UI;

public class WeaponSlot
{
    public WeaponInventoryItem WeaponItem { get; set; }
    public Button UIButton { get; set; }
    public int SlotNumber { get; set; }
    public bool IsLocked { get; set; }
    public bool IsSelected { get; set; }

    public WeaponSlot(int slotNumber)
    {
        SlotNumber = slotNumber;
        IsSelected = false;
    }

    public bool IsEmpty => WeaponItem == null;

    public void AddItem(WeaponInventoryItem item)
    {
        WeaponItem = item;
    }
}
