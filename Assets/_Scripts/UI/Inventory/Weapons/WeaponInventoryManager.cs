using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
public class WeaponInventoryManager : MonoBehaviour
{
    public static WeaponInventoryManager instance;
    public List<WeaponSlot> weaponSlots = new List<WeaponSlot>();

    public int maxWeaponSlots = 2;  // Max number of equippable weapon slots
    public WeaponSlot currentSelectedWeaponSlot = null; // Currently selected weapon slot  // Slot of the currently equipped weapon
    public WeaponButtonCreator weaponButtonCreator;
    public Transform weaponInventoryPanel;  // UI panel to hold weapon buttons
    public Transform inGameMenu;
    public Transform weaponHolder;
    public Sprite lockedSlotSprite;
    public Sprite emptySlotSprite;
    public InputActionAsset inputActions;
    private InputAction toggleMenuAction;
    public GameObject firstSelectedButton;




    void Awake()
    {
        if (instance == null)
        {
            instance = this;

        }
        else
        {
            Destroy(gameObject);
        }


        // Find the weaponHolder by tag
        weaponHolder = GameObject.FindGameObjectWithTag("WeaponHolder").transform;

        if (weaponHolder == null)
        {
            Debug.LogError("WeaponHolder not found. Please make sure it is tagged correctly in the scene.");
        }


    }

    // Start is called before the first frame update
    void Start()
    {
        InitializeWeaponSlots();
        UpdateWeaponInventoryUI();
    }
    void OnEnable()
    {
        // Activate the UI action map
        var uiActionMap = inputActions.FindActionMap("UI");
        if (uiActionMap != null)
        {
            uiActionMap.Enable();
        }
        else
        {
            Debug.LogError("UI Action Map not found in Input Actions.");
        }
    }

    void OnDisable()
    {
        // Deactivate the UI action map
        var uiActionMap = inputActions.FindActionMap("UI");
        if (uiActionMap != null)
        {
            uiActionMap.Disable();
        }
    }


    public void OnToggleGameMenu(InputAction.CallbackContext context)
    {
        if (context.performed)  // Check if the action was performed (not just started or canceled)
        {
            ToggleGameMenu();
        }
    }
    private void ToggleGameMenu()
    {


        bool isMenuActive = inGameMenu.gameObject.activeSelf;
        inGameMenu.gameObject.SetActive(!isMenuActive);

        if (!isMenuActive)
        {


            // Select the first button when opening the menu
            EventSystem.current.SetSelectedGameObject(firstSelectedButton);
        }

        Time.timeScale = isMenuActive ? 1 : 0;

    }

    private void InitializeWeaponSlots()
    {
        // Initialize weapon slots: one unlocked and one locked
        WeaponSlot unlockedWeaponSlot = new WeaponSlot(0); // Unlocked slot
        WeaponSlot lockedWeaponSlot = new WeaponSlot(1) { IsLocked = true }; // Locked slot

        weaponSlots.Add(unlockedWeaponSlot);
        weaponSlots.Add(lockedWeaponSlot);

        // Optionally, select the first unlocked weapon slot as the current selected slot
        currentSelectedWeaponSlot = unlockedWeaponSlot;
        currentSelectedWeaponSlot.IsSelected = true;
    }

    public void AddWeapon(Weapon weapon)
    {
        // Create a new WeaponInventoryItem for the picked weapon
        WeaponInventoryItem weaponItem = new WeaponInventoryItem(weapon.weaponName, weapon.gameObject);

        // First, try to find an empty and unlocked weapon slot
        WeaponSlot availableSlot = weaponSlots.FirstOrDefault(s => s.IsEmpty && !s.IsLocked);

        // If there is no empty weapon slot, allow replacement in the first unlocked weapon slot
        if (availableSlot == null)
        {
            availableSlot = weaponSlots.FirstOrDefault(s => !s.IsLocked);
        }

        // If there's still no available weapon slot, it means all weapon slots are locked/full
        if (availableSlot == null)
        {
            Debug.LogWarning("No available slot for weapon " + weaponItem.ItemId + " and inventory is full or locked.");
            return; // Exit the method as no slot is available for the new weapon item
        }

        // If the slot already has an item, it will be replaced
        if (!availableSlot.IsEmpty && availableSlot.UIButton != null)
        {
            Destroy(availableSlot.UIButton.gameObject); // This will remove the button from the UI
        }

        // Add the weapon item to the available slot
        availableSlot.AddItem(weaponItem);

        // If there is an existing button, destroy it before creating a new one
        if (availableSlot.UIButton != null)
        {
            Destroy(availableSlot.UIButton.gameObject);
            availableSlot.UIButton = null; // Clear the reference
        }

        // Create a button for this weapon
        availableSlot.UIButton = weaponButtonCreator.CreateWeaponButton(
            weaponItem.weaponPrefab, weaponInventoryPanel, weaponItem.weaponPrefab.GetComponent<Weapon>().weaponData.weaponTier);

        // Update the inventory UI to reflect the new state
        UpdateWeaponInventoryUI();

        // Select the slot
        SelectWeaponSlot(availableSlot.SlotNumber); // Use SlotNumber instead of slotIndex

        // If a weapon item was added, update the weapons in WeaponManager
        WeaponManager.instance.UpdateWeapons();
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

        AddWeapon(newWeapon);  // Add the new weapon to the inventory

        UpdateWeaponInventoryUI();

        return true;  // Return true to indicate success
    }

