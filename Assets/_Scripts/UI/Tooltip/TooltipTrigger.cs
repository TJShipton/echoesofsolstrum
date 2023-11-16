using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipTrigger : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    private string tooltipText; // The text to display in the tooltip.

    // Method to set the tooltip text from outside
    public void SetTooltipText(string text)
    {
        tooltipText = text;
        Debug.Log($"Tooltip text set to: {tooltipText}"); // Debug line
    }

    // This method will be called when the object becomes the selected UI element
    public void OnSelect(BaseEventData eventData)
    {
        if (!string.IsNullOrEmpty(tooltipText))
        {
            TooltipSystem.Instance.ShowTooltip(tooltipText);
        }
    }

    // This method will be called when the object is no longer the selected UI element
    public void OnDeselect(BaseEventData eventData)
    {
        TooltipSystem.Instance.HideTooltip();
    }
}
