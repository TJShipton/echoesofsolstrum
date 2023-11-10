using System;
using UnityEngine;

// Define an enumeration for different inventory item types
public enum InventoryItemType
{
    Weapon,
    Modchip,
    // Add more item types as needed
}

[Serializable]
public class InventoryItem
{
    // Properties
    public string ItemId { get; set; }
    public Sprite icon;
    public InventoryItemType ItemType { get; private set; } // Added to hold the type of the inventory item

    // Constructor
    public InventoryItem(string id, InventoryItemType itemType)
    {
        ItemId = id;
        ItemType = itemType; // Initialize the ItemType with the constructor
    }

    // Virtual method to use the item

}
