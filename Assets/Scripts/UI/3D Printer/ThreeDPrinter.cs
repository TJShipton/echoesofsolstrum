using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class ThreeDPrinter : MonoBehaviour
{
    public EventSystem eventSystem;
    public GameObject weaponSelectPanel;
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

        weaponButtonCreator.Initialize();  // Ensure WeaponButtonCreator is initialized


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
                canSelectWeapon = false;  // Disable weapon selection
                Invoke("EnableWeaponSelection", 0.5f);  // Enable it after 0.5 seconds
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
            promptText.text = "Press X";
        }
        else
        {
            promptText.text = "Press E";
        }
    }
    private void ToggleWeaponSelectPanel()
    {
        // Check if the player is near, if not, return early.
        if (!isPlayerNear)
        {
            return;
        }

        if (isWeaponPanelActive)
        {
            weaponSelectPanel.SetActive(false);
            isWeaponPanelActive = false;

            // Reset the selected object in the EventSystem
            if (eventSystem != null)
            {
                eventSystem.SetSelectedGameObject(null);
            }

            // Make the prompt text reappear
            promptText.gameObject.SetActive(true);
        }
        else
        {
            weaponSelectPanel.SetActive(true);
            ShowWeaponOptions();
            isWeaponPanelActive = true;

            // Hide the prompt text
            promptText.gameObject.SetActive(false);
        }
    }

    private void ShowWeaponOptions()
    {
        // Early return if GameManager instance is not available
        if (GameManager.instance == null)
        {
            return;
        }

        // Clear previous buttons
        foreach (Transform child in weaponSelectPanel.transform)
        {
            Destroy(child.gameObject);
        }

        // Initialize lastRandomTiers if it's null
        if (lastRandomTiers == null)
        {
            lastRandomTiers = new List<WeaponTier>();
        }

        // Get random 3 weapons from the list of unlocked weapons, only if lastRandomWeapons is null
        if (lastRandomWeapons == null)
        {
            lastRandomWeapons = WeaponManager.instance.GetRandomUnlockedWeapons(3);

            lastRandomTiers.Clear();  // Clear the lastRandomTiers list

            // Loop through each randomly selected weapon to assign and store a random tier
            foreach (GameObject weapon in lastRandomWeapons)
            {
                WeaponTier randomTier = weaponRaritySelector.GetRandomTier();
                lastRandomTiers.Add(randomTier);  // Store the random tier
            }
        }

        firstButton = null; // To keep track of the first button

        // Loop through each randomly selected weapon
        for (int i = 0; i < lastRandomWeapons.Count; i++)
        {
            GameObject weapon = lastRandomWeapons[i];
            WeaponTier randomTier = lastRandomTiers[i];  // Use the stored tier

            // Call CreateWeaponButton method from WeaponButtonCreator script
            Button newButton = weaponButtonCreator.CreateWeaponButton(weapon, weaponSelectPanel.transform, randomTier);


            // Assign the button's click listener
            newButton.onClick.AddListener(() => OnWeaponButtonClicked(weapon.name));  // Call new method OnWeaponButtonClicked

            // Set the first button as the selected object in the EventSystem
            if (firstButton == null)
            {
                firstButton = newButton;
                eventSystem.SetSelectedGameObject(firstButton.gameObject);
            }

            // Assign OnSelect and OnDeselect events to update button appearance
            EventTrigger eventTrigger = newButton.gameObject.AddComponent<EventTrigger>();
            EventTrigger.Entry onSelectEntry = new EventTrigger.Entry();
            onSelectEntry.eventID = EventTriggerType.Select;
            onSelectEntry.callback.AddListener((eventData) => { UpdateButtonAppearance(newButton, true); });
            eventTrigger.triggers.Add(onSelectEntry);

            EventTrigger.Entry onDeselectEntry = new EventTrigger.Entry();
            onDeselectEntry.eventID = EventTriggerType.Deselect;
            onDeselectEntry.callback.AddListener((eventData) => { UpdateButtonAppearance(newButton, false); });
            eventTrigger.triggers.Add(onDeselectEntry);
        }
    }


    private void UpdateButtonAppearance(Button button, bool isSelected)
    {
        // Change the button's appearance based on whether it's selected or not
        // For example, you can change the button's color
        Color targetColor = isSelected ? Color.green : Color.white;
        button.GetComponent<Image>().color = targetColor;
    }


    private void OnWeaponButtonClicked(string weaponName)  // New method to handle button click
    {
        bool wasWeaponPicked = InventoryManager.instance.PickWeapon(weaponName, weaponManager.weaponHolder);
        if (wasWeaponPicked)
        {
            // Find the InventoryItem corresponding to the picked weapon.
            // Pass the weaponName as id to the WeaponInventoryItem constructor.
            InventoryItem newWeaponInventoryItem = new WeaponInventoryItem(weaponName);

            // Update UI and state in ThreeDPrinter
            InventoryManager.instance.UpdateInventoryUI();
            weaponSelectPanel.SetActive(false);
            isWeaponPanelActive = false;


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