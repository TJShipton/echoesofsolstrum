using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;


public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;
    public List<InventorySlot> slots = new List<InventorySlot>(); // List of all inventory slots
    public int maxWeaponSlots = 2;  // Max number of equippable weapon slots
    public InventorySlot currentSelectedSlot = null;  // Slot of the currently equipped weapon
    public WeaponButtonCreator weaponButtonCreator;
    public Transform weaponInventoryPanel;  // UI panel to hold weapon buttons
    public Transform modchipEquipPanel;
    public Transform modchipInventoryPanel;
    public Transform inGameMenu;
    public Transform weaponHolder;

    public Sprite lockedSlotSprite;
    public Sprite emptySlotSprite;
    public Sprite emptyModchipSlotSprite;
    public Button modchipSlotButton1; // Reference to the first modchip slot button
    public Button modchipSlotButton2; // Reference to the second modchip slot button
    public List<ModchipInventoryItem> modchipInventory = new List<ModchipInventoryItem>();
    private int selectedModchipSlotIndex = -1; // Initialize to -1 to indicate no slot is selected

    public List<Button> modchipSlotButtons;

    public InputActionAsset inputActions;
    private InputAction toggleMenuAction;
    public GameObject firstSelectedButton;
    public TooltipSystem tooltipSystem;

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
            Debug.LogWarning("Attempted to initialize a second InventoryManager instance.");
        }

        // Find the weaponHolder by tag
        weaponHolder = GameObject.FindGameObjectWithTag("WeaponHolder").transform;

        if (weaponHolder == null)
        {
            Debug.LogError("WeaponHolder not found. Please make sure it is tagged correctly in the scene.");
        }


    }

    private void Start()
    {
        // Initialize weapon slots: one unlocked and one locked
        InventorySlot unlockedWeaponSlot = new InventorySlot(0, InventorySlot.SlotType.Weapon);
        InventorySlot lockedWeaponSlot = new InventorySlot(1, InventorySlot.SlotType.Weapon) { IsLocked = true };
        slots.Add(unlockedWeaponSlot);
        slots.Add(lockedWeaponSlot);

        // Initialize modchip slots (example: two modchip slots, both unlocked)
        InventorySlot modchipSlot1 = new InventorySlot(2, InventorySlot.SlotType.Modchip);
        InventorySlot modchipSlot2 = new InventorySlot(3, InventorySlot.SlotType.Modchip);
        slots.Add(modchipSlot1);
        slots.Add(modchipSlot2);

        if (modchipSlotButton1 != null)
            modchipSlotButton1.onClick.AddListener(() => SelectEquipSlot(2)); //  slot 2 is for modchipSlotButton1

        if (modchipSlotButton2 != null)
            modchipSlotButton2.onClick.AddListener(() => SelectEquipSlot(3)); //  slot 3 is for modchipSlotButton2

        // Initialize weapon button creator
        weaponButtonCreator.Initialize();

        // Select the first unlocked weapon slot as the current selected slot
        currentSelectedSlot = unlockedWeaponSlot;
        currentSelectedSlot.IsSelected = true;

        UpdateWeaponInventoryUI(); // Update the UI to reflect the initial slot state

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
    void Update()
    {




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


    public void SelectSlot(int slotIndex)
    {
        // Check if the slotIndex is within the range of the slots list
        if (slotIndex >= 0 && slotIndex < slots.Count)
        {
            if (currentSelectedSlot != null)
            {
                currentSelectedSlot.IsSelected = false;  // Deselect the previously selected slot
            }

            currentSelectedSlot = slots[slotIndex];
            currentSelectedSlot.IsSelected = true;  // Select the new slot

        }

        UpdateWeaponAndModchipState();
    }

    private void UpdateWeaponAndModchipState()
    {
        foreach (var slot in InventoryManager.instance.slots)
        {
            if (slot.Item is WeaponInventoryItem weaponItem)
            {
                if (slot == InventoryManager.instance.currentSelectedSlot)
                {
                    weaponItem.Activate();
                }
                else
                {
                    weaponItem.Deactivate();
                }
            }
            else if (slot.Item is ModchipInventoryItem modchipItem)
            {
                if (slot == InventoryManager.instance.currentSelectedSlot)
                {
                    modchipItem.ActivateModchip();
                }
                else
                {
                    modchipItem.Deactivate();
                }
            }
        }
    }


    public InventoryItem GetCurrentItem()
    {
        return currentSelectedSlot?.Item;
    }

    public void AddWeapon(Weapon weapon)
    {
        // Create a new WeaponInventoryItem for the picked weapon
        WeaponInventoryItem weaponItem = new WeaponInventoryItem(weapon.weaponName, weapon.gameObject);

        // First, try to find an empty and unlocked weapon slot
        InventorySlot availableSlot = slots.FirstOrDefault(s => s.Type == InventorySlot.SlotType.Weapon && s.IsEmpty && !s.IsLocked);

        // If there is no empty weapon slot, allow replacement in the first unlocked weapon slot
        // This considers the case where the player might pick up a weapon with full inventory
        if (availableSlot == null)
        {
            availableSlot = slots.FirstOrDefault(s => s.Type == InventorySlot.SlotType.Weapon && !s.IsLocked);
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
        availableSlot.addItem(weaponItem);

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
        SelectSlot(availableSlot.SlotNumber); // Use SlotNumber instead of slotIndex

        // If a weapon item was added, update the weapons in WeaponManager
        WeaponManager.instance.UpdateWeapons();
    }


    public void AddModchip(ModchipInventoryItem modchipItem)
    {
        modchipInventory.Add(modchipItem);

        // Update the inventory UI to reflect the new state
        UpdateModchipInventoryUI();

    }

    public void SelectEquipSlot(int slotIndex)
    {
        // Set the selectedModchipSlotIndex to the slot that was clicked
        selectedModchipSlotIndex = slotIndex;
        OpenModchipInventory();
    }



    public void OpenModchipInventory()
    {
        modchipInventoryPanel.gameObject.SetActive(true);
    }

    private void UpdateModchipInventoryUI()
    {
        for (int i = 0; i < modchipSlotButtons.Count; i++)
        {
            if (i < modchipInventory.Count)
            {
                ModchipInventoryItem modchipItem = modchipInventory[i];
                if (modchipItem.modchipData == null || modchipItem.modchipData.modSprite == null)
                {
                    continue;
                }

                Button slotButton = modchipSlotButtons[i];
                Image slotImage = slotButton.GetComponent<Image>();
                slotImage.sprite = modchipItem.modchipData.modSprite;
                TooltipTrigger tooltipTrigger = slotButton.GetComponent<TooltipTrigger>();
                slotButton.onClick.RemoveAllListeners();
                slotButton.onClick.AddListener(() => EquipModchipToSelectedSlot(modchipItem));

                // Inside the UpdateModchipInventoryUI method of InventoryManager
                if (tooltipTrigger != null && modchipItem != null)
                {
                    //pass the entire ModchipData object
                    tooltipTrigger.SetModchipData(modchipItem.modchipData);
                }


            }
            else
            {
                // For empty slots, reset to default empty sprite
                Button slotButton = modchipSlotButtons[i];
                Image slotImage = slotButton.GetComponent<Image>();
                slotImage.sprite = emptyModchipSlotSprite; // default sprite for empty slots
                slotButton.onClick.RemoveAllListeners();
            }
        }
    }

    private void EquipModchipToSelectedSlot(ModchipInventoryItem modchipItem)
    {


        if (selectedModchipSlotIndex < 0 || selectedModchipSlotIndex >= slots.Count)
        {

            return;
        }

        var slot = slots[selectedModchipSlotIndex];



        slot.addItem(modchipItem); // Equip the modchip to the slot
        modchipItem.ActivateModchip();

        // Find the TMP text component and disable it
        Button slotButton = GetSlotButton(selectedModchipSlotIndex);
        if (slotButton != null)
        {
            TextMeshProUGUI textComponent = slotButton.GetComponentInChildren<TextMeshProUGUI>();
            if (textComponent != null)
            {
                textComponent.enabled = false; // Disable the text component
            }
        }

        UpdateModchipInventoryUI(); // Update the modchip inventory UI

        // Update the UI of the equip slot
        UpdateEquipSlotUI(selectedModchipSlotIndex, modchipItem);

        modchipInventoryPanel.gameObject.SetActive(false);

        selectedModchipSlotIndex = -1; // Reset the selected slot index

        tooltipSystem.HideTooltip();

        // Set a default button to be selected after equipping a modchip
        if (firstSelectedButton != null)
        {
            EventSystem.current.SetSelectedGameObject(firstSelectedButton);
        }
        else
        {
            Debug.LogWarning("No default button set for post-modchip equip.");
        }

    }


    private void UpdateEquipSlotUI(int slotIndex, ModchipInventoryItem modchipItem)
    {
        // Find the UI element corresponding to the slotIndex
        Button slotButton = GetSlotButton(slotIndex);
        if (slotButton != null)
        {
            Image slotImage = slotButton.GetComponent<Image>();
            if (slotImage != null)
            {
                // Set the sprite to modchip's sprite or some other indicator
                slotImage.sprite = modchipItem.modchipData.modSprite;
            }
        }
    }

    private Button GetSlotButton(int slotIndex)
    {
        return slotIndex == 2 ? modchipSlotButton1 : modchipSlotButton2;
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

                    slotToRemove.Item = null;  // InventorySlot.Item has a setter method
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

        AddWeapon(newWeapon);  // Add the new weapon to the inventory

        UpdateWeaponInventoryUI();

        return true;  // Return true to indicate success
    }


    public Weapon GetCurrentWeapon()
    {
        if (currentSelectedSlot != null && currentSelectedSlot.Item is WeaponInventoryItem weaponItem)
        {
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



    public void UpdateWeaponInventoryUI()
    {
        foreach (InventorySlot slot in slots)
        {
            if (slot.Type != InventorySlot.SlotType.Weapon)
            {
                continue; // Skip non-weapon slots
            }

            if (slot.UIButton != null)
            {
                // If the slot is supposed to be empty or locked
                if (slot.IsEmpty || slot.IsLocked)
                {
                    UpdateWeaponSlotVisual(slot, slot.IsLocked ? WeaponButtonCreator.SlotState.Locked : WeaponButtonCreator.SlotState.Empty);
                }
                else if (slot.Item is WeaponInventoryItem weaponItem) // Safe casting to avoid InvalidCastException
                {
                    // Ensure we update the button if it's not the correct one
                    if (slot.UIButton.name != weaponItem.ItemId)
                    {
                        // Properly destroy or deactivate the existing button
                        Destroy(slot.UIButton.gameObject);

                        // Create the weapon button with all dynamic elements
                        slot.UIButton = weaponButtonCreator.CreateWeaponButton(
                            weaponItem.weaponPrefab, weaponInventoryPanel,
                            weaponItem.weaponPrefab.GetComponent<Weapon>().weaponData.weaponTier);
                    }
                }
            }
            else
            {
                // If there is no UIButton, create the appropriate default button
                UpdateWeaponSlotVisual(slot, slot.IsLocked ? WeaponButtonCreator.SlotState.Locked : WeaponButtonCreator.SlotState.Empty);
            }
        }

        LogInventoryState();
    }


    private void UpdateWeaponSlotVisual(InventorySlot slot, WeaponButtonCreator.SlotState state)
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

}