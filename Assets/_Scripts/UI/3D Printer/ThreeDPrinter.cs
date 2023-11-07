using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class ThreeDPrinter : MonoBehaviour, IInteractable
{
    public EventSystem eventSystem;
    public GameObject weaponSelectPanel;
    public GameObject weaponPanel;

    public LayerMask clickableLayer;
    public WeaponManager weaponManager;
    public WeaponButtonCreator weaponButtonCreator;
    public WeaponRaritySelector weaponRaritySelector;
    public float buttonOffsetX = 0.0f;
    public TextMeshProUGUI promptText;


    private bool isPlayerNear = false;
    private Camera mainCamera;
    private bool isWeaponPanelActive = false;
    private List<GameObject> lastRandomWeapons = null;
    private List<WeaponTier> lastRandomTiers = null;
    private Button firstButton = null;
    private bool canSelectWeapon = true;

    private void Start()
    {
        mainCamera = Camera.main;

        // Initialize EventSystem if it's null
        if (eventSystem == null)
        {
            eventSystem = FindObjectOfType<EventSystem>();

        }

        // Initialize WeaponManager if it's null
        if (weaponManager == null)
        {
            weaponManager = FindObjectOfType<WeaponManager>();
        }

        //Initialise weapon button creator 

        weaponButtonCreator.Initialize();


    }

    private void Update()
    {


        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            return;
        }

        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        // Only proceed if the player is within a certain distance (e.g., 1 unit)
        if (distanceToPlayer <= 3f)
        {

            isPlayerNear = true;
            UpdatePromptText();  // Update the prompt text

        }
        else
        {
            isPlayerNear = false;
            promptText.gameObject.SetActive(false);  // Hide the prompt

        }

        // Check for mouse click
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, clickableLayer))
            {
                if (hit.collider.gameObject == this.gameObject)
                {
                    ToggleWeaponSelectPanel();
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.E) && GameManager.instance.LastInputMethod == "keyboard")
        {
            ToggleWeaponSelectPanel();
        }


        // Check for controller button press
        if (Input.GetButtonDown("Fire3"))
        {
            if (canSelectWeapon)
            {
                ToggleWeaponSelectPanel();

            }
        }
    }
    private void UpdatePromptText()

    {
        if (!isPlayerNear || isWeaponPanelActive)  // Added isWeaponPanelActive to the condition
        {
            promptText.gameObject.SetActive(false);  // Hide the prompt
            return;
        }

        // Show the prompt
        promptText.gameObject.SetActive(true);

        if (GameManager.instance.LastInputMethod == "controller")
        {
            promptText.text = "Press B";
        }
        else
        {
            promptText.text = "Press E";
        }
    }
    public void OpenMenu()
    {
        // Check if the player is near, if not, return early.
        if (!isPlayerNear)
        {
            return;
        }

        // Only proceed to open the menu if it is not already active.
        if (!isWeaponPanelActive)
        {
            weaponPanel.SetActive(true);
            ShowWeaponOptions();
            isWeaponPanelActive = true;

            // Hide the prompt text
            promptText.gameObject.SetActive(false);
        }
    }

    public void CloseMenu()
    {
        if (!isPlayerNear) return; // Early return if the player is not near

        weaponPanel.SetActive(false);
        isWeaponPanelActive = false;
        if (eventSystem != null)
        {
            eventSystem.SetSelectedGameObject(null); // Reset the selected object in the EventSystem
        }
        promptText.gameObject.SetActive(true); // Make the prompt text reappear
    }

    private void ToggleWeaponSelectPanel()
    {
        // Check if the player is near, if not, return early.
        if (!isPlayerNear) return;

        if (isWeaponPanelActive)
        {
            UIManager.Instance.CloseCurrentMenu();
        }
        else
        {
            UIManager.Instance.HandleMenuOpen(this);
        }
    }

    private void ShowWeaponOptions()
    {
        // Early return if GameManager instance is not available
        if (!IsGameManagerAvailable())
        {
            return;
        }

        // Clear previous buttons
        ClearPreviousButtons();

        // Initialize or reuse lastRandomWeapons and lastRandomTiers
        InitializeOrReuseRandomWeaponsAndTiers();



        // Create and set up buttons for weapon options
        CreateWeaponButtons();
    }

    private bool IsGameManagerAvailable()
    {
        if (GameManager.instance == null)
        {
            return false;
        }
        return true;
    }

    private void ClearPreviousButtons()
    {
        foreach (Transform child in weaponSelectPanel.transform)
        {
            Destroy(child.gameObject);
        }
    }

    private void InitializeOrReuseRandomWeaponsAndTiers()
    {
        // Initialize lastRandomTiers if it's null
        if (lastRandomTiers == null)
        {
            lastRandomTiers = new List<WeaponTier>();
        }

        // Get random 3 weapons from the list of unlocked weapons, only if lastRandomWeapons is null
        if (lastRandomWeapons == null)
        {
            lastRandomWeapons = WeaponManager.instance.GetRandomUnlockedWeapons(3);
            lastRandomTiers.Clear();

            // Loop through each randomly selected weapon to assign and store a random tier
            foreach (GameObject weapon in lastRandomWeapons)
            {
                WeaponTier randomTier = weaponRaritySelector.GetRandomTier();
                lastRandomTiers.Add(randomTier);
            }
        }
    }

    private void CreateWeaponButtons()
    {
        firstButton = null;  // Reset first button

        for (int i = 0; i < lastRandomWeapons.Count; i++)
        {
            GameObject weapon = lastRandomWeapons[i];
            WeaponTier randomTier = lastRandomTiers[i];
            Button newButton = weaponButtonCreator.CreateWeaponButton(weapon, weaponSelectPanel.transform, randomTier);

            // Set up the button's properties and events
            SetupButton(newButton, weapon.name);
        }
    }

    private void SetupButton(Button button, string weaponName)
    {
        button.onClick.AddListener(() => OnWeaponButtonClicked(weaponName));

        // Set the first button as the selected object in the EventSystem
        if (firstButton == null)
        {
            firstButton = button;
            eventSystem.SetSelectedGameObject(firstButton.gameObject);
        }

        // Assign OnSelect and OnDeselect events to update button appearance
        EventTrigger eventTrigger = button.gameObject.AddComponent<EventTrigger>();
        EventTrigger.Entry onSelectEntry = new EventTrigger.Entry();
        onSelectEntry.eventID = EventTriggerType.Select;
        onSelectEntry.callback.AddListener((eventData) => { UpdateButtonAppearance(button, true); });
        eventTrigger.triggers.Add(onSelectEntry);

        EventTrigger.Entry onDeselectEntry = new EventTrigger.Entry();
        onDeselectEntry.eventID = EventTriggerType.Deselect;
        onDeselectEntry.callback.AddListener((eventData) => { UpdateButtonAppearance(button, false); });
        eventTrigger.triggers.Add(onDeselectEntry);
    }

    private void UpdateButtonAppearance(Button button, bool isSelected)

    {
        // Change the button's appearance based on whether it's selected or not
        Color targetColor = isSelected ? Color.green : Color.white;
        button.GetComponent<Image>().color = targetColor;
    }


    private void OnWeaponButtonClicked(string weaponName)  // New method to handle button click
    {
        bool wasWeaponPicked = InventoryManager.instance.PickWeapon(weaponName, weaponManager.weaponHolder);
        if (wasWeaponPicked)
        {

            // Deactivate the weapon in the weapon holder
            foreach (Transform child in weaponManager.weaponHolder)
            {
                child.gameObject.SetActive(false);
            }

            // get the weapon prefab by name
            GameObject weaponPrefab = weaponManager.GetWeaponPrefabByName(weaponName);
            if (weaponPrefab != null)
            {
                InventoryItem newWeaponInventoryItem = new WeaponInventoryItem(weaponName, weaponPrefab);

                // Update UI and state in ThreeDPrinter
                InventoryManager.instance.UpdateInventoryUI();
                UIManager.Instance.CloseCurrentMenu();

            }
            else
            {
                Debug.LogWarning("Could not find weapon prefab for weapon name: " + weaponName);
            }
        }
    }

    public void ClearLastRandomWeapons()
    {
        if (lastRandomWeapons != null)
        {
            lastRandomWeapons.Clear();
            lastRandomWeapons = null;  // set it back to null so that new weapons are generated next time
        }
    }
}