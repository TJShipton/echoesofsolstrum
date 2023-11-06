using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class WeaponButtonCreator : MonoBehaviour
{
    public Button weaponButtonPrefab;

    public Dictionary<string, Sprite> weaponThumbnails = new Dictionary<string, Sprite>();

    public void Initialize()
    {
        weaponThumbnails.Clear(); // Clear the dictionary

        Sprite ConductiveGloveThumbnail = Resources.Load<Sprite>("WeaponThumbnails/ConductiveGlove");
        weaponThumbnails.Add("ConductiveGlove", ConductiveGloveThumbnail);

        Sprite GlockNSteelThumbnail = Resources.Load<Sprite>("WeaponThumbnails/GlockNSteel");
        weaponThumbnails.Add("GlockNSteel", GlockNSteelThumbnail);

        Sprite HyperbassFluteThumbnail = Resources.Load<Sprite>("WeaponThumbnails/HyperbassFlute");
        weaponThumbnails.Add("HyperbassFlute", HyperbassFluteThumbnail);

        Sprite DrumstickThumbnail = Resources.Load<Sprite>("WeaponThumbnails/Drumstick");
        weaponThumbnails.Add("Drumstick", DrumstickThumbnail);


    }
    void Start()
    {


    }

    public Button CreateWeaponButton(GameObject weaponPrefab, Transform parent, WeaponTier randomTier)
    {
        // Instantiate the button and parent it to the specified transform (e.g., UI panel)
        Button newButton = Instantiate(weaponButtonPrefab, parent);

        // Access and update the WeaponButtonUI component with the weapon data
        Weapon weaponComponent = weaponPrefab.GetComponent<Weapon>();
        WeaponButtonUI weaponButtonUI = newButton.GetComponentInChildren<WeaponButtonUI>();

        if (weaponButtonUI != null && weaponComponent != null)
        {
            // Apply tier and its modifiers
            weaponComponent.weaponData.weaponTier = randomTier;
            weaponComponent.ApplyTierModifiers();

            // Equip random modifiers
            weaponComponent.EquipRandomModifiers();

            // Get descriptions for each modifier to use in tooltips
            string[] modifierDescriptions = weaponComponent.equippedModifiers.Select(mod => mod.GetDescription()).ToArray();

            // Set the modifier icons and their tooltips in the UI
            weaponButtonUI.SetModifierIcons(weaponComponent.equippedModifiers.ToArray(), modifierDescriptions);


            // Update UI with the new data
            weaponButtonUI.nameText.text = weaponComponent.weaponData.weaponName;
            weaponButtonUI.damageText.text = $"{weaponComponent.weaponData.baseDamage}";
            weaponButtonUI.rarityText.text = weaponComponent.GetTierName();
            weaponButtonUI.weaponDescriptionText.text = weaponComponent.weaponData.weaponDescription;


            // Update the modifier text based on the newly equipped modifiers
            for (int i = 0; i < weaponComponent.equippedModifiers.Count; i++)
            {
                if (i < weaponButtonUI.modifierTexts.Count)
                {
                    weaponButtonUI.modifierTexts[i].text = weaponComponent.equippedModifiers[i].GetName();
                }
            }
            //Clear text if modifier not applied
            for (int i = weaponComponent.equippedModifiers.Count; i < weaponButtonUI.modifierTexts.Count; i++)
            {
                weaponButtonUI.modifierTexts[i].text = "";
            }

        }

        // Set the button image
        Image buttonImage = newButton.GetComponentInChildren<Image>();
        Outline imageOutline = buttonImage.GetComponent<Outline>();  // Get the Outline component

        // Ensure the weaponPrefab name matches the key used in the weaponThumbnails dictionary
        string weaponNameKey = weaponPrefab.name.Replace("(Clone)", "").Trim();

        if (weaponThumbnails.ContainsKey(weaponNameKey))
        {
            buttonImage.sprite = weaponThumbnails[weaponNameKey];
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

       

        // Set the weapon thumbnail
        if (weaponThumbnails.TryGetValue(weaponComponent.weaponName, out Sprite thumbnail))
        {
            weaponButtonUI.GetComponent<Image>().sprite = thumbnail;
        }



        return newButton;
    }

    public void UpdateButtonName(Button button, string newName)
    {
        // Check if either button or newName is null before proceeding
        if (button == null || newName == null)
        {
            Debug.LogError("Button or new name is null in UpdateButtonName method");
            return;  // Exit the method early if either is null
        }

        // Assumes each button has a child TextMeshProUGUI component to display the name
        TextMeshProUGUI buttonText = button.GetComponentInChildren<TextMeshProUGUI>();
        if (buttonText != null)
        {
            buttonText.text = newName;
        }
        else
        {
            Debug.LogWarning("No TextMeshProUGUI component found on button");
        }
    }


}
