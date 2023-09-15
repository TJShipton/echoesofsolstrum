using UnityEngine;

public class Item
{
    public string itemName;     // The name of the item.
    public string description;  // A brief description of the item 
    public Sprite icon;         // The icon or image representing the item we may change this if using 3D models instead
    public int itemID;          // An optional unique identifier for the item.
    public bool stackable;      // Indicates if the item can stack in the inventory.
    public int maxStack;        // The maximum number of items that can stack (if stackable).

    public WeaponData weaponData;

}
