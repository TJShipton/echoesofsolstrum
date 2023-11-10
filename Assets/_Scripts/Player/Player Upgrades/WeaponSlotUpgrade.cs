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

            InventorySlot secondSlot = InventoryManager.instance.slots[1];
            secondSlot.IsLocked = false; // Unlock the second slot
            Debug.Log("Second slot unlocked.");

            // Call UpdateInventoryUI to refresh the UI and show the unlocked slot sprite
            InventoryManager.instance.UpdateWeaponInventoryUI();
        }
    }

}