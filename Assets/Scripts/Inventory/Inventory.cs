using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    // List to store items in the inventory.
    public List<Item> items = new List<Item>();

    // Maximum number of items the player can carry.
    public int inventorySpace = 10;

    // Function to add an item to the inventory.
    public bool AddItem(Item item)
    {
        if (items.Count < inventorySpace)
        {
            items.Add(item);
            return true; // Item added successfully.
        }
        else
        {
            Debug.LogWarning("Inventory is full. Cannot add item: " + item.itemName);
            return false; // Inventory is full.
        }
    }

    // Function to remove an item from the inventory.
    public void RemoveItem(Item item)
    {
        items.Remove(item);
    }
}
