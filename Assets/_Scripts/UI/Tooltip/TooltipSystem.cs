using TMPro;
using UnityEngine;

public class TooltipSystem : MonoBehaviour
{
    public static TooltipSystem instance;

    public GameObject modchipTooltipPanel;

    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI damageText;
    public TextMeshProUGUI rangeText;
    public TextMeshProUGUI durationText;
    public TextMeshProUGUI cooldownText;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;

        }
        else
        {
            Destroy(gameObject);
        }


        HideTooltip(); // Start with the tooltip hidden
    }


    public void ShowTooltip(ModchipData modchipData, Vector3 position)
    {


        if (modchipTooltipPanel == null || modchipData == null)
        {
            Debug.LogError("Tooltip components or data are not assigned.");
            return;
        }

        modchipTooltipPanel.SetActive(true);

        // Set individual fields
        nameText.text = modchipData.modchipName;
        descriptionText.text = modchipData.modchipDescription;
        damageText.text = $"Damage: {modchipData.modDamage}";
        rangeText.text = $"Range: {modchipData.modRange}";
        durationText.text = $"Duration: {modchipData.modDuration} seconds";
        cooldownText.text = $"Cooldown: {modchipData.modCooldown} seconds";

        UpdatePosition(position);
    }

    public void HideTooltip()
    {
        if (modchipTooltipPanel != null)
        {
            modchipTooltipPanel.SetActive(false);
        }
    }

    public void UpdatePosition(Vector3 position)
    {
        float offsetX = 150f;
        float offsetY = 10f;

        Vector3 tooltipPosition = position + new Vector3(offsetX, offsetY, 0);
        modchipTooltipPanel.transform.position = tooltipPosition;
    }



}
