using UnityEngine;

public class WeaponSlotUpgrade : IPlayerUpgrade
{
    public string UpgradeName => "Extra Weapon Slot";

    public string UpgradeDescription => "Increases weapon carry capacity by 1, duh!";
    public int UpgradeCost => 1000;

    public int AmountSpent { get; set; }

    public float Progress => (float)AmountSpent / UpgradeCost;
    // Inside WeaponSlotUpgrade.ApplyUpgrade method:
    public void ApplyUpgrade(PlayerController player)
    {
        {
            InventoryManager.instance.maxWeaponSlots++;
            InventoryManager.instance.slots.Add(new InventorySlot(1));  // Add a new slot
            Debug.Log("Extra weapon slot added. New maxWeaponSlots: " + InventoryManager.instance.maxWeaponSlots);
        }
    }

}