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
    public Button weaponButtonPrefab;
    public Dictionary<string, Sprite> weaponThumbnails;
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


        //Image paths for weapon select buttons in 3d printer UI
        weaponThumbnails = new Dictionary<string, Sprite>();

        Sprite conductiveGloveThumbnail = Resources.Load<Sprite>("WeaponThumbnails/ConductiveGlove");
        weaponThumbnails.Add("Conductive Glove", conductiveGloveThumbnail);

        Sprite glockNSteelThumbnail = Resources.Load<Sprite>("WeaponThumbnails/GlockNSteel");
        weaponThumbnails.Add("Glock 'n' Steel", glockNSteelThumbnail);

        Sprite hyperbassFluteThumbnail = Resources.Load<Sprite>("WeaponThumbnails/HyperbassFlute");
        weaponThumbnails.Add("Hyperbass Flute", hyperbassFluteThumbnail);

        Sprite drumstickThumbnail = Resources.Load<Sprite>("WeaponThumbnails/Drumstick");
        weaponThumbnails.Add("Drumstick", drumstickThumbnail);

        // Add more thumbnails here...
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
        if (distanceToPlayer <= 5f)
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
        if (Input.GetButtonDown("Fire2"))
        {
            if (canSelectWeapon)
            {
                ToggleWeaponSelectPanel();
                canSelectWeapon = false;  // Disable weapon selection
                Invoke("EnableWeaponSelection", 2f);  // Enable it after 0.5 seconds
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
            promptText.text = "Press B Button";
        }
        else
        {
            promptText.text = "Press E Key";
        }
    }

    private void EnableWeaponSelection()
    {
        canSelectWeapon = true;
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

            // Decide the tier rarity before displaying the button
            Weapon weaponComponent = weapon.GetComponent<Weapon>();
            if (weaponComponent != null)
            {
                weaponComponent.weaponData.weaponTier = randomTier;
            }

            // Instantiate the button and parent it to the UI panel
            Button newButton = Instantiate(weaponButtonPrefab, weaponSelectPanel.transform);

            // Access and update the WeaponButtonUI component with the weapon data
            WeaponButtonUI weaponButtonUI = newButton.GetComponentInChildren<WeaponButtonUI>();
            if (weaponButtonUI != null && weaponComponent != null)
            {
                weaponComponent.ApplyTierModifiers();  // Ensure tier modifiers are applied
                weaponButtonUI.nameText.text = weaponComponent.weaponData.weaponName;
                weaponButtonUI.damageText.text = $"Damage: {weaponComponent.weaponData.baseDamage}";
                weaponButtonUI.rarityText.text = weaponComponent.GetTierName();  // Set the tier text
            }

            // Set the button image
            Image buttonImage = newButton.GetComponentInChildren<Image>();
            Outline imageOutline = buttonImage.GetComponent<Outline>();  // Get the Outline component

            if (weaponThumbnails.ContainsKey(weapon.name))
            {
                buttonImage.sprite = weaponThumbnails[weapon.name];
            }

            // Determine the outline color based on tier
            string rarityColorHex;
            switch (randomTier)
            {
                case WeaponTier.Rare: rarityColorHex = "#00DCFF"; break;  // Hex for blue
                case WeaponTier.Epic: rarityColorHex = "#FF03C1"; break;
                case WeaponTier.Legendary: rarityColorHex = "#F6FF03"; break;  // Hex for yellow
                default: rarityColorHex = "#03FF08"; break;  // Hex for green (common tier)
            }

            // Convert the hex string to a Unity Color object
            Color rarityColor = ColorHelper.HexToColor(rarityColorHex);

            // Assign the converted Color object to the rarityText
            weaponButtonUI.rarityText.color = rarityColor;  // Set the text color

            if (imageOutline != null)  // Check if Outline component exists
            {
                imageOutline.effectColor = ColorHelper.HexToColor(rarityColorHex);  // Set the outline color using hex
            }

            // Assign the button's click listener
            newButton.onClick.AddListener(() => PickWeapon(weapon.name));

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



    private void ToggleWeaponSelectPanel()
    {
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

    private void UpdateButtonAppearance(Button button, bool isSelected)
    {
        // Change the button's appearance based on whether it's selected or not
        // For example, you can change the button's color
        Color targetColor = isSelected ? Color.green : Color.white;
        button.GetComponent<Image>().color = targetColor;
    }

    private void AssignListener(Button button, string weaponName)
    {
        button.onClick.AddListener(() => PickWeapon(weaponName));
    }

    public void PickWeapon(string weaponName)
    {

        if (string.IsNullOrEmpty(weaponName))
        {
            return;
        }

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            WeaponManager weaponManager = player.GetComponent<WeaponManager>();
            if (weaponManager != null)
            {
                GameObject weaponPrefab = WeaponManager.instance.GetWeaponPrefabByName(weaponName);
                if (weaponPrefab != null)
                {
                    Weapon newWeapon = Instantiate(weaponPrefab.GetComponent<Weapon>(), weaponManager.weaponHolder);
                    newWeapon.gameObject.SetActive(false);
                    newWeapon.transform.SetParent(weaponManager.weaponHolder);

                    newWeapon.transform.localPosition = newWeapon.localPosition;
                    newWeapon.transform.localRotation = Quaternion.identity;
                    newWeapon.transform.localEulerAngles = newWeapon.localOrientation;

                    newWeapon.ApplyTierModifiers();  // Call to apply tier modifiers

                    weaponManager.availableWeapons.Add(newWeapon);
                    weaponManager.SwitchWeapon(newWeapon);
                }
            }
        }

        weaponSelectPanel.SetActive(false);

        isWeaponPanelActive = false; // Reset the flag to false

    }
}