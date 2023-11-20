using System;
using UnityEngine;

[Serializable]
public class WeaponInventoryItem : InventoryItem
{
    public GameObject weaponPrefab;

    public WeaponInventoryItem(string id, GameObject weaponPrefab)
        : base(id, InventoryItemType.Weapon) // Pass InventoryItemType.Weapon for weapons
    {
        this.weaponPrefab = weaponPrefab;
    }

    // Override the Use method if necessary
    // Method to activate the weapon
    public void Activate()
    {
        if (weaponPrefab != null)
        {
            weaponPrefab.SetActive(true);
            // Additional activation logic if needed
        }
    }

    // Method to deactivate the weapon
    public void Deactivate()
    {
        if (weaponPrefab != null)
        {
            weaponPrefab.SetActive(false);
            // Additional deactivation logic if needed
        }
    }
}


