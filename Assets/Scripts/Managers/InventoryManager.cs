using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;
    public List<InventorySlot> slots = new List<InventorySlot>(); // List of all inventory slots
    public int maxWeaponSlots = 0;  // Max number of equippable weapon slots
    public InventorySlot currentSelectedSlot = null;  // Slot of the currently equipped weapon
    public WeaponButtonCreator weaponButtonCreator;
    public Transform weaponInventoryPanel;  // UI panel to hold weapon buttons



    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }




    }

    private void Start()
    {

        InventoryManager.instance.slots.Add(new InventorySlot(1));  // Add one slot for the initial weapon
        weaponButtonCreator.Initialize();  // Ensure WeaponButtonCreator is initialized

    }
    void Update()
    {
        // Check if the Escape key is pressed
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Toggle the visibility of the weaponInventoryPanel
            weaponInventoryPanel.gameObject.SetActive(!weaponInventoryPanel.gameObject.activeSelf);
        }
    }

    // Method to count current weapons
    public int CountCurrentWeapons()
    {
        int count = 0;
        foreach (InventorySlot slot in slots)
        {
            if (!slot.IsEmpty && slot.Item is WeaponInventoryItem)
            {
                count++;
            }
        }
        return count;
    }

    // Method to find an empty slot
    private InventorySlot FindEmptySlot()
    {
        foreach (InventorySlot slot in slots)
        {
            if (slot.IsEmpty)
            {
                return slot;
            }
        }
        return null;  // return null if no empty slot is found
    }

    // Method to update UI
    private void UpdateButton(Button newButton, InventoryItem item, InventorySlot slot)
    {
        weaponButtonCreator.UpdateButtonName(newButton, item.ItemId);
        slot.UIButton = newButton;
    }

    public void SelectSlot(int slotIndex)
    {
        if (currentSelectedSlot != null)
        {
            currentSelectedSlot.IsSelected = false;  // Deselect the previously selected slot
        }
        currentSelectedSlot = slots[slotIndex];
        currentSelectedSlot.IsSelected = true;  // Select the new slot
    }

    public void AddItem(InventoryItem item)
    {
        Debug.Log("Trying to add item: " + item.ItemId); // Add Debug here
                                                         // Check if item is a weapon item
        if (item is WeaponInventoryItem weaponItem)
        {
            Debug.Log("Item is a WeaponInventoryItem."); // Add Debug here
            InventorySlot availableSlot = FindEmptySlot();  // Look for an empty slot

            // If no empty slot is found and there's room to expand the inventory, create a new slot
            if (availableSlot == null && CountCurrentWeapons() < maxWeaponSlots)
            {
                availableSlot = new InventorySlot(slots.Count);  // Create a new slot
                slots.Add(availableSlot);  // Add the new slot to the slots list
                Debug.Log("Created new InventorySlot."); // Add Debug here
            }

            // If an available slot is found or created, add the item to it
            if (availableSlot != null)
            {
                availableSlot.addItem(item);  // Add the item to the available slot
                Debug.Log($"Item {item.ItemId} added to Slot {availableSlot.SlotNumber}"); // Debug already exists, good!

                // Create a button for this weapon
                Button newButton = weaponButtonCreator.CreateWeaponButton(
                    weaponItem.weaponPrefab, weaponInventoryPanel, weaponItem.weaponPrefab.GetComponent<Weapon>().weaponData.weaponTier);

                // Update button
                UpdateButton(newButton, item, availableSlot);

                // Update the inventory UI
                UpdateInventoryUI();
            }
            else
            {
                Debug.LogWarning($"No available slot for item {item.ItemId}, and inventory is full."); // Debug already exists, good!
            }
        }
    }




    // InventoryManager.cs
    public void RemoveItem(InventoryItem itemToRemove)
    {
        if (itemToRemove is WeaponInventoryItem weaponItem)
        {

            for (int i = 0; i < slots.Count; i++)
            {
                InventorySlot slotToRemove = slots[i];
                if (slotToRemove != null && slotToRemove.Item == itemToRemove)  // Added null check here
                {
                    Debug.Log("Checking slot " + i + ": " + slotToRemove.Item.ItemId);
                    if (slotToRemove.UIButton != null)  // Added null check here
                    {
                        Debug.Log("Button: " + slotToRemove.UIButton.name);
                        slotToRemove.UIButton.name = "Empty Slot";
                        Debug.Log("New Name: " + slotToRemove.UIButton.name);
                    }
                    else
                    {
                        Debug.LogWarning("slotToRemove.UIButton is null");
                    }

                    slotToRemove.Item = null;  // Assume InventorySlot.Item has a setter method
                    UpdateInventoryUI();  // Assume this method updates the UI to reflect changes
                    break;  // Exit the loop once the item is removed
                }
            }
        }
    }

    // Method to swap weapons
    public void SwapWeapon(InventoryItem newWeaponInventoryItem)
    {
        Debug.Log("Attempting to swap weapon.");

        // Check if the item being swapped is actually a weapon.
        if (newWeaponInventoryItem is WeaponInventoryItem)
        {
            Debug.Log("Item is a WeaponInventoryItem.");

            // Initialize selected slot index to -1 (indicating no slot is selected)
            int selectedSlotIndex = 0;

            // Find the first available slot index to swap the weapon
            for (int i = 0; i < slots.Count; i++)
            {
                if (slots[i].IsSelected)
                {
                    selectedSlotIndex = i;
                    Debug.Log($"Selected slot index found: {i}");
                    break;
                }
            }

            Debug.Log($"Current selected slot index: {selectedSlotIndex}");

            

            // Check if we found a selected slot
            if (selectedSlotIndex == 0)
            {
                Debug.LogWarning("No slot is selected for weapon swap.");
                return;
            }

            // Here, call SelectSlot with the selectedSlotIndex.
            SelectSlot(selectedSlotIndex);

           

            // Perform the swap
            try
            {
                InventoryItem oldWeapon = slots[selectedSlotIndex].Item;
                slots[selectedSlotIndex].Item = newWeaponInventoryItem;
                Debug.Log($"Successfully swapped {oldWeapon.ItemId} with {newWeaponInventoryItem.ItemId}.");
            }
            catch (ArgumentOutOfRangeException e)
            {
                Debug.LogError($"ArgumentOutOfRangeException: {e.Message}");
            }
        }
        else
        {
            Debug.LogWarning("Item is not a WeaponInventoryItem. Swap aborted.");
        }
    }





    public void UpdateInventoryUI()
    {
        // Example logic to update UI
        foreach (InventorySlot slot in slots)
        {
            // Assume each slot has a reference to a UI element
            if (slot.UIButton != null)
            {
                slot.UIButton.gameObject.SetActive(slot.Item != null);
            }
        }

        LogInventoryState();
    }

    public void LogInventoryState()
    {
        string inventoryState = "Inventory State:\n";
        foreach (var slot in slots)
        {
            if (slot.Item is WeaponInventoryItem weaponItem)
            {
                inventoryState += weaponItem.weaponPrefab.name + "\n";
            }
            else
            {
                inventoryState += "Empty Slot\n";
            }
        }
        Debug.Log(inventoryState);
    }

}