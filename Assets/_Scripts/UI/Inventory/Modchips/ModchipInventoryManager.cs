using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;


public class ModchipInventoryManager : MonoBehaviour
{
    public static ModchipInventoryManager instance;
    public List<ModchipSlot> modchipSlots = new List<ModchipSlot>(); // List of all inventory slots

    public ModchipSlot currentSelectedModchipSlot = null;  // Slot of the currently equipped weapon

    private ModchipInventoryItem slot0ModchipItem;
    private ModchipInventoryItem slot1ModchipItem;

    public Transform modchipEquipPanel;
    public Transform modchipInventoryPanel;
    public Transform modchipInventoryPanelHolder;
    public Transform inGameMenu;


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

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            SingletonManager.instance.RegisterSingleton(this); // Register with SingletonManager
        }
        else
        {
            Destroy(gameObject);
        }


    }


    private void Start()
    {
        // Initialize modchip slots (example: two modchip slots, both unlocked)
        ModchipSlot modchipSlot1 = new ModchipSlot(0); // First modchip slot
        ModchipSlot modchipSlot2 = new ModchipSlot(1); // Second modchip slot

        modchipSlots.Add(modchipSlot1);
        modchipSlots.Add(modchipSlot2);

        if (modchipSlotButton1 != null)
            modchipSlotButton1.onClick.AddListener(() => SelectEquipSlot(0)); //  slot 0 is for modchipSlotButton1

        if (modchipSlotButton2 != null)
            modchipSlotButton2.onClick.AddListener(() => SelectEquipSlot(1)); //  slot 1 is for modchipSlotButton2


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
        modchipInventoryPanelHolder.gameObject.SetActive(true);
        modchipInventoryPanel.gameObject.SetActive(true);
    }

    private void UpdateModchipInventoryUI()
    {
        for (int i = 0; i < modchipSlotButtons.Count; i++)
        {
            if (i < modchipInventory.Count)
            {
                ModchipInventoryItem modchipItem = modchipInventory[i];
                Button slotButton = modchipSlotButtons[i];
                Image slotImage = slotButton.GetComponent<Image>();

                if (modchipItem.modchipData != null && modchipItem.modchipData.modSprite != null)
                {
                    slotImage.sprite = modchipItem.modchipData.modSprite;
                    TooltipTrigger tooltipTrigger = slotButton.GetComponent<TooltipTrigger>();
                    slotButton.onClick.RemoveAllListeners();
                    slotButton.onClick.AddListener(() => EquipModchipToSelectedSlot(modchipItem));
                    if (tooltipTrigger != null)
                    {
                        tooltipTrigger.SetModchipData(modchipItem.modchipData);
                    }
                }
                else
                {
                    slotImage.sprite = emptyModchipSlotSprite; // Use default sprite for empty slots
                }
            }
            else
            {
                // Reset to default empty sprite for slots beyond inventory count
                Button slotButton = modchipSlotButtons[i];
                Image slotImage = slotButton.GetComponent<Image>();
                slotImage.sprite = emptyModchipSlotSprite;
                slotButton.onClick.RemoveAllListeners();
            }
        }
    }

    public ModchipInventoryItem GetModchipInventoryItemForSlot(int slotIndex)
    {
        return slotIndex == 0 ? slot0ModchipItem : slot1ModchipItem;
    }

    public void SetModchipInventoryItemForSlot(int slotIndex, ModchipInventoryItem item)
    {
        if (slotIndex == 0)
            slot0ModchipItem = item;
        else if (slotIndex == 1)
            slot1ModchipItem = item;
    }

    private void EquipModchipToSelectedSlot(ModchipInventoryItem modchipItem)
    {
        if (selectedModchipSlotIndex < 0 || selectedModchipSlotIndex >= modchipSlots.Count)
        {
            return;
        }

        var slot = modchipSlots[selectedModchipSlotIndex];

        slot.AddItem(modchipItem); // Equip the modchip to the slot

        // Set the modchip item for the selected slot
        SetModchipInventoryItemForSlot(selectedModchipSlotIndex, modchipItem);

        // Activate the modchip in the correct holder
        GameObject targetHolder = selectedModchipSlotIndex == 0 ? PlayerController.instance.modchipHolder1 : PlayerController.instance.modchipHolder2;
        modchipItem.ActivateModchip(targetHolder);

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

        modchipInventoryPanelHolder.gameObject.SetActive(false);
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
        return slotIndex == 0 ? modchipSlotButton1 : modchipSlotButton2;
    }



}