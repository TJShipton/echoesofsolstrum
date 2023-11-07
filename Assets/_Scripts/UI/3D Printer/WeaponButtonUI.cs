using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponButtonUI : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI rarityText;
    public TextMeshProUGUI damageText;
    public TextMeshProUGUI weaponDescriptionText;
    public List<TextMeshProUGUI> modifierTexts;
    public Image[] modifierImages;

    public void SetModifierIcons(IWeaponModifier[] modifiers)
    {
        if (modifierImages == null || modifiers == null)
        {
            Debug.LogError("One of the arrays is null.");
            return;
        }

        // First, disable all modifier image GameObjects
        foreach (var image in modifierImages)
        {
            if (image != null) // Check if the image component is not null
            {
                image.gameObject.SetActive(false);
            }
        }

        // Now, loop through the modifiers and set the images
        for (int i = 0; i < modifiers.Length; i++)
        {
            if (i < modifierImages.Length && modifierImages[i] != null) // Check to avoid index out of range and null Image component
            {
                if (modifiers[i].ModifierIcon != null) // Check if the ModifierIcon is not null
                {
                    modifierImages[i].sprite = modifiers[i].ModifierIcon;
                    modifierImages[i].gameObject.SetActive(true); // Only activate the images we need
                }
                else
                {
                    Debug.LogError($"Modifier icon for modifier at index {i} is null.");
                }
            }
        }
    }

}
