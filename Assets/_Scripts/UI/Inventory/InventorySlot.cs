using UnityEngine.UI;

public class InventorySlot
{
    // Define the SlotType enum inside the InventorySlot class
    public enum SlotType
    {
        Weapon,
        Modchip
    }

    public InventoryItem Item { get; set; }
    public Button UIButton { get; set; }
    public int SlotNumber { get; set; }
    public bool IsLocked { get; set; }
    public bool IsSelected { get; set; } // Indicates if this slot is selected

    // Add a property for the SlotType
    public SlotType Type { get; private set; }

    // Modify the constructor to include SlotType
    public InventorySlot(int slotNumber, SlotType type)
    {
        SlotNumber = slotNumber;
        Type = type;
        IsSelected = true; // Initially, the slot is not selected
    }

    public bool IsEmpty => Item == null;

    public void addItem(InventoryItem item)
    {
        Item = item;
    }
}
