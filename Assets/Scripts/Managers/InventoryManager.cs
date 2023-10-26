using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;
    public List<InventorySlot> slots = new List<InventorySlot>(); // List of all inventory slots
    public int maxWeaponSlots = 0;  // Max number of equippable weapon slots
    public InventorySlot currentSelectedSlot = null;  // Slot of the currently equipped weapon
    public WeaponButtonCreator weaponButtonCreator;
    public Transform weaponInventoryPanel;  // UI panel to hold weapon buttons
    public Transform weaponHolder;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            //Debug.Log("InventoryManager instance initialized.");
        }
        else
        {
            Destroy(gameObject);
            Debug.LogWarning("Attempted to initialize a second InventoryManager instance.");
        }
    }

    private void Start()
    {

        InventoryManager.instance.slots.Add(new InventorySlot(0));  // Add one slot for the initial weapon
        weaponButtonCreator.Initialize();  // Ensure WeaponButtonCreator is initialized
        if (slots.Count > 0)
        {
            currentSelectedSlot = slots[0];
            currentSelectedSlot.IsSelected = true;
            //Debug.Log("Slot 0 initialized and selected.");
        }

        //Debug.Log("Item in slot 0 after initialization: " + (slots[0].Item != null ? slots[0].Item.ToString() : "null"));
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
        //Debug.Log("SelectSlot called with index: " + slotIndex);  // Log the index

        // Check if the slotIndex is within the range of the slots list
        if (slotIndex >= 0 && slotIndex < slots.Count)
        {
            if (currentSelectedSlot != null)
            {
                currentSelectedSlot.IsSelected = false;  // Deselect the previously selected slot
            }

            currentSelectedSlot = slots[slotIndex];
            currentSelectedSlot.IsSelected = true;  // Select the new slot
            //Debug.Log("Current selected slot index: " + slots.IndexOf(currentSelectedSlot));  // Log the selected slot index
        }
        else
        {
            Debug.LogWarning("SelectSlot: Index out of range: " + slotIndex);
        }
    }



    public void AddItem(InventoryItem item)
    {
        // Check if item is a weapon item
        if (item is WeaponInventoryItem weaponItem)
        {
            InventorySlot availableSlot = FindEmptySlot();  // Look for an empty slot

            // If no empty slot is found and there's only one slot (your scenario)
            if (availableSlot == null && slots.Count == 1)
            {
                availableSlot = slots[0];  // Use the existing slot

                // Remove the current weapon from the slot before adding the new one
                if (availableSlot.Item != null)
                {
                    RemoveItem(availableSlot.Item);
                }
            }
            // If no empty slot is found and there's room to expand the inventory, create a new slot
            else if (availableSlot == null && CountCurrentWeapons() < maxWeaponSlots)
            {
                availableSlot = new InventorySlot(slots.Count);  // Create a new slot
                slots.Add(availableSlot);  // Add the new slot to the slots list
            }

            if (availableSlot != null)
            {
                availableSlot.addItem(item);  // Add the item to the available slot

                // Create a button for this weapon
                Button newButton = weaponButtonCreator.CreateWeaponButton(
                    weaponItem.weaponPrefab, weaponInventoryPanel, weaponItem.weaponPrefab.GetComponent<Weapon>().weaponData.weaponTier);

                // Update button
                UpdateButton(newButton, item, availableSlot);

                // Update the inventory UI
                UpdateInventoryUI();

                // Select the slot
                SelectSlot(availableSlot.SlotNumber);  // Use SlotNumber instead of slotIndex
            }
            else
            {
                Debug.LogWarning($"No available slot for item {item.ItemId}, and inventory is full.");
            }
        }

        // If a weapon item was added, update the weapons in WeaponManager
        if (item is WeaponInventoryItem)
        {
            WeaponManager.instance.UpdateWeapons();
        }
    }



    public void RemoveItem(InventoryItem itemToRemove)
    {
        if (itemToRemove is WeaponInventoryItem weaponItem)
        {
            for (int i = 0; i < slots.Count; i++)
            {
                InventorySlot slotToRemove = slots[i];
                if (slotToRemove != null && slotToRemove.Item == itemToRemove)
                {

                    if (slotToRemove.UIButton != null)
                    {
                        Debug.Log("Button: " + slotToRemove.UIButton.name);
                        slotToRemove.UIButton.name = "Empty Slot";
                        Debug.Log("New Name: " + slotToRemove.UIButton.name);
                    }
                    else
                    {
                        Debug.LogWarning("slotToRemove.UIButton is null");
                    }

                    if (weaponItem.weaponPrefab != null)
                    {
                        Destroy(weaponItem.weaponPrefab);  // Destroy the weapon game object
                    }

                    slotToRemove.Item = null;  // Assume InventorySlot.Item has a setter method
                    break;  // Exit the loop once the item is removed
                }
            }

            UpdateInventoryUI();
        }

        // If a weapon item was removed, update the weapons in WeaponManager
        if (itemToRemove is WeaponInventoryItem)
        {
            WeaponManager.instance.UpdateWeapons();
        }
    }


    // Method to swap weapons

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

    }

    public bool PickWeapon(string weaponName, Transform weaponHolder)
    {
        GameObject weaponPrefab = WeaponManager.instance.GetWeaponPrefabByName(weaponName);
        if (weaponPrefab == null)
        {
            return false;  // Return false if weaponPrefab is null
        }

        // Deactivate the current weapon in the weaponHolder, if any
        foreach (Transform child in weaponHolder)
        {
            child.gameObject.SetActive(false);
        }

        Weapon newWeapon = WeaponManager.instance.InstantiateNewWeapon(weaponPrefab, weaponHolder);
        if (newWeapon == null)
        {
            return false;  // Return false if newWeapon instantiation failed
        }

        AddWeaponToInventory(newWeapon);  // Add the new weapon to the inventory

        UpdateInventoryUI();

        return true;  // Return true to indicate success
    }




    private void AddWeaponToInventory(Weapon newWeapon)
    {
        // Create a new InventoryItem for the picked weapon
        InventoryItem newWeaponItem = new WeaponInventoryItem(newWeapon.weaponName, newWeapon.gameObject);

        // Add the new weapon to the inventory
        AddItem(newWeaponItem);
    }





    public Weapon GetCurrentWeapon()
    {
        if (currentSelectedSlot != null && currentSelectedSlot.Item is WeaponInventoryItem weaponItem)
        {
            //Debug.Log("Current Selected Weapon: " + weaponItem.weaponPrefab.name);  // Log the current weapon name
            return weaponItem.weaponPrefab.GetComponent<Weapon>();
        }

        return null;
    }

    public List<Weapon> GetAllWeapons()
    {
        List<Weapon> weapons = new List<Weapon>();
        foreach (var slot in slots)
        {
            if (slot.Item is WeaponInventoryItem weaponItem)
            {
                weapons.Add(weaponItem.weaponPrefab.GetComponent<Weapon>());
            }
        }
        return weapons;
    }



    public void UpdateInventoryUI()
    {
        // Example logic to update UI
        foreach (InventorySlot slot in slots)
        {
            // Assume each slot has a reference to a UI element
            if (slot.UIButton != null)
            {
                // Update the UI button to reflect the current item in the slot
                if (slot.Item != null)
                {
                    // Assuming you have a method to update the button with the new item information
                    UpdateButton(slot.UIButton, slot.Item, slot);

                    slot.UIButton.gameObject.SetActive(true);  // Ensure the button is active
                }
                else
                {
                    slot.UIButton.gameObject.SetActive(false);  // Hide the button if the slot is empty
                }
            }
        }

        LogInventoryState();
    }
}