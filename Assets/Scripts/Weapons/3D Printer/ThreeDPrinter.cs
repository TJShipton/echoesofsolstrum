using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


    public class ThreeDPrinter : MonoBehaviour
{
    public GameObject weaponSelectPanel;
    public Button weaponButtonPrefab;
    public Dictionary<string, Sprite> weaponThumbnails;
    public LayerMask clickableLayer;
    public Camera mainCamera;
    public float buttonOffsetX = 0.0f;
    public GameObject weaponTooltip;  // Reference to the WeaponTooltip panel
    public TextMeshProUGUI tooltipText;  // Reference to the TextMeshPro text field in the tooltip
    public WeaponRaritySelector weaponRaritySelector;

    private void Start()
    {
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
        // Check if the left mouse button was clicked
        if (Input.GetMouseButtonDown(0))
        {


            // Create a ray from the camera to the mouse cursor position
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);  // Use mainCamera here
            RaycastHit hit;




            // Perform the raycast with the layer mask
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, clickableLayer))
            {


                // Check if the raycast hit this 3D printer object
                if (hit.collider.gameObject == this.gameObject)
                {


                    // Show the weapon options UI
                    weaponSelectPanel.SetActive(true);
                    ShowWeaponOptions();
                }
            }

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

        // Get random 3 weapons from the list of unlocked weapons
        List<GameObject> randomWeapons = GameManager.instance.GetRandomUnlockedWeapons(3);

        // Loop through each randomly selected weapon
        foreach (GameObject weapon in randomWeapons)
        {
            // Decide the tier rarity before displaying the button
            WeaponTier randomTier = weaponRaritySelector.GetRandomTier();
            Weapon weaponComponent = weapon.GetComponent<Weapon>();
            if (weaponComponent != null)
            {
                weaponComponent.weaponData.weaponTier = randomTier;
                Debug.Log($"Assigned {randomTier} tier to {weapon.name}");  // Debug log to verify tier assignment
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
            Material borderMaterial = GetComponent<Renderer>().material;

           // Determine the outline color based on tier
            string rarityColorHex;
            switch (randomTier)
            {
                case WeaponTier.Rare: rarityColorHex = "#00DCFF"; break;  // Hex for blue
                case WeaponTier.Epic: rarityColorHex = "#FF03C1"; break;
                case WeaponTier.Legendary: rarityColorHex = "#F6FF03"; break;  // Hex for gold
                default: rarityColorHex = "#03FF08"; break;  // Hex for green (common tier)
            }
            // Convert the hex string to a Unity Color object
            Color rarityColor = ColorHelper.HexToColor(rarityColorHex);  // Assuming you have a HexToColor method in ColorHelper

            // Assign the converted Color object to the rarityText
            weaponButtonUI.rarityText.color = rarityColor;  // Set the text color

            if (imageOutline != null)  // Check if Outline component exists
            {
                imageOutline.effectColor = ColorHelper.HexToColor(rarityColorHex);  // Set the outline color using hex
            }
            else
            {
                Debug.LogWarning("Outline component is missing on ThumbnailImage prefab");
            }

            // Assign the button's click listener
            newButton.onClick.AddListener(() => PickWeapon(weapon.name));

            // Debug log for verifying the updated weapon button
            Debug.Log($"Updated button for weapon {weaponComponent.weaponData.weaponName}");
        }
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
                GameObject weaponPrefab = GameManager.instance.GetWeaponPrefabByName(weaponName);
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
    }
}

