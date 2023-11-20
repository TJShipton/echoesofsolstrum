using UnityEngine.UI;

public class ModchipSlot
{
    public ModchipInventoryItem ModchipItem { get; set; }
    public Button UIButton { get; set; }
    public int SlotNumber { get; set; }
    public bool IsSelected { get; set; }

    public bool IsEmpty { get; private set; }


    public ModchipSlot(int slotNumber)
    {
        SlotNumber = slotNumber;
        IsSelected = false;
    }



    public void AddItem(ModchipInventoryItem item)
    {
        ModchipItem = item;
    }
}
