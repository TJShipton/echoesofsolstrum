using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private string tooltipText; // The text to display in the tooltip.

    // Method to set the tooltip text from outside
    public void SetTooltipText(string text)
    {
        tooltipText = text;
        Debug.Log($"Tooltip text set to: {tooltipText}"); // Debug line
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!string.IsNullOrEmpty(tooltipText))
        {
            TooltipSystem.Instance.ShowTooltip(tooltipText);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TooltipSystem.Instance.HideTooltip();
    }
}