    public Weapon GetCurrentWeapon()
    {
        if (currentSelectedWeaponSlot != null && currentSelectedWeaponSlot.WeaponItem != null)
        {
            return currentSelectedWeaponSlot.WeaponItem.weaponPrefab.GetComponent<Weapon>();
        }

        return null;
    }



    public List<Weapon> GetAllWeapons()
    {
        List<Weapon> weapons = new List<Weapon>();
        foreach (var slot in weaponSlots)
        {
            if (slot.WeaponItem != null)
            {
                weapons.Add(slot.WeaponItem.weaponPrefab.GetComponent<Weapon>());
            }
        }
        return weapons;
    }

    // Method to select a weapon slot by its slot number
    public void SelectWeaponSlot(int slotNumber)
    {
        // Ensure the slot number is valid
        if (slotNumber >= 0 && slotNumber < weaponSlots.Count)
        {
            // Deselect the previously selected slot if there is one
            if (currentSelectedWeaponSlot != null)
            {
                currentSelectedWeaponSlot.IsSelected = false;
            }

            // Select the new slot and update the current selected slot
            currentSelectedWeaponSlot = weaponSlots[slotNumber];
            currentSelectedWeaponSlot.IsSelected = true;

            // Any additional logic needed when a weapon slot is selected
            // For example, update UI to reflect the selected slot
        }
        else
        {
            Debug.LogError("Invalid weapon slot number: " + slotNumber);
        }
    }

    public void UpdateWeaponInventoryUI()
    {
        foreach (WeaponSlot slot in weaponSlots)
        {
            if (slot.UIButton == null)
            {
                // If there is no UIButton, create the appropriate default button
                UpdateWeaponSlotVisual(slot, slot.IsLocked ? WeaponButtonCreator.SlotState.Locked : WeaponButtonCreator.SlotState.Empty);
            }
            else if (slot.IsEmpty || slot.IsLocked)
            {
                // Update the button's sprite for empty or locked slots
                UpdateWeaponSlotVisual(slot, slot.IsLocked ? WeaponButtonCreator.SlotState.Locked : WeaponButtonCreator.SlotState.Empty);
            }
            else if (slot.WeaponItem != null)
            {
                // If there's a weapon in the slot, ensure the button is updated
                if (slot.UIButton.name != slot.WeaponItem.ItemId)
                {
                    // Properly destroy or deactivate the existing button
                    Destroy(slot.UIButton.gameObject);

                    // Create the weapon button with all dynamic elements
                    slot.UIButton = weaponButtonCreator.CreateWeaponButton(
                        slot.WeaponItem.weaponPrefab, weaponInventoryPanel,
                        slot.WeaponItem.weaponPrefab.GetComponent<Weapon>().weaponData.weaponTier);
                }
            }
        }

        LogInventoryState();
    }

    private void UpdateWeaponSlotVisual(WeaponSlot slot, WeaponButtonCreator.SlotState state)
    {
        if (slot.UIButton == null)
        {
            // Create a default button with the state (empty or locked)
            slot.UIButton = weaponButtonCreator.CreateDefaultButton(weaponInventoryPanel, state);
        }

        // Update the button's sprite and alpha based on the state
        Image buttonImage = slot.UIButton.GetComponent<Image>();
        if (state == WeaponButtonCreator.SlotState.Empty)
        {
            buttonImage.sprite = emptySlotSprite;

        }
        else if (state == WeaponButtonCreator.SlotState.Locked)
        {
            buttonImage.sprite = lockedSlotSprite;

        }

        // Activate the button GameObject
        slot.UIButton.gameObject.SetActive(true);
    }

    public void RemoveItem(InventoryItem itemToRemove)
    {
        if (itemToRemove is WeaponInventoryItem weaponItem)
        {
            for (int i = 0; i < weaponSlots.Count; i++)
            {
                WeaponSlot slotToRemove = weaponSlots[i];
                if (slotToRemove != null && slotToRemove.WeaponItem == itemToRemove)
                {

                    if (slotToRemove.UIButton != null)
                    {

                        slotToRemove.UIButton.name = "Empty Slot";

                    }
                    else
                    {
                        Debug.LogWarning("slotToRemove.UIButton is null");
                    }

                    if (weaponItem.weaponPrefab != null)
                    {
                        Destroy(weaponItem.weaponPrefab);  // Destroy the weapon game object
                    }

                    slotToRemove.WeaponItem = null;  // InventorySlot.Item has a setter method
                    break;  // Exit the loop once the item is removed
                }
            }

            UpdateWeaponInventoryUI();
        }

        // If a weapon item was removed, update the weapons in WeaponManager
        if (itemToRemove is WeaponInventoryItem)
        {
            WeaponManager.instance.UpdateWeapons();
        }
    }

    public void LogInventoryState()
    {
        string inventoryState = "Inventory State:\n";
        foreach (var slot in weaponSlots)
        {
            if (slot.WeaponItem != null)
            {
                inventoryState += slot.WeaponItem.weaponPrefab.name + "\n";
            }
            else
            {
                //inventoryState += "Empty Slot\n";
            }
        }

        //Debug.Log(inventoryState);
    }


}
