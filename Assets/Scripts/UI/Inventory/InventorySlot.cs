using UnityEngine.UI;

public class InventorySlot
{
    public InventoryItem Item { get; set; }
    public Button UIButton { get; set; }
    public int SlotNumber { get; private set; }
    public bool IsSelected { get; set; }  // Add this line to indicate if this slot is selected

    public InventorySlot(int slotNumber)
    {
        SlotNumber = slotNumber;
        IsSelected = false;  // Initially, the slot is not selected
    }

    public bool IsEmpty => Item == null;

    public void addItem(InventoryItem item)
    {
        Item = item;
    }

    public void RemoveItem()
    {
        Item = null;
        if (UIButton != null)
        {
            UnityEngine.Object.Destroy(UIButton.gameObject);
            UIButton = null;
        }
    }
}
