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

    public void SetModifierIcons(IWeaponModifier[] modifiers, string[] descriptions)
    {
        if (modifierImages == null || modifiers == null || descriptions == null)
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

        // Now, loop through the modifiers and set the images and tooltips
        for (int i = 0; i < modifiers.Length; i++)
        {
            if (i < modifierImages.Length && modifierImages[i] != null) // Check to avoid index out of range and null Image component
            {
                TooltipTrigger tooltipTrigger = modifierImages[i].GetComponent<TooltipTrigger>();
                if (tooltipTrigger == null)
                {
                    // Optionally, add a TooltipTrigger component if not already attached
                    tooltipTrigger = modifierImages[i].gameObject.AddComponent<TooltipTrigger>();
                }

                if (modifiers[i].ModifierIcon != null) // Check if the ModifierIcon is not null
                {
                    modifierImages[i].sprite = modifiers[i].ModifierIcon;
                    modifierImages[i].gameObject.SetActive(true); // Only activate the images we need

                    // Set the tooltip text
                    tooltipTrigger.SetTooltipText(descriptions[i]);
                }
                else
                {
                    Debug.LogError($"Modifier icon for modifier at index {i} is null.");
                }
            }
        }
    }

}