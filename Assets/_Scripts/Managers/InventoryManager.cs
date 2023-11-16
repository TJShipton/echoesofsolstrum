using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
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

    public bool isMenuActive = false;


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


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            bool isMenuActive = inGameMenu.gameObject.activeSelf;
            inGameMenu.gameObject.SetActive(!isMenuActive);

            // Toggle game pause
            Time.timeScale = isMenuActive ? 1 : 0;

            // Notify the PlayerController about the menu state change
            if (PlayerController.instance != null)
            {
                PlayerController.instance.SetUIState(!isMenuActive);
            }
        }
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
        //Debug.Log("Adding modchip: " + modchipItem.ItemId);

        modchipInventory.Add(modchipItem);

        // Update the inventory UI to reflect the new state
        UpdateModchipInventoryUI();

        if (modchipItem == null || string.IsNullOrEmpty(modchipItem.ItemId))
        {
            //Debug.LogWarning("Attempted to add a null or empty modchip item to inventory.");
            return;
        }


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
            Button slotButton = modchipSlotButtons[i];

            if (i < modchipInventory.Count)
            {
                ModchipInventoryItem modchipItem = modchipInventory[i];
                if (modchipItem.modchipData == null || modchipItem.modchipData.modSprite == null)
                {
                    Debug.LogError("Modchip data or sprite is null for: " + modchipItem.ItemId);
                    continue;
                }

                Image slotImage = slotButton.GetComponent<Image>();
                slotImage.sprite = modchipItem.modchipData.modSprite;
                slotButton.onClick.RemoveAllListeners();
                slotButton.onClick.AddListener(() => EquipModchipToSelectedSlot(modchipItem));

                // Adding selection and deselection event triggers
                AddEventTriggersToModchipButton(slotButton, modchipItem);
            }
            else
            {
                Image slotImage = slotButton.GetComponent<Image>();
                slotImage.sprite = emptyModchipSlotSprite;
                slotButton.onClick.RemoveAllListeners();
            }
        }
    }

    private void AddEventTriggersToModchipButton(Button button, ModchipInventoryItem item)
    {
        EventTrigger trigger = button.gameObject.GetComponent<EventTrigger>() ?? button.gameObject.AddComponent<EventTrigger>();

        EventTrigger.Entry entrySelect = new EventTrigger.Entry();
        entrySelect.eventID = EventTriggerType.Select;
        entrySelect.callback.AddListener((data) => { OnModchipSelect(item); });
        trigger.triggers.Add(entrySelect);

        EventTrigger.Entry entryDeselect = new EventTrigger.Entry();
        entryDeselect.eventID = EventTriggerType.Deselect;
        entryDeselect.callback.AddListener((data) => { TooltipSystem.Instance.HideTooltip(); });
        trigger.triggers.Add(entryDeselect);
    }

    private void OnModchipSelect(ModchipInventoryItem modchipData)
    {
        // You need to implement the GetDetails method in ModchipInventoryItem.
        string modchipDetails = modchipData.GetDetails();
        TooltipSystem.Instance.ShowTooltip(modchipDetails);
    }



    private void EquipModchipToSelectedSlot(ModchipInventoryItem modchipItem)
    {
        Debug.Log("EquipModchipToSelectedSlot called with: " + modchipItem.ItemId);
        Debug.Log("Selected Modchip Slot Index: " + selectedModchipSlotIndex);

        if (selectedModchipSlotIndex < 0 || selectedModchipSlotIndex >= slots.Count)
        {
            Debug.LogError("Invalid slot index: " + selectedModchipSlotIndex);
            return;
        }

        var slot = slots[selectedModchipSlotIndex];

        Debug.Log("Slot before adding item: " + (slot.Item != null ? slot.Item.ItemId : "Empty"));

        slot.addItem(modchipItem); // Equip the modchip to the slot
        modchipItem.ActivateModchip();

        Debug.Log("Modchip added to slot: " + slot.Item.ItemId);


        Debug.Log("Modchip removed from general inventory: " + modchipItem.ItemId);

        UpdateModchipInventoryUI(); // Update the modchip inventory UI

        // Update the UI of the equip slot
        UpdateEquipSlotUI(selectedModchipSlotIndex, modchipItem);

        modchipInventoryPanel.gameObject.SetActive(false);

        selectedModchipSlotIndex = -1; // Reset the selected slot index
    }


    private void UpdateEquipSlotUI(int slotIndex, ModchipInventoryItem modchipItem)
    {

        Button slotButton = GetSlotButton(slotIndex);
        if (slotButton != null)
        {
            Image slotImage = slotButton.GetComponent<Image>();
            if (slotImage != null)
            {
                // Set the sprite to modchip's sprite 
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
                        //Debug.Log("Button: " + slotToRemove.UIButton.name);
                        slotToRemove.UIButton.name = "Empty Slot";
                        //Debug.Log("New Name: " + slotToRemove.UIButton.name);
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

            UpdateWeaponInventoryUI();
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

        AddWeapon(newWeapon);  // Add the new weapon to the inventory

        UpdateWeaponInventoryUI();

        return true;  // Return true to indicate success
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