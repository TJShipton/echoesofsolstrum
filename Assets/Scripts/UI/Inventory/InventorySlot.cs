using UnityEngine.UI;
using UnityEngine;

public class InventorySlot
{
    public InventoryItem Item { get; set; }
    public Button UIButton { get; set; }
    public int SlotNumber { get;  set; }

    
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

    public void Attack()
    {
        if (Item is WeaponInventoryItem weaponItem && weaponItem.weaponPrefab.GetComponent<Weapon>() is Weapon weapon)
        {
            Debug.Log("Weapon component accessed. Initiating attack.");
            weapon.PrimaryAttack();
        }
        else
        {
            Debug.LogWarning("Failed to access Weapon component or initiate attack.");
        }
    }

}
